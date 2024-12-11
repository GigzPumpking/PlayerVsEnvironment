using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HeroKnight : MonoBehaviour {

    [SerializeField] public float      m_speed = 4.0f;
    [SerializeField] float      m_currentSpeed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] int        damagePower;
    [SerializeField] float      attackRange = 2f;
    [SerializeField] LayerMask  enemyLayer;
    [SerializeField] LayerMask enemyLayer2;
    [SerializeField] GameObject m_slideDust;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor   m_groundSensor;
    private Sensor   m_wallSensorR1;
    private Sensor   m_wallSensorR2;
    private Sensor   m_wallSensorL1;
    private Sensor   m_wallSensorL2;
    private bool                m_isWallSliding = false;
    private bool                m_grounded = false;
    private int                 m_facingDirection = 1;
    private int                 m_currentAttack = 0;
    private float               m_timeSinceAttack = 0.0f;

    private AudioSource         m_audioSource;

    [SerializeField]
    private SFXClip[] m_swordSwingSounds;

    [SerializeField]
    private SFXClip m_jumpSound;

    private List<float> m_slowSources = new List<float>();

    void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor>();

        Vector3 spawnPos = GameManager.Instance.GetSpawnPoint().position;

        if (spawnPos != null)
        {
            transform.position = spawnPos;
        }

        m_animator.Play("Idle");
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance || !GameManager.Instance.GetGameState())
        {
            return;
        }

        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
            m_animator.SetBool("Moving", true);
        }

        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
            m_animator.SetBool("Moving", true);
        }
        else
        {
            m_animator.SetBool("Moving", false);
        }

        // Move
        if ((!(m_wallSensorR1.State() && m_wallSensorR2.State()) && inputX > 0) || (!(m_wallSensorL1.State() && m_wallSensorL2.State()) && inputX < 0) && m_timeSinceAttack > 0.25f)
        {
            if(m_body2d.linearVelocity.x < inputX * m_currentSpeed)
            {
                m_body2d.AddForceX(5);
            }
            else if (m_body2d.linearVelocity.x > inputX * m_currentSpeed)
            {
                m_body2d.AddForceX(-5);
            }
            //m_body2d.linearVelocity = new Vector2(inputX * m_speed, m_body2d.linearVelocity.y);
        }

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.linearVelocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State() && m_body2d.linearVelocity.x >= 0) || (m_wallSensorL1.State() && m_wallSensorL2.State() && m_body2d.linearVelocity.x <= 0);
        if (m_grounded)
        {
            m_isWallSliding = false;
        }
        m_animator.SetBool("WallSlide", m_isWallSliding);

        //Attack
        if(Input.GetKeyDown("left shift") && m_grounded && m_timeSinceAttack > 0.25f)
        {
            if (m_body2d.linearVelocity.x < 0)
            {
                m_body2d.AddForceX(7);
            }
            else if (m_body2d.linearVelocity.x > 0)
            {
                m_body2d.AddForceX(-7);
            }
            //m_body2d.linearVelocity = new Vector2(0f, m_body2d.linearVelocity.y);
            // Define the direction of the ray
            Vector2 rayDirection = new Vector2(m_facingDirection, 0f);

            // Cast a ray to detect enemies
            RaycastHit2D objectHit = Physics2D.Raycast(transform.position + new Vector3(0f, 0.5f, 0f), rayDirection, attackRange, enemyLayer);
            RaycastHit2D objectHit2 = Physics2D.Raycast(transform.position + new Vector3(0f, 0.5f, 0f), rayDirection, attackRange, enemyLayer2);

            // Visualize the ray
            // Debug.DrawRay(transform.position + new Vector3(0f, 0.5f, 0f), rayDirection * attackRange, Color.green, 3f);

            // If we hit something
            if (objectHit.collider != null)
            {
                if (objectHit.collider.CompareTag("Enemy"))
                {
                    // Damage the enemy
                    objectHit.collider.GetComponent<Health>()?.TakeDamage(damagePower);
                }
            }
            if (objectHit2.collider != null)
            {
                if (objectHit2.collider.CompareTag("Enemy"))
                {
                    // Damage the enemy
                    objectHit2.collider.GetComponent<Health>()?.TakeDamage(damagePower);
                }
            }

            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            // Play the sword swing sound clip depending on current attack
            if (m_swordSwingSounds.Length > m_currentAttack - 1)
            {
                m_audioSource.Play(m_swordSwingSounds[m_currentAttack - 1]);
            }

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }

        //Jump
        else if (Input.GetKeyDown("space") && m_grounded)
        {
            m_audioSource.Play(m_jumpSound);
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.linearVelocity = new Vector2(m_body2d.linearVelocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        UpdateCurrentSpeed();
    }

    // Animation Events
    // Called in slide animation.
    void AE_SlideDust()
    {
        Vector3 spawnPosition;

        if (m_facingDirection == 1)
            spawnPosition = m_wallSensorR2.transform.position;
        else
            spawnPosition = m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            // Set correct arrow spawn position
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }

    public void AddSlowSource(float slowMultiplier)
    {
        m_slowSources.Add(slowMultiplier);
        UpdateCurrentSpeed();
    }

    public void RemoveSlowSource(float slowMultiplier)
    {
        m_slowSources.Remove(slowMultiplier);
        UpdateCurrentSpeed();
    }

    private void UpdateCurrentSpeed()
    {
        float effectiveMultiplier = 1f;

        foreach (float multiplier in m_slowSources)
        {
            effectiveMultiplier *= multiplier;
        }

        m_currentSpeed = m_speed * effectiveMultiplier;
    }

    public void ResetSpeed()
    {
        m_slowSources.Clear();
        m_currentSpeed = m_speed;
    }
}

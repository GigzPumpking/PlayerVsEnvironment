using UnityEngine;
using System.Collections;

public class HeroKnight : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;
    [SerializeField] int        damagePower;
    [SerializeField] float      attackRange = 2f;
    [SerializeField] LayerMask  enemyLayer;
    [SerializeField] GameObject m_slideDust;

    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_HeroKnight   m_groundSensor;
    private Sensor_HeroKnight   m_wallSensorR1;
    private Sensor_HeroKnight   m_wallSensorR2;
    private Sensor_HeroKnight   m_wallSensorL1;
    private Sensor_HeroKnight   m_wallSensorL2;
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

    void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start ()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();

        Vector3 spawnPos = GameManager.Instance.GetSpawnPoint().position;

        if (spawnPos != null)
        {
            transform.position = spawnPos;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (!GameManager.Instance.GetGameState())
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
        if(!m_animator.GetBool("WallSlide") && m_timeSinceAttack > 0.25f)
        {
            m_body2d.linearVelocity = new Vector2(inputX * m_speed, m_body2d.linearVelocity.y);
        }

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.linearVelocity.y);

        // -- Handle Animations --
        //Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);

        //Attack
        if(Input.GetKeyDown("left shift") && m_grounded && m_timeSinceAttack > 0.25f)
        {
            m_body2d.linearVelocity = new Vector2(0f, m_body2d.linearVelocity.y);
            // Define the direction of the ray
            Vector2 rayDirection = new Vector2(m_facingDirection, 0f);

            // Cast a ray to detect enemies
            RaycastHit2D objectHit = Physics2D.Raycast(transform.position + new Vector3(0f, 0.5f, 0f), rayDirection, attackRange, enemyLayer);

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
}

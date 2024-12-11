using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 100;
    public float iFrames = 1f;
    private float iFrameTimer = 0f;
    private bool canBeHurt = true;
    private Transform healthBar;
    private Animator m_animator;
    [SerializeField] Vector3 relativePosition;

    [SerializeField]
    private SFXClip[] m_hurtSounds;

    private AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        healthBar = transform.Find("HealthBar");
        if(transform.CompareTag("Player"))
            m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(transform.CompareTag("Player"))
        {
            if(iFrameTimer <= 0f)
            {
                canBeHurt = true;
            }
            else
            {
                iFrameTimer -= Time.deltaTime;
            }
        }

        if(!CheckHeight())
        {
            health = 0;
            PlayerDead();
        }
    }

    public void TakeDamage(int damage)
    {
        if(canBeHurt)
        {
            health -= damage;
            if(health <= 0)
            {
                health = 0;
                if (transform.CompareTag("Player"))
                {
                    PlayerDead();
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
            else if(transform.CompareTag("Player"))
            {
                iFrameTimer = iFrames;
                m_animator.SetTrigger("Hurt");
                canBeHurt = false;
            }
            UpdateHealthBar();

            if (m_audioSource != null && m_hurtSounds != null)
            {
                // Play a random hurt sound
                m_audioSource.Play(m_hurtSounds[Random.Range(0, m_hurtSounds.Length)]);
            }
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.localScale = new Vector3(health * 0.01f, 1f, 1f);
    }

    private bool CheckHeight()
    {
        if(transform.position.y < -60f)
        {
            return(false);
        }
        return true;
    }

    private void PlayerDead()
    {
        m_animator.SetTrigger("Death");
        GetComponent<HeroKnight>().enabled = false;
        GameManager.Instance.GameEnd(false);
    }
}

using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 100;
    private Transform healthBar;
    private Animator m_animator;

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

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            health = 0;
            if (transform.CompareTag("Player"))
            {
                m_animator.SetTrigger("Death");
                GetComponent<HeroKnight>().enabled = false;
                GameManager.Instance.GameEnd(false);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        else if(transform.CompareTag("Player"))
        {
            m_animator.SetTrigger("Hurt");
        }

        UpdateHealthBar();

        if (m_audioSource != null && m_hurtSounds != null)
        {
            // Play a random hurt sound
            m_audioSource.Play(m_hurtSounds[Random.Range(0, m_hurtSounds.Length)]);
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.localScale = new Vector3(health * 0.01f, 1f, 1f);
    }
}

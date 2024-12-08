using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject playerWinPanel;
    [SerializeField] private GameObject environmentWinPanel;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;

    private void Awake()
    {
        // Ensure there's only one UIManager instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (playerWinPanel == null) playerWinPanel = transform.Find("PlayerWinPanel").gameObject;
        if (environmentWinPanel == null) environmentWinPanel = transform.Find("EnvironmentWinPanel").gameObject;
        if (pauseMenu == null) pauseMenu = transform.Find("PauseMenu").gameObject;
        HideWinUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            PauseGame(!pauseMenu.activeSelf);
        }
    }

    public void ShowPlayerWinPanel()
    {
        playerWinPanel.SetActive(true);
    }

    public void ShowEnvironmentWinPanel()
    {
        environmentWinPanel.SetActive(true);
    }

    public void HideWinUI()
    {
        playerWinPanel.SetActive(false);
        environmentWinPanel.SetActive(false);
    }

    public void PauseGame(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }
}

using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Canvas objectCanvas;

    [SerializeField] private GameObject player;

    [SerializeField] private Transform spawnPoint;

    private bool gameState = true;

    private void Awake()
    {
        // Ensure there's only one GameManager instance
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Transform GetSpawnPoint()
    {
        return spawnPoint;
    }

    public Canvas GetObjectCanvas()
    {
        return objectCanvas;
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public bool GetGameState()
    {
        return gameState;
    }

    public void SetGameState(bool gameState)
    {
        this.gameState = gameState;

        if (gameState)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }

    public void GameEnd(bool playerWin)
    {
        SetGameState(false);

        if (UIManager.Instance == null)
        {
            Debug.LogError("UIManager instance is null");
            return;
        }

        if (playerWin)
        {
            UIManager.Instance.ShowPlayerWinPanel();
        }
        else
        {
            UIManager.Instance.ShowEnvironmentWinPanel();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

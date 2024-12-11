using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Canvas objectCanvas;

    [SerializeField] private GameObject player;

    [SerializeField] private Transform spawnPoint;

    private bool gameState = true;

    [SerializeField] private bool _debug = false;

    private List<GameObject> enemies;

    [SerializeField] private SFXClip music;

    private AudioSource audioSource;

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

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.Play(music);
    }

    private void Update()
    {
        if (_debug)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                player.transform.position = spawnPoint.position;
            }
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

    public void RestartGame()
    {
        SetGameState(true);
        UIManager.Instance.HideWinUI();
        player.transform.position = spawnPoint.position;

        player.GetComponent<Health>().ResetHealth();

        if (enemies != null)
        {
            RemoveAllEnemies();
        }

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        audioSource.Play(music);
    }
    
    public void AppendEnemy(GameObject enemy)
    {
        if (enemies == null)
        {
            enemies = new List<GameObject>();
        }

        enemies.Add(enemy);
    }

    public void RemoveAllEnemies()
    {
        if (enemies == null)
        {
            return;
        }

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        enemies.Clear();
    }
}

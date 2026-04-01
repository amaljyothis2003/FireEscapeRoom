using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    public GameObject gameOverPlane;

    [Header("Win Condition")]
    public int totalFirePoints = 0;
    private int extinguishedFires = 0;
    private bool gameEnded = false;

    void Awake()
    {
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
        // Hide the game over plane at start
        if (gameOverPlane != null)
        {
            gameOverPlane.SetActive(false);
            Debug.Log("✅ GameOver plane hidden at start");
        }
        else
        {
            Debug.LogError("❌ GameOver plane is NOT assigned in GameManager!");
        }

        // Count all fire points in the scene
        FireBehaviour[] allFires = FindObjectsOfType<FireBehaviour>();
        totalFirePoints = allFires.Length;
        
        Debug.Log($"🎮 Game started with {totalFirePoints} fire points");
    }



    public void OnFireExtinguished()
    {
        if (gameEnded) return;

        extinguishedFires++;
        Debug.Log($"🔥 Fire extinguished! Progress: {extinguishedFires}/{totalFirePoints}");

        if (extinguishedFires >= totalFirePoints)
        {
            GameWon();
        }
    }

    public void OnFireReignited()
    {
        if (gameEnded) return;

        if (extinguishedFires > 0)
        {
            extinguishedFires--;
            Debug.Log($"🔥 Fire re-ignited! Progress: {extinguishedFires}/{totalFirePoints}");
        }
    }

    void GameWon()
    {
        gameEnded = true;
        Debug.Log("✅ ALL FIRES EXTINGUISHED! GAME WON!");

        // Show the game over plane
        if (gameOverPlane != null)
        {
            gameOverPlane.SetActive(true);
            Debug.Log("✅ GameOver plane shown!");
        }
        else
        {
            Debug.LogError("❌ Cannot show GameOver plane - it's not assigned!");
        }

        // Unlock cursor so player can click buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // Resume time if paused
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        Debug.Log("Game Quit");
    }
}

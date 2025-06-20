using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    [Header("Scene Settings")]
    [SerializeField] private string _mainMenuScene = "Level0";

    public string PreviousScene { get; private set; }
    public bool HasPreviousScene => !string.IsNullOrEmpty(PreviousScene);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePreviousScene();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePreviousScene()
    {
        PreviousScene = SceneManager.GetActiveScene().name;
    }

    public void LoadScene(string sceneName, bool recordCurrentScene = true)
    {
        if (recordCurrentScene)
            StoreCurrentScene();

        SceneManager.LoadScene(sceneName);
    }


    public void LoadMainMenu()
    {
        LoadScene(_mainMenuScene);
    }

    public void LoadPreviousScene()
    {
        if (HasPreviousScene)
            LoadScene(PreviousScene);
        else
            LoadMainMenu();
    }

    private void StoreCurrentScene()
    {
        PreviousScene = SceneManager.GetActiveScene().name;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}

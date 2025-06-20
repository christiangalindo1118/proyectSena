using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Botones UI globales")]
    public Button playButton;
    public Button optionsButton;
    public Button backToMenuButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignButtonListeners();
    }

    private void AssignButtonListeners()
    {
        // Recuperar referencias si no están asignadas en el Inspector
        if (playButton == null)
            playButton = GameObject.Find("PlayButton")?.GetComponent<Button>();
        if (optionsButton == null)
            optionsButton = GameObject.Find("OptionsButton")?.GetComponent<Button>();
        if (backToMenuButton == null)
            backToMenuButton = GameObject.Find("BackButton")?.GetComponent<Button>();

        // Play → Level1
        if (playButton != null)
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(() =>
            {
                PlayerPositionHandler.ClearEntry();
                SceneController.Instance.LoadScene("Level1", false);
            });
        }

        // Options → Options
        if (optionsButton != null)
        {
            optionsButton.onClick.RemoveAllListeners();
            optionsButton.onClick.AddListener(() =>
            {
                PlayerPositionHandler.ClearEntry();
                SceneController.Instance.LoadScene("Options", false);
            });
        }

        // Back to Menu → Level0
        if (backToMenuButton != null)
        {
            backToMenuButton.onClick.RemoveAllListeners();
            backToMenuButton.onClick.AddListener(() =>
            {
                PlayerPositionHandler.ClearEntry();
                SceneController.Instance.LoadScene("Level0", false);
            });
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }
}


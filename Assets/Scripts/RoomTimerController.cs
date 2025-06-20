using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class RoomTimerController : MonoBehaviour
{
    [Header("Configuración Principal")]
    [SerializeField] private float _timeLimit = 30f;
    [SerializeField] private bool _startOnAwake = true;

    [Header("Feedback Visual")]
    [SerializeField] private ProgressBar _progressBar;
    [SerializeField] private GameObject _timerWarningUI;
    [SerializeField] private float _warningThreshold = 5f;

    [Header("Eventos")]
    public UnityEvent OnTimerStart;
    public UnityEvent OnTimerEnd;
    public UnityEvent OnTimeExtended;

    private Coroutine _timerCoroutine;
    private float _currentTime;
    private bool _isTimerRunning;
    private const string DefaultScene = "Level0"; // Nunca usado, sólo en último recurso

    private void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;
        _currentTime = _timeLimit;
    }

    private void Start()
    {
        if (_startOnAwake) StartTimer();
        ToggleWarningUI(false);
    }

    public void StartTimer()
    {
        if (_isTimerRunning) return;
        _timerCoroutine = StartCoroutine(TimerRoutine());
        OnTimerStart?.Invoke();
    }

    private IEnumerator TimerRoutine()
    {
        _isTimerRunning = true;
        while (_currentTime > 0f)
        {
            _currentTime -= Time.deltaTime;
            UpdateVisualFeedback();
            if (_currentTime <= _warningThreshold)
                ToggleWarningUI(true);
            yield return null;
        }
        HandleTimerEnd();
    }

    private void HandleTimerEnd()
    {
        _isTimerRunning = false;
        OnTimerEnd?.Invoke();

        // 1) Si hay entry explícita, volvemos a esa posición
        if (PlayerPositionHandler.TryGetEntry(out string originLevel, out Vector3 entryPos))
        {
            Debug.Log($"[RoomTimer] Regresando a {originLevel} @ {entryPos}");

            Vector3 offsetPos = entryPos + new Vector3(2f, 0f, 0f);
            PlayerPositionHandler.SetEntry(originLevel, offsetPos);
            // Carga sin actualizar historial
            SceneController.Instance.LoadScene(originLevel, false);
            return;
        }

        // 2) Si no hay entry, pero SceneController tiene un PreviousScene válido
        if (SceneController.Instance.HasPreviousScene &&
            SceneController.Instance.PreviousScene.StartsWith("Level"))
        {
            string prev = SceneController.Instance.PreviousScene;
            Debug.Log($"[RoomTimer] Usando PreviousScene como fallback: {prev}");
            SceneController.Instance.LoadScene(prev, false);
            return;
        }

        // 3) Último recurso: Level0
        Debug.LogWarning("[RoomTimer] Sin datos → Level0");
        SceneController.Instance.LoadScene("Level0", false);
    }

    private void LoadSafe(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
            SceneManager.LoadScene(sceneName);
        else
            Debug.LogError($"[RoomTimer] La escena '{sceneName}' no está en Build Settings.");
    }

    private void UpdateVisualFeedback()
    {
        _progressBar?.UpdateProgress(_currentTime / _timeLimit);
    }

    private void ToggleWarningUI(bool state)
    {
        if (_timerWarningUI != null)
            _timerWarningUI.SetActive(state);
    }
}


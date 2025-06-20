using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Referencias")]
    [SerializeField] private Transform spawnPoint;    // Asignar en Inspector
    [SerializeField] private GameObject playerPrefab;

    private GameObject _playerInstance;
    private FollowPlayer _followCam;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _followCam = Object.FindAnyObjectByType<FollowPlayer>();
        }
        else Destroy(gameObject);
    }

    private void OnEnable()  => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Saltar escenas que no tienen jugador
        if (scene.name == "Level0" || scene.name == "Options") return;

        // 1) Si hay entrada de RoomTrigger:
        if (PlayerPositionHandler.TryGetEntry(out string lvl, out Vector3 pos))
        {
            Debug.Log($"[GameManager] Spawn en {lvl} @ {pos}");
            _playerInstance = Instantiate(playerPrefab, pos, Quaternion.identity);
            _followCam?.SetPlayer(_playerInstance.transform);

            // 2) Ahora sí limpiamos para el próximo Room
            
            return;
        }

        // 3) Si no, cae al spawnPoint habitual
        _playerInstance = Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        _followCam?.SetPlayer(_playerInstance.transform);
    }



    private void SpawnOrMovePlayer(Vector3 position)
    {
        if (_playerInstance == null)
        {
            _playerInstance = Instantiate(playerPrefab, position, Quaternion.identity);
            DontDestroyOnLoad(_playerInstance);
        }
        else
        {
            _playerInstance.transform.position = position;
            _playerInstance.SetActive(true);
        }

        // Vincula la cámara
        _followCam?.SetPlayer(_playerInstance.transform);
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class LevelTrigger : MonoBehaviour
{
    [Header("Configuración de la escena destino")]
    [SerializeField] private string targetLevelScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        string current = SceneManager.GetActiveScene().name;
        Debug.Log($"[RoomTrigger] Entrando a {targetLevelScene} desde {current} @ {transform.position}");
        PlayerPositionHandler.SetEntry(current, transform.position);

        SceneController.Instance.LoadScene(targetLevelScene, false);
    }
  
}

using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class RoomTrigger : MonoBehaviour
{
    [Header("Nombre de la escena del Room")]
    [SerializeField] private string targetRoomScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        string current = SceneManager.GetActiveScene().name;
        Debug.Log($"[RoomTrigger] Entrando a {targetRoomScene} desde {current} @ {transform.position}");
        PlayerPositionHandler.SetEntry(current, transform.position);

        SceneController.Instance.LoadScene(targetRoomScene, false);
    }

}




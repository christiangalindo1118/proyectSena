using UnityEngine;
using UnityEngine.SceneManagement; // Asegúrate de incluir este using

public class PlayerTeleporter : MonoBehaviour
{
    [SerializeField] private string roomSceneName; // Nombre de la escena del room
    
    // Este método se llama cuando el jugador decide entrar al room
    public void EnterRoom()
    {
        // Guardar la escena actual para regresar después
        PlayerPrefs.SetString("PreviousSceneName", SceneManager.GetActiveScene().name);
        
        // Guardar la posición actual del jugador
        PlayerPrefs.SetFloat("PreviousPosX", transform.position.x);
        PlayerPrefs.SetFloat("PreviousPosY", transform.position.y);
        PlayerPrefs.SetFloat("PreviousPosZ", transform.position.z);
        
        // Cargar la escena del room
        SceneManager.LoadScene(roomSceneName);
    }
}


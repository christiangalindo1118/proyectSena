using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("Configuración de cámara")]
    [Tooltip("Offset de la cámara respecto al player")]
    [SerializeField] private Vector3 _offset = new Vector3(0, 0, -10);

    [Header("Búsqueda del Player")]
    [Tooltip("Tag del objeto player a seguir")]
    [SerializeField] private string _playerTag = "Player";
    [Tooltip("Cada cuántos segundos reintentar encontrar al Player (0 = cada frame)")]
    [SerializeField] private float _retryInterval = 0f;

    private Transform _player;
    private float _retryTimer;

    private void LateUpdate()
    {
        // Si no tenemos aún referencia al Player, intentamos buscarlo según el intervalo
        if (_player == null)
        {
            if (_retryInterval <= 0f || Time.unscaledTime >= _retryTimer)
            {
                TryFindPlayer();
                _retryTimer = Time.unscaledTime + _retryInterval;
            }
            // Si tras el intento sigue null, salimos para no crashear
            if (_player == null) return;
        }

        // Si lo tenemos, seguimos su posición con el offset
        transform.position = _player.position + _offset;
    }

    /// <summary>
    /// Busca el objeto con tag=_playerTag y almacena su Transform.
    /// </summary>
    private void TryFindPlayer()
    {
        var go = GameObject.FindWithTag(_playerTag);
        if (go != null)
        {
            _player = go.transform;
            Debug.Log("[FollowPlayer] Player encontrado, cámara vinculada.");
        }
        else
        {
            Debug.LogWarning($"[FollowPlayer] No se encontró objeto con tag '{_playerTag}'. Reintentando...");
        }
    }

    /// <summary>
    /// Permite que el GameManager asigne manualmente el Transform del Player
    /// justo después de instanciarlo, evitando búsquedas adicionales.
    /// </summary>
    public void SetPlayer(Transform playerTransform)
    {
        _player = playerTransform;
        Debug.Log("[FollowPlayer] Player asignado manualmente, cámara vinculada.");
    }
}




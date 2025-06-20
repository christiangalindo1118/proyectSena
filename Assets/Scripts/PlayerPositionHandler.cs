using UnityEngine;

public static class PlayerPositionHandler
{
    private static string  _sceneName;
    private static Vector3 _entryPosition;
    private static bool    _hasEntry = false;

    /// <summary>
    /// Guarda de qué escena y desde qué posición entró el jugador.
    /// Debe llamarse **antes** de LoadScene(...) en tu trigger.
    /// </summary>
    public static void SetEntry(string scene, Vector3 position)
    {
        Debug.Log($"[PPH] SetEntry → scene: {scene}, position: {position}");
        _sceneName     = scene;
        _entryPosition = position;
        _hasEntry      = true;
    }

    /// <summary>
    /// Intenta recuperar la última entrada guardada.
    /// Devuelve true si había datos válidos.
    /// </summary>
    public static bool TryGetEntry(out string scene, out Vector3 position)
    {
        Debug.Log($"[PPH] TryGetEntry → hasEntry: {_hasEntry}");
        scene    = _sceneName;
        position = _entryPosition;
        return _hasEntry;
    }

    /// <summary>
    /// Limpia los datos de entrada tras ser consumidos por el GameManager.
    /// </summary>
    public static void ClearEntry()
    {
        Debug.Log("[PPH] ClearEntry");
        _hasEntry = false;
    }
}

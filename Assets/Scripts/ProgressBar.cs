using UnityEngine;
using UnityEngine.UI;
using TMPro; // Asegúrate de tener TextMeshPro instalado y referenciado

public class ProgressBar : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI progressText; // ← CAMBIO AQUÍ

    [Header("Settings")]
    [SerializeField] private bool showPercentage = true;
    [SerializeField] private string prefixText = "Tiempo: ";
    [SerializeField] private string suffixText = "%";

    public void UpdateProgress(float percentage)
    {
        // Actualizar imagen de relleno
        if (fillImage != null)
        {
            fillImage.fillAmount = Mathf.Clamp01(percentage);
        }

        // Actualizar texto con TextMeshPro
        if (progressText != null && showPercentage)
        {
            progressText.text = $"{prefixText}{percentage * 100:F0}{suffixText}";
        }
    }
}

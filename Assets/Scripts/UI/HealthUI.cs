using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image loadingBarImage;
    [SerializeField] private Health health;

    void Start() => health.currentHealth.OnValueChanged += UpdateUI;

    private void UpdateUI(int previousValue, int newValue) => loadingBarImage.fillAmount = newValue / 100f;
}

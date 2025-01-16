using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public static PlayerHealthBar Instance;
    private HealthComponent _healthComponent;
    private Image _image;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        
        _image = GetComponent<Image>();
    }

    public void SetPlayer()
    {
        _healthComponent = MainManager.Instance.myPlayer.GetComponent<HealthComponent>();

        _healthComponent.OnHealthUpdated += UpdateHealth;
    }

    private void UpdateHealth()
    {
        _image.fillAmount = (float)_healthComponent.Health / (float)_healthComponent.MaxHealth;
    }

    private void OnDestroy()
    {
        _healthComponent.OnHealthUpdated -= UpdateHealth;
    }
}

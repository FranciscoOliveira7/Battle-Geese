using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private HealthComponent _healthComponent;
    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();

        _healthComponent.OnHealthUpdated += UpdateHealth;
    }

    private void UpdateHealth()
    {
        _slider.value = _healthComponent.Health / _healthComponent.MaxHealth;
    }

    private void OnDestroy()
    {
        _healthComponent.OnHealthUpdated -= UpdateHealth;
    }
}
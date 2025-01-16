using System;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    public event Action<bool> OnCurrentInputChange;

    public ConsumableDataSO Data { get; set; } // Ensure the set accessor is public

    public float AnimationSpeedMultiplier = 1.0f;
    private int counter;

    public bool CurrentInput
    {
        get => _currentInput;
        set
        {
            if (_currentInput != value)
            {
                _currentInput = value;
                OnCurrentInputChange?.Invoke(_currentInput);
            }
        }
    }

    public GameObject BaseGameObject { get; private set; }

    public AnimationEventHandler EventHandler { get; private set; }
    public HealthComponent User { get; set; }

    private bool _currentInput;

    public void SetData(ConsumableDataSO data)
    {
        Data = data;

        counter = Data.Counter;
        // Ensure the counter is set correctly
        if (counter <= 0)
        {
            counter = 1; // Set a default value if the counter is zero or less
        }
    }

    private void Awake()
    {
        BaseGameObject = transform.Find("Base")?.gameObject;

        User = transform.parent?.GetComponent<HealthComponent>();

        EventHandler = BaseGameObject?.GetComponent<AnimationEventHandler>();
    }

    public void Use(Player player)
    {
        if (Data.Effect != null && counter > 0)
        {
            Data.Effect.ApplyEffect(player);
            counter--;

            if (counter <= 0)
            {
                // Handle the case when the consumable is depleted
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.LogWarning("Consumable effect is not set or counter is zero.");
        }
    }
}

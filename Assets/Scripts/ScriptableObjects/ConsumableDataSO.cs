using UnityEngine;

[CreateAssetMenu(fileName = "newConsumableData", menuName = "Data/Consumable Data/Consumable Data", order = 0)]
public class ConsumableDataSO : ScriptableObject
{
    public Sprite Sprite;
    [field: SerializeField] public string Name { get; set; }
    [field: SerializeField] public ConsumableEffect Effect { get; set; }
    [field: SerializeField] public int Counter { get; set; } = 1; // Ensure the counter is initialized to a non-zero value
}

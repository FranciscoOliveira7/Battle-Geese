using UnityEngine;
using UnityEngine.UI;

public class ConsumableInventoryUI : MonoBehaviour
{
    [SerializeField] private Image consumableSlot;
    [SerializeField] private Text consumableCountText; // Text for the consumable count

    private void Awake()
    {
        if (consumableSlot == null)
        {
            Debug.LogWarning("Consumable slot image is not assigned.");
        }

        if (consumableCountText == null)
        {
            Debug.LogWarning("Consumable count text is not assigned.");
        }
    }

    public void UpdateConsumableSlot(Consumable consumable)
    {
        if (consumableSlot != null)
        {
            consumableSlot.sprite = consumable != null ? consumable.Data.Sprite : null;
            consumableSlot.enabled = consumable != null;
        }

        if (consumableCountText != null)
        {
            consumableCountText.text = consumable != null ? consumable.Data.Counter.ToString() : "0";
        }
    }
}

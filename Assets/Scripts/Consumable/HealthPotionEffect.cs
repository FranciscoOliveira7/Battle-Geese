using UnityEngine;

[CreateAssetMenu(fileName = "HealthPotionEffect", menuName = "Data/Consumable Effects/Health Potion Effect", order = 0)]
public class HealthPotionEffect : ConsumableEffect
{
    [SerializeField] private float healAmount = 20f;
    //instantiate gameobject to play visual effect prefab
    [SerializeField] private GameObject visualEffect;




    public override void ApplyEffect(Player player)
    {
        //instantiate visual effect
        GameObject effect = Instantiate(visualEffect, player.transform);
        effect.transform.localPosition += Vector3.down * 0.345f;
        player.GetComponent<HealthComponent>().Health += healAmount;
        Debug.Log($"Health potion used. Player healed by {healAmount}.");
    }
}

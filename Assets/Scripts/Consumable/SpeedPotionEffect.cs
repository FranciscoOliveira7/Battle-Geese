using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedPotionEffect", menuName = "Data/Consumable Effects/Speed Potion Effect", order = 1)]
public class SpeedPotionEffect : ConsumableEffect
{
    [SerializeField] private float speedMultiplier = 1.5f;
    [SerializeField] private float duration = 5f;
    [SerializeField] private GameObject visualEffect;


    public override void ApplyEffect(Player player)
    {
        GameObject effect = Instantiate(visualEffect, player.transform);
        effect.transform.localPosition += Vector3.left * 0.26f;
        player.StartCoroutine(ApplySpeedBoost(player, effect));
    }

    private IEnumerator ApplySpeedBoost(Player player, GameObject effect)
    {
        player.StateMachine.ReusableData.BonusSpeedMultiplier *= speedMultiplier;
        Debug.Log($"Speed potion used. Player speed increased by {speedMultiplier}x for {duration} seconds.");
        yield return new WaitForSeconds(duration);
        player.StateMachine.ReusableData.BonusSpeedMultiplier /= speedMultiplier;

        Destroy(effect);
        Debug.Log("Speed boost ended.");
    }
}

using UnityEngine;

public class SpeedsterCap : HatBehaviour
{
    private float speedMultiplier = 0.25f;
    
    public override void Equip()
    {
        player.StateMachine.ReusableData.BonusSpeedMultiplier += speedMultiplier;
    }

    protected override void Unequip()
    {
        player.StateMachine.ReusableData.BonusSpeedMultiplier -= speedMultiplier;
    }
}
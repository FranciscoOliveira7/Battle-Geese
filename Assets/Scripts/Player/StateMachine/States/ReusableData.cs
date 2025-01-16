using UnityEngine;

public class ReusableData
{
    public Vector2 MovementInput;
    
    private float _baseSpeedMultiplier;

    public float SpeedMultiplier
    {
        get => _baseSpeedMultiplier * BonusSpeedMultiplier;
        set => _baseSpeedMultiplier = value;
    }
    
    // -- Hat multipliers --
    
    public float BonusSpeedMultiplier = 1f;
}

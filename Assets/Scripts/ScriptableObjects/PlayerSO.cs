using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "Data/Player")]
public class PlayerSO : ScriptableObject
{
    [field: SerializeField][field: Range(0f, 10f)] public float BaseSpeed { get; private set; }

    [field: SerializeField][field: Range(1f, 30f)] public float MoveAcceleration { get; private set; }

    [field: SerializeField][field: Range(0f, 1f)] public float AttackingMovingSpeedMultiplier { get; private set; }

    [field: SerializeField][field: Range(0f, 5f)] public float DashSpeedMultiplier { get; private set; }

    [field: SerializeField][field: Range(0f, 2f)] public float DashDuration { get; private set; }

    [field: SerializeField][field: Range(0f, 2f)] public float DashCooldown { get; private set; }

    [field: SerializeField][field: Range(0f, 2f)] public float StunDuration { get; private set; }

    [field: SerializeField][field: Range(0f, 1f)] public float RevivingSpeedMultiplier { get; private set; }

    [field: SerializeField] public Material FlashMaterial { get; private set; }
}

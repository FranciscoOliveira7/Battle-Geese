using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyHealthComponent : HealthComponent
{
    // This action is to inform the room to check if it was cleared by the player
    public GameObject Ragdoll;

    protected override void Die()
    {
        base.Die();
        Room.activeRoom.OnEnemyDeath();
        
        if (Ragdoll != null) Instantiate(Ragdoll, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

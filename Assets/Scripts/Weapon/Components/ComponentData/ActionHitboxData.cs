using System;
using UnityEngine;

public class ActionHitboxData : ComponentData<AttackActionHitbox>
{
    [field: SerializeField] public LayerMask DetectableLayers { get; private set; }

    public ActionHitboxData()
    {
        ComponentDependency = typeof(ActionHitbox);
    }

    protected override void SetComponentDependency()
    {
        ComponentDependency = typeof(ActionHitbox);
    }
}
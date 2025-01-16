using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeStatusData : ComponentData
{
    protected override void SetComponentDependency()
    {
        ComponentDependency = typeof(ChargeStatus);
    }
}

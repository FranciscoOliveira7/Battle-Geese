using UnityEngine;

public class HammoData : ComponentData<AttackHammo>
{


    public HammoData()
    {
        ComponentDependency = typeof(Hammo);
    }

    protected override void SetComponentDependency()
    {
        ComponentDependency = typeof(Hammo);
    }
}

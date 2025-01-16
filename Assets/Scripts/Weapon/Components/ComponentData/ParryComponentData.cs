
public class ParryComponentData : ComponentData<AttackParry>
{
    public ParryComponentData()
    {
        ComponentDependency = typeof(ParryComponent);
    }

    protected override void SetComponentDependency()
    {
        ComponentDependency = typeof(ParryComponent);
    }
}
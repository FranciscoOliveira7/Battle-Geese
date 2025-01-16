public class AOEComponentData : ComponentData<AttackAOE>
{
    public AOEComponentData()
    {
        ComponentDependency = typeof(AOEComponent);
    }
    
    protected override void SetComponentDependency()
    {
        ComponentDependency = typeof(AOEComponent);
    }
}
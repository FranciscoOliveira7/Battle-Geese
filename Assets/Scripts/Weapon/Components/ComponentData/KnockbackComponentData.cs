
public class KnockbackComponentData : ComponentData<AttackKnockback>
{
    public KnockbackComponentData()
    {
        ComponentDependency = typeof(KnockbackComponent);
    }

    protected override void SetComponentDependency()
    {
        ComponentDependency = typeof(KnockbackComponent);
    }
}
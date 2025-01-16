
public class DamageData : ComponentData<AttackDamage>
{
    public DamageData()
    {
        ComponentDependency = typeof(Damage);
    }

    protected override void SetComponentDependency()
    {
        ComponentDependency = typeof(Damage);
    }
}
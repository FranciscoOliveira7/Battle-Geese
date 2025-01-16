
public class EffectData : ComponentData<AttackEffect>
{
    public EffectData()
    {
        ComponentDependency = typeof(Effect);
    }

    protected override void SetComponentDependency()
    {
        ComponentDependency = typeof(Effect);
    }
}

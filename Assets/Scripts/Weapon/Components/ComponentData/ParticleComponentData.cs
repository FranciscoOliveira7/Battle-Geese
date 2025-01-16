
public class ParticleComponentData : ComponentData<AttackParticle>
{
    public ParticleComponentData()
    {
        ComponentDependency = typeof(ParticleComponent);
    }

    protected override void SetComponentDependency()
    {
        ComponentDependency = typeof(ParticleComponent);
    }
}

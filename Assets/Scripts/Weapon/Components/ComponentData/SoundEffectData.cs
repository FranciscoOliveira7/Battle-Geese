
public class SoundEffectData : ComponentData<AttackSoundEffect>
{
    public SoundEffectData()
    {
        ComponentDependency = typeof(SoundEffectComponent);
    }

    protected override void SetComponentDependency()
    {
        ComponentDependency = typeof(SoundEffectComponent);
    }
}

using UnityEngine;

public class ProjectileSpawnerData : ComponentData<AttackProjectileSpawner>
{
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float Damage { get; private set; }
    [field: SerializeField] public LayerMask DetectableLayers { get; private set; }

    public ProjectileSpawnerData()
    {
        ComponentDependency = typeof(ProjectileSpawner);
    }

    protected override void SetComponentDependency()
    {
        ComponentDependency = typeof(ProjectileSpawner);
    }
}

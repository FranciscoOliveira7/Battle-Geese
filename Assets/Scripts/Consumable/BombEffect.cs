using UnityEngine;
using UnityEngine.Serialization;
using static UnityEngine.RuleTile.TilingRuleOutput;

[CreateAssetMenu(fileName = "BombEffect", menuName = "Data/Consumable Effects/Bomb Effect", order = 0)]
public class BombEffect : ConsumableEffect
{
    public GameObject bomb;
    Vector3 target;
    private Plane _plane = new Plane(Vector3.down, 0.35f);
    public float Speed;
    public float Damage;
    public LayerMask DetectableLayers;

    public override void ApplyEffect(Player player)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (_plane.Raycast(ray, out float distance))
        {
            target = ray.GetPoint(distance);
        }
        
        MainManager.Instance.SpawnProjectile(
            bomb,
            player.transform.position,
            target,
            Speed,
            Damage,
            DetectableLayers
        );
    }
}

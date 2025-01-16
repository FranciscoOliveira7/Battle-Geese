using UnityEngine;
using System;

[Serializable]
public abstract class HatBehaviour : MonoBehaviour
{
    protected Player player;
    public HatSO data;

    private void Awake()
    {
        player = transform.parent.transform.parent.GetComponent<Player>();
    }

    private void OnDestroy() => Unequip();

    public abstract void Equip();

    protected abstract void Unequip();
}
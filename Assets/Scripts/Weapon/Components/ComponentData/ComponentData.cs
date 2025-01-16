using System;
using UnityEngine;

[Serializable]

public abstract class ComponentData
{
    public Type ComponentDependency { get; protected set; }

    public ComponentData()
    {
        SetComponentDependency();
    }

    protected abstract void SetComponentDependency();
}

[Serializable]

public abstract class ComponentData<T> : ComponentData where T : AttackData
{
    [field: SerializeField] public T AttackData { get; private set; }
}
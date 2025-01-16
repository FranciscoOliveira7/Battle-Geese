using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newItemData", menuName = "Data/Item Data", order = 0)]

public class ItemSO : ScriptableObject
{
    public Sprite Sprite;

    //public T GetData<T>()
    //{
    //    return ComponentDatas.OfType<T>().FirstOrDefault();
    //}

    //public void AddData(ComponentData data)
    //{
    //    if (ComponentDatas.FirstOrDefault(t => t.GetType() == data.GetType()) != null) return;

    //    ComponentDatas.Add(data);
    //}
}
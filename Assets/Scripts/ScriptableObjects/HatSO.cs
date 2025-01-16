using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "newHatData", menuName = "Data/Hat/Hat Data", order = 0)]

public class HatSO : ScriptableObject
{
    [field: SerializeField] public GameObject Prefab { get; private set; }
}
    
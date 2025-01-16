using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

[CustomEditor(typeof(WeaponDataSO))]
public class WeaponSOEditor : Editor
{
    private static List<Type> dataCompTypes = new();

    private WeaponDataSO _dataSO;

    private void OnEnable()
    {
        _dataSO = target as WeaponDataSO;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        foreach (var dataCompType in dataCompTypes)
        {
            if (GUILayout.Button(dataCompType.Name))
            {
                var comp = Activator.CreateInstance(dataCompType) as ComponentData;

                if (comp == null) return;

                _dataSO.AddData(comp);
            }
        }
    }

    [DidReloadScripts]
    private static void OnRecompile()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var types = assemblies.SelectMany(assembly => assembly.GetTypes());
        var filteredTypes = types.Where(
            type => type.IsSubclassOf(typeof(ComponentData))
            && !type.ContainsGenericParameters
            && type.IsClass
        );
        dataCompTypes = filteredTypes.ToList();
    }
}

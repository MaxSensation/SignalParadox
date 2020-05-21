using UnityEditor;
using UnityEngine;

public class DefaultMaterialChanger : EditorWindow
{
    private Material newMaterial;
    private string nameStartWith;
    
    [MenuItem("Window/MaterialChanger")]
    public static void ShowWindow() => GetWindow<DefaultMaterialChanger>("MaterialChanger");

    private void OnGUI()
    {
        newMaterial = (Material)EditorGUILayout.ObjectField("New Material", newMaterial, typeof(Material));
        GUILayout.Label("Change all Default-Material to new Material");
        if (GUILayout.Button("Change"))
            ChangeMaterialsStartingWith("Default-Material");
        GUILayout.Label("Change all materials starting with string");
        nameStartWith = EditorGUILayout.TextField("Name starting with:", nameStartWith);
        if (GUILayout.Button("Change"))
            ChangeMaterialsStartingWith(nameStartWith);
    }

    private void ChangeMaterialsStartingWith(string materialName)
    {
        object[] obj = FindSceneObjectsOfType(typeof (GameObject));
        var counter = 0;
        foreach (var o in obj)
        {
            var g = (GameObject) o;
            var r = g.GetComponent<Renderer>();
            if (r == null || r.sharedMaterials.Length <= 0) continue;
            var materials = r.sharedMaterials;
            for (var i = 0; i < r.materials.Length; i++)
            {
                if (!r.materials[i].name.StartsWith(materialName)) continue;
                Debug.Log("Changed material: " + r.materials[i].name + " to material: " + newMaterial);
                materials[i] = newMaterial;
                counter++;
            }
            r.sharedMaterials = materials;
        }
        Debug.Log("Total of: " + counter + " has been changed to the new material: " + newMaterial.name + " from all materials in the scene starting with: " + materialName + ".");
    }
}
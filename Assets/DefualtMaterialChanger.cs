using UnityEngine;

[ExecuteInEditMode]
public class DefualtMaterialChanger : MonoBehaviour
{
    [SerializeField] private Material materialToChangeTo;

    [ContextMenu("Change")]
    private void Change()
    {
        object[] obj = FindSceneObjectsOfType(typeof (GameObject));
        var counter = 0;
        foreach (var o in obj)
        {
            var g = (GameObject) o;
            var m = g.GetComponent<MeshRenderer>();
            if (m == null || m.sharedMaterial == null || m.sharedMaterial.name != "Default-Material") continue;
            m.material = materialToChangeTo;
            counter++;
        }
        Debug.Log(counter);
    }
}
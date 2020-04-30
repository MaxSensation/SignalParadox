using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceColliderType : MonoBehaviour
{

    public enum Mode  {Default, Grass, Dirt}
    public Mode terrainType;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public string GetTerrainType()
    {
        string typeString = "";

        switch (terrainType) {

            case Mode.Default:
                typeString = "Default";
                break;

            case Mode.Grass:
                typeString = "Grass";
                break;

            case Mode.Dirt:
                typeString = "Dirt";
                break;
            default:
                typeString = "";
                break;
                
        }

        return typeString;


    }
}

//Main author: Ferreira Dos Santos Keziah
using UnityEngine;
namespace FootStepSound
{
    public class SurfaceColliderType : MonoBehaviour
    {
        public enum Mode {Default, Grass, Dirt}
        public Mode terrainType;
    
        public string GetTerrainType()
        {
            var typeString = "";
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
}

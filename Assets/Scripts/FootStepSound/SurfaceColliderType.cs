//Main author: Ferreira Dos Santos Keziah

using UnityEngine;
namespace FootStepSound
{
    public class SurfaceColliderType : MonoBehaviour
    {
        public enum Mode {Footsteps, Grass, Dirt}
        public Mode terrainType;
    
        public string GetTerrainType()
        {
            var typeString = "";
            switch (terrainType) {
                case Mode.Footsteps:
                    typeString = "Footsteps";
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

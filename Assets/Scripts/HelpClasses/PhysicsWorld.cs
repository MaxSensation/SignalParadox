//Main author: Maximiliam Rosén

using UnityEngine;


[CreateAssetMenu(menuName = "WorldParameters")]
public class PhysicsWorld : ScriptableObject
{
    public float gravityCoefficient;
    public float airResistant;
}

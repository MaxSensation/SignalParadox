using UnityEngine;

public static class Physic3D
{
    private static float _gravityCoefficient;
    private static float _airResistant;

    public static Vector3 GetNormalForce(Vector3 velocity, Vector3 normal)
    {
        var dotProduct = Vector3.Dot(velocity, normal);
        var projection = (dotProduct > 0 ? 0 : dotProduct) * normal;
        return -projection;
    }

    public static Vector3 GetFriction(Vector3 velocity, float normalForceMagnitude, float dynamicFrictionCoefficient, float staticFrictionCoefficient)
    { 
        if (velocity.magnitude < normalForceMagnitude * staticFrictionCoefficient) return Vector3.zero;
        return velocity - velocity.normalized * (normalForceMagnitude * dynamicFrictionCoefficient);
    }

    public static float GetAirResistant()
    {
        return Mathf.Pow(1 - _airResistant, Time.deltaTime);
    }

    public static Vector3 GetAcceleration(Vector3 velocity, float accelerationSpeedCoefficient)
    {
        return velocity * (accelerationSpeedCoefficient * Time.deltaTime);
    }

    // public static Vector3 GetDeceleration(Vector3 velocity, float decelerateSpeedCoefficient, float decelerateThreshold)
    // {
    //     var decelerateVector = velocity;
    //     decelerateVector.y = 0;
    //     if (decelerateVector.magnitude > decelerateThreshold) 
    //         velocity += decelerateVector.normalized * (decelerateSpeedCoefficient * Time.deltaTime);
    //     return velocity;
    // } 
    

    public static float GetTurnVelocity(Vector3 forces, Vector3 velocity)
    {
        return Mathf.Lerp(0.1f, 0.4f, Vector3.Dot(forces.normalized, velocity.normalized));
    }

    public static Vector3 GetGravity()
    {
        return Vector3.down * (_gravityCoefficient * Time.deltaTime);
    }

    public static void LoadWorldParameters(World world)
    {
        _airResistant = world.airResistant;
        _gravityCoefficient = world.gravityCoefficient;
    }
}

using UnityEngine;

public class TwoDController : MonoBehaviour
{
    public float terminalVelocity;
    public float accelerationSpeed;
    public float decelerateSpeed;
    public float decelerateThreshold;
    public float staticFriction;
    public float dynamicFriction;
    public float airResistant;
    public float jumpHeight;
    public float gravity;
    public float skinWidth;
    public float groundCheckDistance;
    public LayerMask geometriLayer;
    private BoxCollider2D _collider;
    private Vector2 _velocity;

    private void Awake()
    {
        accelerationSpeed = 15f;
        decelerateSpeed = -1f;
        decelerateThreshold = 0.01f;
        terminalVelocity = 30f;
        jumpHeight = 9f;
        gravity = 14f;
        staticFriction = 0.6f;
        dynamicFriction = 0.4f;
        airResistant = 0.05f;
        skinWidth = 0.05f;
        groundCheckDistance = 0.1f;
        _collider = GetComponent<BoxCollider2D>();
        _velocity = Vector2.zero;
    }

    private Vector3 GetAllowedDistance(Vector2 movement)
    {
        var _hit = Physics2D.BoxCast(transform.position, _collider.size, 0, movement.normalized, float.PositiveInfinity, geometriLayer);
        if (!_hit.collider) return movement;
        var allowedDistance = _hit.distance + (skinWidth / Vector2.Dot(movement.normalized, _hit.normal));
        if (allowedDistance >= movement.magnitude) return movement;
        var normalForce = Physic3D.GetNormalForce(_velocity, _hit.normal);
        _velocity += (Vector2)normalForce;
        AddFriction(normalForce.magnitude);
        return GetAllowedDistance(_velocity * Time.deltaTime);
    }
    
    private Vector3 GetJumpVector()
    {
        if (!IsGrounded() || !Input.GetKeyDown(KeyCode.Space)) return Vector3.zero;
        return Vector2.up * jumpHeight;
    }
    
    private Vector3 GetInputVector()
    {
        return (new Vector2(Input.GetAxisRaw("Horizontal"), 0) * (accelerationSpeed * Time.deltaTime));
    }
    
    private Vector3 GetGravityVector()
    {
        return (Vector3) Vector2.down * (gravity * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, _collider.size, 0.0f, Vector2.down, groundCheckDistance + skinWidth, geometriLayer);
    }

    private void AddForce()
    {
        // Add forces
        var inputVector = GetInputVector();
        var forces = (inputVector + GetJumpVector() + GetGravityVector());
        
        // Accelerate or Decelerate
        if (inputVector.magnitude > 0) 
            Accelerate(forces);
        else 
            Decelerate();
        
        // Add forces to velocity
        _velocity += (Vector2)forces;
        
        // Add AirResistance
        _velocity *= GetAirResistance();

        // Limit vector outside collision
        transform.position += GetAllowedDistance(_velocity * Time.deltaTime);
    }

    private float GetAirResistance()
    {
        return Mathf.Pow(1 - airResistant, Time.deltaTime);
    }

    private void AddFriction(float normalForceMagnitude)
    {
        if (_velocity.magnitude < (normalForceMagnitude * staticFriction)) _velocity = Vector3.zero;
        _velocity = (_velocity - (_velocity.normalized * (normalForceMagnitude * dynamicFriction)));
    }

    private void Accelerate(Vector2 forces)
    {
        var turnSpeed = Mathf.Lerp(0.2f, 0.4f, Vector2.Dot(forces.normalized, _velocity.normalized));
        _velocity += forces * ((accelerationSpeed + turnSpeed) * Time.deltaTime);
        if (_velocity.magnitude > terminalVelocity) _velocity = _velocity.normalized * (terminalVelocity);
    }
    
    private void Decelerate()
    {
        var decelerateVector = _velocity;
        decelerateVector.y = 0;
        if (decelerateVector.magnitude > decelerateThreshold) _velocity += decelerateVector.normalized * (decelerateSpeed * Time.deltaTime);
        else _velocity.x = 0;
    }

    private void Update()
    {
        AddForce();
    }
}

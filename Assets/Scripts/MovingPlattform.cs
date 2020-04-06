using System;
using UnityEngine;

public class MovingPlattform : MonoBehaviour
{
    [SerializeField]
    private Vector3 endOffset;
    [SerializeField]
    private float speed;
    
    private Vector3 _velocity;
    private PlayerController _player;
    private float _maxDistance;
    private RaycastHit _hit;
    private bool _wasOnPlatform;
    
    private Rigidbody _rigidbody;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private bool _movingFromStartPosition;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _startPosition = transform.position;
        _movingFromStartPosition = true;
        _endPosition = _startPosition + endOffset;
        _player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        _maxDistance = (_player.GetSkinWidth() + _player.GetGroundCheckDistance()) * 4f;
        _wasOnPlatform = false;
    }


    // Update is called once per frame
    void Update()
    {
        CheckCollition();
    }


    private void FixedUpdate()
    {
        var direction = (_endPosition - transform.position).normalized;
        var magnitude = direction.magnitude * speed * Time.fixedTime;
        if (_movingFromStartPosition)
        {
            _rigidbody.AddForce(direction * magnitude);
        }
    }

    private void CheckCollition()
    {
        Physics.BoxCast(transform.position, transform.lossyScale / 2, Vector3.up, out _hit, transform.rotation, _maxDistance);
        if (_hit.collider && _hit.collider.CompareTag("Player"))
        {
            // Debug.Log("Found Player");
            _player.transform.parent = transform;
            _wasOnPlatform = true;
        }
        else
        {
            if (_wasOnPlatform)
            {
                _player.AddForce(_rigidbody.velocity);
                _player.transform.parent = null;
                _wasOnPlatform = false;
            }
        }
    }
}

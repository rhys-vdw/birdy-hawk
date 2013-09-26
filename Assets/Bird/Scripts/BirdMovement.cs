using UnityEngine;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;

public class BirdMovement : MonoBehaviour
{
    public KeyCode FlapKey = KeyCode.UpArrow;
    public string RotateAxis = "Horizontal";

    public float FlapAcceleration = 20f;
    public float MinSpeed = 5f;
    public float MaxSpeed = 50f;
    public float RotateSpeed = 20f;

    public float FlapDelay = 0.2f;

    public float Drag = 0.5f;
    public float Gravity = 10f;

    public Vector3 MaxMoveSpeed = new Vector3( 10f, 10f, 10f );
    public float MaxSpinSpeed = 10f;

    float _lastFlapTime = 0;
    float _steerDirection = 0;

    float _forwardSpeed = 0;
    float _fallSpeed = 0;

    Vector3 _velocity = Vector3.zero;

    void Update()
    {
        if( Time.time > _lastFlapTime + FlapDelay )
        {
            if( Input.GetKeyDown( FlapKey ) )
            {
                _velocity = FlapAcceleration * transform.forward;
                //_forwardSpeed += _forwardSpeed + FlapAcceleration;
                _lastFlapTime = Time.time;
                _fallSpeed = 0;
            }
        }
    }

    void LateUpdate()
    {
        _forwardSpeed -= Drag * Time.fixedDeltaTime;
        _forwardSpeed = Mathf.Clamp( _forwardSpeed, MinSpeed, MaxSpeed );

        _fallSpeed += Physics.gravity.y * Time.fixedDeltaTime;

        transform.Rotate( Input.GetAxis( RotateAxis ) * RotateSpeed * -Vector3.forward, Space.World );
        transform.Translate( Time.fixedDeltaTime * (
                    _forwardSpeed * transform.forward +
                    _fallSpeed * Vector3.up ),
                Space.World
        );
    }

    void ClampVelocity()
    {
        var velocity = rigidbody.velocity;
        for( int i = 0; i < 3; i++ )
        {
            velocity[i] = Mathf.Clamp( velocity[i], -MaxMoveSpeed[i], MaxMoveSpeed[i] );
        }
        rigidbody.velocity = velocity;
    }
}

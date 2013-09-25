using UnityEngine;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;

public class BirdMovement : MonoBehaviour
{
    public KeyCode FlapKey = KeyCode.UpArrow;
    public KeyCode SteerClockwise = KeyCode.LeftArrow;
    public KeyCode SteerAnticlockwise = KeyCode.RightArrow;
    public float FlapUpForceMagnitude = 100f;
    public float FlapForceMagnitude = 100f;
    public float FlapDelay = 0.2f;
    public float RotateSpeed = 10f;

    public Vector3 MaxMoveSpeed = new Vector3( 10f, 10f, 10f );
    public float MaxSpinSpeed = 10f;

    float _lastFlapTime = 0;
    float _steerDirection = 0;

    void Update()
    {
        _steerDirection = 0;
        if( Input.GetKey( SteerClockwise ) )     _steerDirection++;
        if( Input.GetKey( SteerAnticlockwise ) ) _steerDirection--;

        if( Time.time > _lastFlapTime + FlapDelay )
        {
            if( Input.GetKeyDown( FlapKey ) )
            {
                rigidbody.AddForce(
                    FlapForceMagnitude * transform.forward +
                    FlapUpForceMagnitude * Vector3.up
                );
            }
        }

        ClampVelocity();
    }

    void LateUpdate()
    {
        if( _steerDirection != 0 )
        {
            transform.Rotate( _steerDirection * RotateSpeed * Vector3.forward, Space.World );
        }
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

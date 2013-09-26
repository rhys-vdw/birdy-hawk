using UnityEngine;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;
using UnityBasics;
using System.Linq;
using System;

public class BirdMovement : Behavior
{
    public string RotateAxis = "Horizontal";
    public float RotateSpeed = 10f;
    public KeyCode FlapKey = KeyCode.UpArrow;

    public float FlapAcceleration = 5f;
    public float SteerSpeed = 10f;

    public float Decelleration = 2f;
    public float DecellerationScale = 0.95f;
    public float Gravity = 10f;

    Vector3 _velocity = Vector3.zero;

    Renderer[] _renderers;

    public event Action<BirdMovement> FlapEvent;

    void Awake()
    {
        _renderers = Children().Components<Renderer>().ToArray();
    }

    void Update()
    {
        if( Input.GetKeyDown( FlapKey ) )
        {
            Flap();
        }
    }

    void Flap()
    {
        _velocity += transform.forward * FlapAcceleration;

        if( FlapEvent != null )
        {
            FlapEvent( this );
        }
    }

    void FixedUpdate()
    {
        // Get component of velocity in forward direction.
        var forwardVelocity = Vector3.Project( _velocity, transform.forward );
        if( Vector3.Dot( forwardVelocity, transform.forward ) < 0f )
        {
            forwardVelocity = Vector3.zero;
        }

        Debug.DrawRay( transform.position, forwardVelocity, Color.red );

        // Check if we've rotated.
        var rotateDirection = Input.GetAxis( RotateAxis );

        if( rotateDirection != 0f )
        {
            transform.Rotate( -rotateDirection * RotateSpeed * Time.fixedDeltaTime * Vector3.forward, Space.World );

            // If we've moving fast enough, we can control our flight.
            if( forwardVelocity.magnitude > SteerSpeed )
            {
                foreach( var r in _renderers ) r.material.color = Color.red;
                _velocity -= forwardVelocity;
                _velocity += transform.forward * forwardVelocity.magnitude;
            }
            else foreach( var r in _renderers ) r.material.color = Color.white;
        }

        // Apply drag.
        var oldY = _velocity.y;
        //_velocity = Vector3.MoveTowards( _velocity, Vector3.zero, Decelleration * Time.fixedDeltaTime );
        _velocity = _velocity.normalized * _velocity.magnitude * DecellerationScale;
        if( oldY < 0 ) {
            _velocity.y = oldY;
        }

        // Apply gravity.
        _velocity += Vector3.down * Gravity * Time.fixedDeltaTime;

        // Move.
        transform.Translate( _velocity * Time.fixedDeltaTime, Space.World );
    }
}

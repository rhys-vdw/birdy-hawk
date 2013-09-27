using UnityEngine;
using UnityBasics;

public class BirdRotationController : Behavior
{
    void LateUpdate()
    {
        var dir = transform.parent.rotation.eulerAngles.x - 180f;
        print( dir );
        var up = Quaternion.Euler( dir, 0f, 0f ) * Vector3.up;
        if( up.y < 0 ) { up = up.WithY( -up.y ); }
        transform.rotation = Quaternion.LookRotation(
            transform.parent.forward,
            up
        );

        Debug.DrawRay( transform.position, up * 10f );
        //transform.localRotation = Quaternion.Euler( Vector3.forward * dir );
    }
}
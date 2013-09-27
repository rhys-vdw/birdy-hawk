using UnityEngine;
using UnityBasics;
using System.Collections.Generic;
using System.Linq;

public class TrailDetector : Behavior
{
    public TrailDetector Next = null;
    public int MinimumLoopCount = 10;

    public void StartTimeout( float duration )
    {
        Invoke( "EnableCollider", 0.5f );
        Invoke( "Repool", duration );
    }

    void EnableCollider()
    {
        collider.enabled = true;
    }

    void Repool()
    {
        Component<Poolable>().Repool();
    }

    void OnDisable()
    {
        Next = null;
        collider.enabled = false;
    }

    void OnTriggerEnter( Collider c )
    {
        var descendants = Descendants().ToArray();
        if( descendants.Length < MinimumLoopCount )
        {
            return;
        }

        foreach( var detector in descendants )
        {
            var sphere = GameObject.CreatePrimitive( PrimitiveType.Sphere );
            sphere.transform.position = detector.transform.position;
        }

        int originalLayer = gameObject.layer;
        int loopTest = LayerMask.NameToLayer( "Loop Test" );
        foreach( var detector in descendants )
        {
            detector.gameObject.layer = loopTest;
        }

        foreach( var flower in Scene.Object<FlowerManager>().Flowers )
        {
            var hits = Physics.RaycastAll(
                flower.transform.position + Vector3.right * 100f,
                Vector3.left,
                100f,
                1 << loopTest
            );

            Debug.Log( "hits: " + hits.Length );
            if( hits.Length % 2 != 0 )
            {
                flower.SetOwner( 0 );
            }
        }
    }

    IEnumerable<TrailDetector> Descendants()
    {
        var iter = this;
        while( iter != null )
        {
            yield return iter;
            iter = iter.Next;
        }
    }
}
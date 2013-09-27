using UnityEngine;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;
using UnityBasics;
using System.Linq;

public class Player : Behavior
{
    public int Number = 0;

    public int TrailLayer
    {
        get
        {
            return LayerMask.NameToLayer( "Trail " + Number );
        }
    }

    public int BirdLayer
    {
        get
        {
            return LayerMask.NameToLayer( "Bird " + Number );
        }
    }

    void Start()
    {
        foreach( var go in Descendants().Select( t => t.gameObject ) )
        {
            go.layer = BirdLayer;
        }
    }
}

using UnityEngine;
using System.Collections.Generic;
using IEnumerator = System.Collections.IEnumerator;
using UnityBasics;
using System;

public class BirdWings : Behavior
{
    void Awake()
    {
        Parent().Component<BirdMovement>().FlapEvent += FlapHandler;
    }

    void FlapHandler( BirdMovement movement )
    {
    }
}

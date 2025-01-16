using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;

public class TestBehaviourTree : BehaviourTree.Tree
{
    public Transform _location;

    public static float speed = 0.2f;

    protected override Node SetupTree()
    {
        Node _root = new TaskGoTo(transform, _location);

        return _root;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeartrapStateMachine : StateMachine
{

    public Beartrap Beartrap { get; }


    public BeartrapActiveState ActiveState { get; }
    public BeartrapInactiveState InactiveState { get; }



    public BeartrapStateMachine(Beartrap beartrap)
    {
        Beartrap = beartrap;

        ActiveState = new BeartrapActiveState(this);
        InactiveState = new BeartrapInactiveState(this);


    }


}

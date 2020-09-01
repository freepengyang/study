

//-------------------------------------------------------------------------
//Avater行为
//Author LiZongFu
//Time 2015.12.15
//-------------------------------------------------------------------------

using UnityEngine;
using System.Collections;

public abstract class BehaviourProvider
{
    public abstract bool InitializeFSM(FSMState fsm);

    public abstract void Reset();

    public delegate void OnRunOverDoSmoethingStart();

    public OnRunOverDoSmoethingStart onRunOverDoSmoethingStart;

    public delegate void OnRunOverDoSmoethingEnd();

    public OnRunOverDoSmoethingEnd onRunOverDoSmoethingEnd;
}

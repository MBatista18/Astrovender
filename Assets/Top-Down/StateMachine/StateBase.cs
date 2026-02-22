using UnityEngine;

public class StateBase
{
    StateMachineBase system;

    public StateBase(StateMachineBase _system)
    {
        system = _system;
    }

    public virtual void thisStart()
    {
    }

    public virtual void thisUpdate()
    {

    }

    public virtual void thisFixedUpdate()
    {

    }

    public virtual void thisLateUpdate()
    {

    }

    public virtual void thisEnd()
    {
    }
}

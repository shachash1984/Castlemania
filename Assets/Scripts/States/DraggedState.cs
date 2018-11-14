using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggedState : State
{
    public override void Action()
    {
        Drag();
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }

    public void Drag()
    {
        _enemy.OnDrag();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;

public class ThrownState : State
{
    public override void Action()
    {
        Throw();
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }

    public void Throw()
    {
        _enemy.OnThrow();
    }
}

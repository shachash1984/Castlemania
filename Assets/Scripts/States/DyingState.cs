using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingState : State
{
    public override void Action()
    {
        OnDying();
    }

    public override void OnEnterState()
    {
        base.OnEnterState();
    }

    public override void OnExitState()
    {
        base.OnExitState();
    }

    public void OnDying()
    {
        _enemy.OnDying();
    }
}

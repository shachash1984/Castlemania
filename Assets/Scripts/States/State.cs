using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour {

    protected Enemy _enemy;
    public virtual void OnEnterState()
    {
        _enemy = GetComponent<Enemy>();
    }
    public abstract void Action();
    public virtual void OnExitState()
    {
        Destroy(this);
    }
}

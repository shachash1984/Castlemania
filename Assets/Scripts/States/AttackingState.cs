using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingState : State
{
    #region Fields
    private int _damage;
    private float _attackSpeed;
    #endregion

    #region Unity Callbacks
    void Start()
    {
        OnEnterState();
    }
    void OnDestroy()
    {

    }
    #endregion
    public override void Action()
    {
        Attack();
    }

    public override void OnEnterState()
    {
        _enemy = GetComponent<Enemy>();
    }

    public override void OnExitState()
    {
        
    }

    public void Attack()
    {
        _enemy.Attack();
    }
}

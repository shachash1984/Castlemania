using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : State
{
    

    #region Methods
    public override void Action()
    {
        Move();
    }

    private void Move()
    {
        _enemy.Move();        
    }
    #endregion

}

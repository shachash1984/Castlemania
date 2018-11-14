using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour {

    public int HP = 100;

    public void TakeDamage(int damage)
    {
        HP -= damage;
        //GUIScript.S.SetHp(HP);
        
    }
}

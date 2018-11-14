using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCastle : ProjectileBase
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter(Collider otherCollider)
    {
     
        if (otherCollider.tag != "Bullet")
        {
            if (otherCollider.tag == "Enemy")
            {
                //otherCollider.GetComponent<EnemyControl>().TakeDamage(damage); //calling the enemy's take damage script
            }
          
            Destroy(gameObject);
        }
    }
}

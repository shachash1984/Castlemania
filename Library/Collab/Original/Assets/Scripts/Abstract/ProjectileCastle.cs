using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCastle : ProjectileBase
{
    //public override float speed { get; set; }
    //public override int damage { get; set; }

    

    

    protected override void OnTriggerEnter(Collider otherCollider)
    {
        speed = 5;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase : MonoBehaviour {

    [SerializeField]
    public virtual float speed { get; set; }
    public virtual int damage { get; set; }

    protected virtual void Start () {

        Destroy(gameObject, 5); //the projectile will die after 5 seconds
    }

    protected virtual void Update () {

        transform.Translate(Vector3.forward * speed * Time.deltaTime); //need to change the vector3 direction in according to the real view of the game
    }

    protected virtual void OnTriggerEnter(Collider otherCollider) //this function is called when the projectile hits other objects, will probably call that object's take damage function
    {
        if (otherCollider.tag != "Bullet") //the projectile ignores other bullets
        {
            /*....code goes here...
             * ...
             * ....*/

            Destroy(gameObject);
        }
    }
}

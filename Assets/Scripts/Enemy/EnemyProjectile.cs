using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

    public int speed;

    private int _damage;

    [HideInInspector]
    public EnemyBase parentEnemy;

    private GameObject _target;


    private void Start()
    {
       
    }

    private void Update()
    {
        Move();
    }

    internal void Move()
    {
        this.gameObject.transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.gameObject.GetComponent<Castle>();

        if (target != null)
        {
            StartCoroutine(parentEnemy.ApplyDamage(parentEnemy.Target));
            Destroy(this.gameObject);
        }
    }
}

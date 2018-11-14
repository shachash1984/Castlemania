using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : Projectile {

    public RealCastle castle;
    public Enemy enemyShooter;
    private Vector3 initialPos = new Vector3(2.5f, 2f, 0f);
    private Vector3 initiaRot = new Vector3(0f, 0f, 60f);
    private Vector3 initialMeshPos = new Vector3(0f, -0.431f, 0f);
    private Vector3 initialMeshRot = Vector3.zero;
    [SerializeField] private GameObject _mesh;
    

    void Start()
    {
        StartCoroutine(Shoot());
    }

    void OnTriggerEnter(Collider col)
    {
        
        if (col.transform.parent.Equals(castle.transform))
        {
            Destroy(gameObject, 1f);
        }
            
    }

    void OnDestroy()
    {
        if (castle)
            castle.TakeDamage(enemyShooter.damage);
    }
}

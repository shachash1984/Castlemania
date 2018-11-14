using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public bool isCaught = false;
    public bool isThrown = false;
    public Rigidbody rb;

    

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!isCaught && !isThrown)           
            Move();
    }

	public void Move()
    {
        Vector3 wantedPos = transform.position;
        wantedPos.x -= 0.5f;
        transform.position = Vector3.Lerp(transform.position, wantedPos, 0.1f);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<Ground>())
            isThrown = false;
        Castle c = col.gameObject.GetComponent<Castle>();
        if(c != null)
        {
            c.TakeDamage(20);
        }
       
    }
}

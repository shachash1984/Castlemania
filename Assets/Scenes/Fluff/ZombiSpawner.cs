using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiSpawner : MonoBehaviour {

    public GameObject Zombie;
    private Rigidbody _rb;

	// Use this for initialization
	void Start () {

        _rb = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if (this.transform.position.y <= -6)
        {
            _rb.useGravity = false;
            _rb.isKinematic = true;
        }


        if (Input.touchCount > 0 && Input.GetTouch(0).tapCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 11.9f));
            mousePos.z = Zombie.transform.position.z;
            Instantiate(Zombie, mousePos, Zombie.transform.rotation);
        }


	}
}

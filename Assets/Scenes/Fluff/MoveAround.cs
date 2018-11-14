using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoveAround : MonoBehaviour {

    public float maxRight;
    public float maxLeft;
    [Range(0, 1)]
    public float speedF;

    private float _speed;
    private Vector3 _origin;
    private float _timer;
    private float _rand;


	// Use this for initialization
	void Start () {

        _rand = Random.Range(this.transform.position.x - 3, this.transform.position.x + 3);
        _timer = Time.timeSinceLevelLoad;
        _origin = this.transform.position;
        _speed = speedF;
    }
	
	// Update is called once per frame
	void Update () {

        LerpToPosition();

        _timer += Time.deltaTime;

        if ((int)this.transform.position.x == (int)_rand || this.transform.position.x < maxLeft || this.transform.position.x > maxRight)
        {
            if (this.transform.position.x < maxLeft)
            {
                _rand = Random.Range(this.transform.position.x, this.transform.position.x + 4);
                return;
            }
            else if(this.transform.position.x > maxRight)
            {
                _rand = Random.Range(this.transform.position.x, this.transform.position.x - 4);
                return;
            }

            _timer = 0;
            _origin = this.transform.position;
            _rand = Random.Range(this.transform.position.x - 3, this.transform.position.x + 3);
        }
    }

    private void LerpToPosition()
    {
        if (_rand > this.transform.position.x)
        {
            this.transform.Translate(Vector3.forward * Time.deltaTime * _speed);
            this.transform.eulerAngles = new Vector3(this.transform.rotation.x, 90, this.transform.rotation.z);
        }
        else
        {
            this.transform.Translate(Vector3.forward * Time.deltaTime * _speed);
            this.transform.eulerAngles = new Vector3(this.transform.rotation.x, -90, this.transform.rotation.z);
        }
    }
}

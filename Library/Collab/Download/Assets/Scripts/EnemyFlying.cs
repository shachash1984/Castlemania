﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;

public class EnemyFlying : Enemy {

    #region Unity Callbacks
    private void Update()
    {
        _timer += Time.deltaTime;
    }
    private void Start()
    {
        rand = Random.Range(3, 10);
    }
    void OnTriggerEnter(Collider col)
    {
        if (_state is ThrownState && col.GetComponent<FlickTrigger>())
        {
            SetState(gameObject.AddComponent<DyingState>());
            rigidbody.useGravity = true;
        }
            
    }

    void OnCollisionEnter(Collision col)
    {
        if (_castle.Equals(col.gameObject.GetComponent<RealCastle>()))
            SetState(gameObject.AddComponent<AttackingState>());
        if (_state is ThrownState)
        {
            if (col.gameObject.GetComponent<Ground>())
                SetState(gameObject.AddComponent<MovingState>());
        }
        else if (_state is DyingState)
        {
            if (col.gameObject.GetComponent<Ground>())
                Die();
        }
    }

    private void MGesture_StateChanged(object sender, GestureStateChangeEventArgs e)
    {
        if (_mGesture != null && _mGesture.State == Gesture.GestureState.Ended || _mGesture.State == Gesture.GestureState.Cancelled || _mGesture.State == Gesture.GestureState.Failed)
        {
            Player.S.Flick();
            Player.S.ClearEnemy();
            SetState(gameObject.AddComponent<ThrownState>());
            _mGesture.StateChanged -= MGesture_StateChanged;
            _mGesture = null;
        }


    }
    #endregion

    #region Methods
    public override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }

    public override void Init()
    {
        _timer = Time.timeSinceLevelLoad;
        _hp = 10;
        damage = 1;
        _castle = EnemyManager.S.castle;
        SetState(gameObject.AddComponent<MovingState>());
        base.Init();
    }

    public override void OnUpdate()
    {
        _state.Action();
    }

    public override void SetState(State newState)
    {
        base.SetState(newState);
    }

    public override void Attack()
    {
        if (Time.frameCount % attackSpeed == 0)
        {
            //TODO
            //play attack animation
            StartCoroutine(PlaySFX("Attack", 0, 3));//play attack sound
            _castle.TakeDamage(damage);
        }
        else
        {
            //TODO
            //StartCoroutine(PlaySFX("Idle", 0, 3));//play Idle animation
            //play idle sounds
        }
    }

    public override void Move()
    {
        //TODO
        StartCoroutine(PlaySFX("Move", 5, 15));//play move animation
        //play move sound
        transform.Translate(Vector3.left * Time.deltaTime * speed * speedMult * EnemyManager.EnemySpeedFactor);
    }

    public override void OnDrag()
    {
        //TODO
        //play drag animation
        StartCoroutine(PlaySFX("Drag", 0, 3));//play drag sounds
        if (Input.touchCount < 1)
        {
            SetState(gameObject.AddComponent<MovingState>());
            return;
        }

        //rigidbody.isKinematic = true;
        if (_mGesture == null)
        {
            _mGesture = GetComponent<MetaGesture>();
            _mGesture.StateChanged += MGesture_StateChanged;
        }

        Vector3 wantedPos = transform.position;
        if (!float.IsNaN(_mGesture.ScreenPosition.y) && !float.IsNaN(_mGesture.ScreenPosition.x))
        {
            float touchDistanceX = cam.ScreenToWorldPoint(_mGesture.ScreenPosition).x - cam.ScreenToWorldPoint(_mGesture.PreviousScreenPosition).x;
            float touchDistanceY = cam.ScreenToWorldPoint(_mGesture.ScreenPosition).y - cam.ScreenToWorldPoint(_mGesture.PreviousScreenPosition).y;
            wantedPos += new Vector3(touchDistanceX, touchDistanceY, 0);
            transform.position = wantedPos;
        }

    }



    public override void OnThrow()
    {
        //rigidbody.isKinematic = false;
        //play thrown animation
        StartCoroutine(PlaySFX("Throw", 0, 3));//play throw sounds

    }

    public override void OnDying()
    {
        //play dying animation
        StartCoroutine(PlaySFX("Die", 0, 3));//play dying sounds
    }
    #endregion
}

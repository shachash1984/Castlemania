using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;


public class EnemyRange : Enemy
{
    #region Fields
    private Vector3 _attackPosition = Vector3.zero;
    [SerializeField] private float _attackPositionRadius = 0.5f;
    [SerializeField] private float _minDistanceFromTarget;
    [SerializeField] private float _maxDistanceFromTarget;
    [SerializeField] private GameObject _projectile;

    #endregion

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
            SetState(gameObject.AddComponent<DyingState>());
        //play falling animation
        //play falling sounds
    }

    void OnCollisionEnter(Collision col)
    {
        if (_state is ThrownState)
        {
            if (col.gameObject.GetComponent<Ground>())
            {
                SetState(gameObject.AddComponent<AttackingState>());
                _animator.SetTrigger("Land");
            }
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
        //play death animation
        StartCoroutine(PlaySFX("Die", soundWaitIntervalMinimum, soundWaitIntervalMaximum));//play death sounds
        _animator.SetTrigger("HitGround");
        Destroy(gameObject, 1f);


    }

    public override void Init()
    {
        _timer = Time.timeSinceLevelLoad;
        _hp = 10;
        damage = 10;
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
            if (!_animator.GetBool("isAttacking"))
                _animator.SetBool("isAttacking", true);

           //play attack sound
            //launching projectile via animator
            
            
        }
        else
        {
            //TODO
            if (_animator.GetBool("isAttacking"))
                _animator.SetBool("isAttacking", false);
            //play Idle animation DO NOT ACTIVATE THIS
            //play idle sounds
        }
    }

    public override void Move()
    {
        //TODO
        //play move animation
        if (_animator.GetBool("isDragged"))
            _animator.SetBool("isDragged", false);
        StartCoroutine(PlaySFX("Move", soundWaitIntervalMinimum, soundWaitIntervalMaximum));//play move sound
        if (_attackPosition == Vector3.zero)
        {
            SelectAttackingPosition();
        }            
        else if (Mathf.Abs(_attackPosition.x) - Mathf.Abs(transform.position.x) < _attackPositionRadius)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed * speedMult * EnemyManager.EnemySpeedFactor);
        }            
        else
            SetState(gameObject.AddComponent<AttackingState>());
    }

    public void SelectAttackingPosition()
    {
        _attackPosition = new Vector3(Random.Range(_minDistanceFromTarget, _maxDistanceFromTarget), 0f, transform.position.z);
    }



    public override void OnDrag()
    {
        //TODO
        //play drag animation
        rigidbody.isKinematic = true;
        if (!_animator.GetBool("isDragged"))
            _animator.SetBool("isDragged", true);
        StartCoroutine(PlaySFX("Drag", soundWaitIntervalMinimum, soundWaitIntervalMaximum));//play drag sounds
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
        rigidbody.isKinematic = false;
        if (_animator.GetBool("isDragged"))
            _animator.SetBool("isDragged", false);
        StartCoroutine(PlaySFX("Throw", soundWaitIntervalMinimum, soundWaitIntervalMaximum));//play throw sounds

    }

    public override void OnDying()
    {
        //play dying animation
        StartCoroutine(PlaySFX("Die", soundWaitIntervalMinimum, soundWaitIntervalMaximum));//play dying sounds
    }


    public void LaunchProjectile()
    {
        GameObject projectile = Instantiate(_projectile, new Vector3(transform.localPosition.x - 1.5f, transform.localPosition.y + 2.4f, transform.localPosition.z), Quaternion.Euler(new Vector3(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z + 55f)));
        ProjectileEnemy pe = projectile.GetComponent<ProjectileEnemy>();
        pe.castle = this._castle;
        pe.enemyShooter = this;
    }

    #endregion
}

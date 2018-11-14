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
        //play death animation
        //play death sounds
        Destroy(gameObject);

    }

    public override void Init()
    {
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
            //play attack sound
            //Launch projectile
            GameObject projectile = Instantiate(_projectile, new Vector3(transform.localPosition.x - 1.5f, transform.localPosition.y + 2.4f, transform.localPosition.z), Quaternion.Euler(new Vector3(transform.localRotation.x, transform.localRotation.y, transform.localRotation.z + 55f)));
            ProjectileEnemy pe = projectile.GetComponent<ProjectileEnemy>();
            pe.castle = this._castle;
            pe.enemyShooter = this;
        }
        else
        {
            //TODO
            //play Idle animation
            //play idle sounds
        }
    }

    public override void Move()
    {
        //TODO
        //play move animation
        //play move sound
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
        Debug.Log(_attackPosition);
    }



    public override void OnDrag()
    {
        //TODO
        //play drag animation
        rigidbody.isKinematic = true;
        _animator.SetBool("isDragged", true);
        //play drag sounds
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
        _animator.SetBool("isDragged", false);
        //rigidbody.isKinematic = false;
        //play thrown animation
        //play throw sounds

    }

    public override void OnDying()
    {
        //play dying animation
        //play dying sounds
    }


    #endregion
}

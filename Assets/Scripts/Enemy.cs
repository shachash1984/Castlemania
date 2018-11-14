using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;
using System.Linq;
using DG.Tweening;

public enum EnemyType
{
    Infantry = 0,
    Flying = 1,
    Range = 2
}

public enum EnemyDamageType
{
    Regular = 1,
    Splash = 2
}

public abstract class Enemy : MonoBehaviour
{


    public float speed;
    public float speedMult;
    public int goldValue;
    protected State _state;
    public int damage;
    [Tooltip("The Smaller the value, the faster the attack")]
    [Range(60, 150)]
    public int attackSpeed;
    public float flickForce;
    [SerializeField] protected int _hp;

    [Header("Assign in Editor")]
    public new Rigidbody rigidbody;
    [SerializeField] protected Animator _animator;
    [SerializeField] protected AudioSource _audioSource;
    [SerializeField] protected RealCastle _castle;
    [NamedArray(new string[] { "Attack", "Idle", "Move", "Drag", "Thrown", "Die" })]
    [SerializeField]
    protected AudioClip[] _audioClips = new AudioClip[6];
    protected MetaGesture _mGesture;
    protected Camera cam;
    // Dor Added!
    [Range(4, 15)]
    public int soundWaitIntervalMinimum = 4;
    [Range(4, 15)]
    public int soundWaitIntervalMaximum = 15;
    protected int rand;
    protected float _timer;

    public abstract void OnUpdate();
    public abstract void Attack();
    public abstract void Move();
    public abstract void OnDrag();
    public abstract void OnThrow();
    public abstract void OnDying();
    public virtual void SetState(State newState)
    {
        if (_state)
            _state.OnExitState();

        _state = newState;
        _state.OnEnterState();

    }
    private void Update()
    {
        _timer += Time.deltaTime;
    }
    public virtual void Init()
    {
        EnemyManager.S.AddEnemy(this);
        cam = Camera.main;
        if (!_animator)
            _animator = GetComponent<Animator>();
    }
    public virtual void Die()
    {
        _castle.SetGold(goldValue);
        _castle.SetEnemiesKilled();
        EnemyManager.S.RemoveEnemy(this);
        
    }

    public float GetXFlickDirection()
    {
        return Input.touches.Length > 0 ? Input.touches[0].deltaPosition.x : 1;
    }

    public IEnumerator PlaySFX(string clipName, int WaitIntervalMinimum, int WaitIntervalMaximum,
    AudioManager.PlayMode playMode = AudioManager.PlayMode.PlayFirstDelayed)
    {
        yield return null;

        var sfx = _audioClips.FirstOrDefault(aClip => aClip.name.Contains(clipName));

        if (sfx == null || _audioSource.isPlaying && _audioSource.clip == sfx || _timer < rand && _audioSource.clip == sfx)
        {
            yield break;
        }

        else if (_timer > rand || _audioSource.clip != sfx)
        {
            _audioSource.clip = sfx;
            _audioSource.PlayOneShot(sfx);

            _timer = 0;

            rand = Random.Range(WaitIntervalMinimum, WaitIntervalMaximum);
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using TouchScript.Gestures;



[RequireComponent(typeof(Animator), typeof(AudioSource), typeof(Rigidbody))]
[RequireComponent(typeof(MetaGesture))]
[HelpURL("https://www.facebook.com/SGDickDesigner")]
public class EnemyBase : MonoBehaviour
{

    #region Fields

    [Header("Attributes")]
    [SerializeField]
    private int _hp = 10;
    [SerializeField]
    private int _damage = 1;
    public EnemyDamageType DamageType = EnemyDamageType.Regular;
    [SerializeField]
    [Range(1, 15)]
    [Tooltip("Range is measured from 2 to 15. 2 means melee, 15 means highest distance from castle. the RANGE is actualy 10 sections of the distance between the edge of the screen and the castle.")]
    private float _range = 2f;
    [SerializeField]
    [Tooltip("Larger number = larger attack speed.")]
    private float _attackSpeed = 1;
    [SerializeField]
    [Tooltip("Armor is ranged 1 - 10. 1 signifies no reduction, 2 signifies half damage, 3 third etc.")]
    [Range(0, 10)]
    private float _armor = 0;
    //[SerializeField]
    //[Tooltip("Level represents the overall strength of he enemy in accordance to its attributes")]
    //private int _level = 1;
    [SerializeField]
    [Tooltip("Represents damage done as a percentage of full hp of the actor")]
    [Range(0, 100)]
    private float _fallDamage = 100;
    [SerializeField]
    [Range(0.1f,7)]
    private float Speed = 1;
    private float speedMult = 1f;
    [HideInInspector]
    public bool isCaught = false;

    public EnemyType Type = EnemyType.Infantry;

    [Space]

    public bool IsLiftable = true;
    //[SerializeField]
    //private int _lane;
    [TextArea]
    public string Name = "Default Enemy";

    [Header("Audio Settings")]
    [NamedArray(new string[] { "Attack" , "Getting Hit", "Grabbed", "Die", "Voice01", "Voice02", "Voice03" })]
    public AudioClip[] AudioClips = new AudioClip[7];
    [Range(4,15)]
    public int soundEffectWaitIntervalMinimum;
    [Range(4, 15)]
    public int soundEffectWaitIntervalMaximum;


    private Animator _animator;
    private AudioSource _audioSource;
    private bool isDead;
    public int _maxHP { get; private set; }
    private int _score;
    [HideInInspector]
    public Rigidbody rB;

    private float _dPS;

    public float maxDistanceFromCastle = 15;
    private GameObject _castlePrefab;

    private RealCastle _castle;
    private bool canPlaySound = true;
    private float initializationTime;
    private bool _canAttack;
    public GameObject projectile;
    private EnemyProjectile _projectile;
    public GameObject Target { get; private set; }
    [HideInInspector]
    public bool isThrown;
    private bool _isDying = false;
    private bool _isAttacking = false;
    #endregion

    #region Properties
    public float DPS
    {
        get { return _dPS; }
        set
        {
            _dPS = _damage * _attackSpeed * Time.deltaTime;
        }
    }
    #endregion

    #region Unity Local Methods
    private void Start()
    {
        Init();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isThrown)
        {
            var ground = collision.gameObject.GetComponent<Ground>();

            FallGround(ground);
            isThrown = false;
        }
        else if (collision.gameObject == _castle.gameObject)
        {
            _isAttacking = true;
        }

    }
    #endregion

    #region Methods

    /// <summary>
    /// Decreases the damage from the hp of the actor
    /// </summary>
    /// <param name="damage">amount of damage to recieve</param>
    public void RecieveDamage(int damage)
    {
        Animate(AnimationManager.AnimationState.GetHit);

        damage = (int)(damage / _armor);
        this._hp -= damage;
    }
    /// <summary>
    /// Recieves damamge as a percentage of this actors hp
    /// </summary>
    /// <param name="damagePercent">0 for no dmg, 100 for full dmg</param>
    public void RecieveDamage(float damagePercent = 100)
    {
        if (damagePercent < 0 || damagePercent > 100)
            damagePercent = 100;

        Animate(AnimationManager.AnimationState.GetHit);
        this._hp -= (int)(_maxHP * damagePercent / 100f);
    }
    /// <summary>
    /// Initializes the actor
    /// </summary>
    public void Init()
    {
        // EDIT: should get number of lanes from Game Manager / Spawn Manager
        //_castlePrefab = EnemyManager.S.castle;
        //int random = Random.Range(1, 5);
        //_lane = random;
        speedMult = Random.Range(0.5f, 1.5f);
        isDead = false;
        isCaught = false;
        _animator = this.GetComponent<Animator>();
        _audioSource = this.GetComponent<AudioSource>();
        _maxHP = this._hp;
        //EnemyManager.AddEnemy(this);
        this.rB = this.GetComponent<Rigidbody>();
        _canAttack = true;
        this.Target = this._castlePrefab;
        isThrown = false;

        if (projectile != null)
        {
            _projectile = projectile.GetComponent<EnemyProjectile>();
        }

        _castle = _castlePrefab.GetComponent<RealCastle>();

        initializationTime = Time.timeSinceLevelLoad;

        rB.freezeRotation = true;
    }
    /// <summary>
    /// A generic method for animating the actor
    /// </summary>
    /// <param name="aT">Choose an animate state from a singleton</param>
    public void Animate (AnimationManager.AnimationState aT)
    {
        //string animationName = aT.ToString();
        //_animator.Play(animationName);
    }
    /// <summary>
    /// A generic method for playing the enemies sound effect
    /// </summary>
    /// <param name="sfx">The given sfx should be used within the enemies' own audioclip array</param>
    /// <returns></returns>
    public IEnumerator PlaySFX(AudioClip sfx, AudioManager.PlayMode playMode = AudioManager.PlayMode.PlayFirstDelayed)
    {
        // PLaySFX should be rewritten, too messy
        // Checks if given SFX is null or the same sfx is trying to play again >> Upload Sound to source >> plays it >>  waits >> empties source
        if (sfx == null || _audioSource.isPlaying)
        {
            canPlaySound = false;
            yield break;
        }

        else if (canPlaySound || playMode == AudioManager.PlayMode.Override)
        {
            var rand = Random.Range(soundEffectWaitIntervalMinimum, soundEffectWaitIntervalMaximum);

            if (playMode == AudioManager.PlayMode.PlayFirstDelayed && Time.timeSinceLevelLoad - initializationTime < rand - soundEffectWaitIntervalMinimum/2) // for first instanciation
            {
                yield break;
            }
            _audioSource.clip = sfx;
            _audioSource.Play();
            
            if (playMode != AudioManager.PlayMode.Override)
            {
                yield return new WaitForSeconds(rand);
                yield return new WaitUntil(() => !_audioSource.isPlaying);
            }

            canPlaySound = true;
        }
    }
    /// <summary>
    /// Provides enemy logic for each frame.
    /// </summary>
    public void OnUpdate()
    {
        // Check death all enemy
        // isCaught == true, isKinematic = true
        // isCaught == false, isKinemtic = false + MOVE()
        // Move to all enemy
        if (!_isDying)
        {
            if (_hp <= 0)            
                isDead = true;
            if (isDead)
            {
                _isDying = true;
                StartCoroutine(Die());
            }
            else if (_isAttacking && _castle)
                Attack(_castle.gameObject);
            else if (isCaught)
                Lifted();
            else if (!isCaught && !isThrown)
            {
                rB.isKinematic = false;
                Move();
            }
        }
        
    }
    /// <summary>
    /// Is the the target within the actor's range?
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private bool IsWithinRange(GameObject target)
    {
        float distance = target.transform.position.x - transform.position.x;
        if (distance <= _range)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// Applies damage to castle
    /// </summary>
    /// <returns></returns>
    internal IEnumerator ApplyDamage(GameObject target)
    {
        if (target == _castlePrefab || target.tag == "Castle" || target.tag == "Player")
        {
            if (_range == 2) // infantry
            {
                if (_canAttack)
                {
                    _castle.TakeDamage(_damage);
                    _canAttack = false;
                    yield return new WaitForSeconds(1f / _attackSpeed);
                    _canAttack = true;
                    yield break;
                }
                else
                    yield break;
            }
            else // range
            {
                _castle.TakeDamage(_damage);
            }
        }
    }
    internal IEnumerator Shoot()
    {
        if (_canAttack)
        {
            var proj = Instantiate(this.projectile, this.transform.position, this.transform.rotation);
            var projScript = proj.GetComponent<EnemyProjectile>();
            projScript.parentEnemy = this;

            _canAttack = false;

            yield return new WaitForSeconds(1f / _attackSpeed);

            _canAttack = true;

            yield break;
        }
        else
            yield break;
    }
    internal void FallGround(Ground ground)
    {
        if (ground != null)
        {
            StartCoroutine(PlaySFX(AudioClips.FirstOrDefault(clip => clip.name.Contains("Hit"))));
            RecieveDamage(this._fallDamage);
        }
    }

    #endregion

    #region States

    public IEnumerator Die()
    {
        // Stops enemy from doing anything >> Plays death Animation >> Plays death sound >> waiting until animation is over >> Remove itself from enemy manager >> destroying the object
        isDead = true;
        Animate(AnimationManager.AnimationState.Die);
        StartCoroutine(PlaySFX(AudioClips.FirstOrDefault(aClip => aClip.name.Contains("Die")))); // play sfx that contains the word Dead
        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName("Die")); // waiting until death has finished
        //EnemyManager.RemoveEnemy(this);
        Destroy(this.gameObject);

        yield return null;
    }
    public void Move()
    {
        StartCoroutine(PlaySFX(AudioClips.FirstOrDefault(aClip => aClip.name.Contains("Voice01"))));
        this.transform.Translate(Vector3.left * Time.deltaTime * Speed * speedMult * EnemyManager.EnemySpeedFactor);
        Animate(AnimationManager.AnimationState.Move);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="target">the target to be attacked</param>
    /// <returns>did the enemy attack or not?</returns>
    public bool Attack(GameObject target)
    {
        if (IsWithinRange(target) && _range == 1) // none ranged unit
        {
            StartCoroutine(PlaySFX(AudioClips.FirstOrDefault(aClip => aClip.name.Contains("Attack")), AudioManager.PlayMode.Override));
            Animate(AnimationManager.AnimationState.Attack);
            StartCoroutine(ApplyDamage(target));
            return true;
        }
        else if (IsWithinRange(target) && _range > 2 && _projectile != null) // ranged unit
        {
            StartCoroutine(PlaySFX(AudioClips.FirstOrDefault(aClip => aClip.name.Contains("Attack")), AudioManager.PlayMode.Override));

            //var v = new Vector3(this.transform.position.x - 3, this.transform.position.y, this.transform.position.z);

            StartCoroutine(Shoot());

            Animate(AnimationManager.AnimationState.Attack);

            return true;
        }
        return false;
    }
    public void Lifted()
    {
        Animate(AnimationManager.AnimationState.Idle);
        StartCoroutine(PlaySFX(AudioClips.FirstOrDefault(clip => clip.name.Contains("Grabbed"))));
    }


    #endregion
}

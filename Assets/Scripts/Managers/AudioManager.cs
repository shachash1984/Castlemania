using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    private AudioSource _audioSource;
    public AudioClip LoopMusic;

    public enum EnemyState
    {
        Attack = 1,
        Hit = 2,
        Grabbed = 3,
        Die = 4,
        Voice01 = 5,
        Voice02 = 6,
        Voice03 = 7,
        Move = 8
    }

    public enum PlayMode
    {
        PlayOnAwake = 1,
        PlayFirstDelayed = 2,
        Override = 3,
    }

    // Use this for initialization
    void Start()
    {
        _audioSource = this.GetComponent<AudioSource>();
        _audioSource.loop = true;
        _audioSource.clip = LoopMusic;
        _audioSource.playOnAwake = true;
        _audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
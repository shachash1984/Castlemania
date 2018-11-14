using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class Projectile : MonoBehaviour {

    [SerializeField] protected float _force;
    [SerializeField] protected Rigidbody _rigidbody;
    

    public virtual IEnumerator Shoot()
    {
        _rigidbody.isKinematic = true;
        transform.DOLocalMove(new Vector3(transform.localPosition.x + 0.75f, transform.localPosition.y - 0.5f,transform.localPosition.z), 0.5f);
        
        yield return new WaitUntil(() => !DOTween.IsTweening(transform));
        _rigidbody.isKinematic = false;
        _rigidbody.AddRelativeForce(new Vector3(0f, 0.8f, 1f) * _force, ForceMode.Impulse);
    }
    
   

}

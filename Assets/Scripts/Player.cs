using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TouchScript.Gestures;

public class Player : MonoBehaviour
{
    #region Fields
    static public Player S;
    [SerializeField] float flickForce = 50f;
    private Ray ray;
    private Camera cam;
    private RaycastHit hit;
    [SerializeField] private Enemy enemy;
    private float touchDistance;
    #endregion

    #region Unity Callbacks
    void Awake()
    {
        if (S != null)
            Destroy(gameObject);
        S = this;
    }

    void Start()
    {
        cam = Camera.main;
    }

    void FixedUpdate()
    {
        if (Input.touchCount > 0)
            GrabEnemy();
        else
            ClearEnemy();
        
        
    }
    #endregion

    

    #region Methods
    public void GrabEnemy()
    {
        ray = cam.ScreenPointToRay(Input.touches[0].position);
        if (enemy == null && Physics.Raycast(ray.origin, cam.transform.forward, out hit, 50f))
        {
            enemy = hit.collider.GetComponent<Enemy>();
            if (enemy)            
                enemy.SetState(enemy.gameObject.AddComponent<DraggedState>());
            
        }
    }

    public void ClearEnemy()
    {
        enemy = null;
    }

    public void Flick()
    {
        if (enemy != null)
        {
            enemy.rigidbody.isKinematic = false;
            
            enemy.rigidbody.AddForce(new Vector3(0.25f*Mathf.Sign(enemy.GetXFlickDirection()), Input.touches[0].deltaPosition.normalized.y, 0f) * flickForce, ForceMode.Impulse);
        }
            
    }
    #endregion
    
}
       
    
    

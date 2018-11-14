using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealCastle : MonoBehaviour
{
    public int hp;
    public int gold;
    public int TimeLasted;
    public int enemiesKilled;
    private GameObject fortification;
    ProjectileCastle CastleProjectile;

    // Use this for initialization
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hp <= 0)
            Die();
    }

    public void SetTimeLasted()
    {
        TimeLasted = (int)Time.time;
    }

    public void SetEnemiesKilled()
    {
        ++enemiesKilled;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        GUIScript.S.SetHp(hp);
        StartCoroutine(GUIScript.S.DamageFlash());
    }

    void init()
    {
        hp = 1000;
        gold = 0;
        enemiesKilled = 0;
        GUIScript.S.SetHp(hp);
        GUIScript.S.SetGold(0);
    }

    void Die()
    {
        StopCoroutine(GUIScript.S.DamageFlash());
        SetTimeLasted();
        GUIScript.S.SetTimeText();
        GUIScript.S.SetEnemiesKilledText();
        GUIScript.S.GameOver();
        Destroy(gameObject, 0.5f);
    }  
    
    public void SetGold(int addedGold)
    {
        gold += addedGold;
        GUIScript.S.SetGold(gold);
    }
}
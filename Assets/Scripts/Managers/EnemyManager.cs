using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager S;
    private static readonly List<Enemy> Enemies = new List<Enemy>();

    public RealCastle castle;

    public const int EnemySpeedFactor = 1;

    void Awake()
    {
        if (S != null)
            Destroy(gameObject);
        S = this;
        castle = FindObjectOfType<RealCastle>();
    }

    void FixedUpdate()
    {
        foreach (Enemy enemy in Enemies)
        {
            enemy.OnUpdate();
        }
    }

    void OnDestroy()
    {
        Enemies.Clear();
    }


    #region Methods
    public void AddEnemy(Enemy e)
    {
        Enemies.Add(e);
    }
    public void RemoveEnemy(Enemy e)
    {
        Enemies.Remove(e);
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Spawn
{
    public float spawnTime;
    public EnemyType enemyToSpawn;
    public Lane lane;

}

[CreateAssetMenu(fileName = "Wave", menuName = "Wave", order = 1)]
public class Wave : ScriptableObject {

    public GameObject[] enemyPrefabs;
    //[SerializeField] private int[] lanes = new int[] { -9, -8, -7, -6, -5, -4, -3, -2 - 1, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    [Space]
    // Dor Added
    [Header("AUTO SPAWN FEATURES")]
    public bool AutoSpawn;
    public int NumOfInfantry;
    public int NumOfRange;
    public int NumOfFlying;
    //
    [Space]
    public float waveStart;
    public float waveEnd;
    public float waveLoop;
    public Spawn[] spawns;
    [HideInInspector] public int currentSpawn = 0;
    //private List<int> occupiedLanes = new List<int>();

}

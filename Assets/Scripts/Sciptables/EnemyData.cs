
using UnityEngine;
using System.Collections.Generic;
using System;
[CreateAssetMenu(fileName = "EnemyData" , menuName = "ScriptableObject/EnemyData")]
public class EnemyData : ScriptableObject {
    public String id;
    public List<StatsForEnemy> StatsForEnemies;
}



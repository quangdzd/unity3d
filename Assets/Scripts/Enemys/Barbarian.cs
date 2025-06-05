using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barbarian : AEnemy
{
    private StatsForEnemy _barbarianStats;

    protected override void Awake() {
        base.Awake();
        
        _barbarianStats = AllEnemyData.Instance.FindEnemyById("2").StatsForEnemies.Find(e => e._name == "Barbarian");
        SetupStat(_barbarianStats);
    }

    protected override void Start()
    {
        base.Start();
        SetupStat(_barbarianStats);

    }
}

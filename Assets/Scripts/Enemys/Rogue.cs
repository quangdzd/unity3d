using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue : AEnemy
{
    private StatsForEnemy _rogueStats;

    protected override void Awake() {
        base.Awake();
        _rogueStats = AllEnemyData.Instance.FindEnemyById("2").StatsForEnemies.Find(e => e._name == "Rogue");
        SetupStat(_rogueStats);
    }

    protected override void Start()
    {
        base.Start();
        SetupStat(_rogueStats);

    }
}

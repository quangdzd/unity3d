using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RougueHooded : AEnemy
{
    private StatsForEnemy _rogueHood;

    protected override void Awake() {

        base.Awake();

        _rogueHood = AllEnemyData.Instance.FindEnemyById("2").StatsForEnemies.Find(e => e._name == "RougueHooded");
        SetupStat(_rogueHood);
    }

    protected override void Start()
    {
        base.Start();


    }
}

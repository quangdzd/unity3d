using System;
using UnityEngine;

public class EnemyHPManager : AHPManager
{
    private AEnemy aEnemy;
    protected override void Awake()
    {
        base.Awake();
        aEnemy = GetComponent<AEnemy>();
    }

    public void SetOriginHp(float hp)
    {
        originHp = hp;
    }
    public float GetHP()
    {
        return currentHp;
    }

    public override void TakeDame( float dmg)
    {
        currentHp = Math.Max(0 , currentHp-dmg);
        // Debug.Log("Tao la " + this.gameObject.name + "con lai " + currentHp + "vi da nhan " + dmg);

        if (currentHp == 0)
        {
            aEnemy.ChangeStateToDeathState();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IState
{
    public void Enter(AEnemy enemy)
    {
        enemy.AttackStateEnter();
    }

    public void Exit(AEnemy enemy)
    {
        enemy.AttackStateExit();
    }

    public void FixedUpdate(AEnemy enemy)
    {
        enemy.AttackStateFixedUpdate();
    }

    public void Update(AEnemy enemy)
    {
        enemy.AttackStateUpdate();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakedameState : IState
{
    public void Enter(AEnemy enemy)
    {
        enemy.TakedameStateEnter();
    }

    public void Exit(AEnemy enemy)
    {
        enemy.TakedameStateExit();
    }

    public void FixedUpdate(AEnemy enemy)
    {
        enemy.TakedameStateFixedUpdate();
    }

    public void Update(AEnemy enemy)
    {
        enemy.TakedameStateUpdate();
    }
}

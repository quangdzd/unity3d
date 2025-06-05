using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : IState
{
    public void Enter(AEnemy enemy)
    {
        enemy.DeathStateEnter();
    }

    public void Exit(AEnemy enemy)
    {
        enemy.DeathStateExit();
    }

    public void FixedUpdate(AEnemy enemy)
    {
        enemy.DeathStateFixedUpdate();
    }

    public void Update(AEnemy enemy)
    {
        enemy.DeathStateUpdate();
    }

}

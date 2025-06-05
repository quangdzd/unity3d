
using UnityEngine;

public class FindTargetState : IState
{
    public void Enter(AEnemy enemy)
    {
        enemy.FindStateEnter();
    }

    public void Exit(AEnemy enemy)
    {
        enemy.FindStateExit();
    }

    public void FixedUpdate(AEnemy enemy)
    {
        enemy.FindStateFixedUpdate();
    }

    public void Update(AEnemy enemy)
    {
        enemy.FindStateUpdate();
    }
}


public class IdleState : IState
{
    public void Enter(AEnemy enemy)
    {
        enemy.IdleStateEnter();
    }

    public void Exit(AEnemy enemy)
    {
        enemy.IdleStateExit();
    }

    public void FixedUpdate(AEnemy enemy)
    {
        enemy.IdleStateFixedUpdate();
    }

    public void Update(AEnemy enemy)
    {
        enemy.IdleStateUpdate();
    }
}

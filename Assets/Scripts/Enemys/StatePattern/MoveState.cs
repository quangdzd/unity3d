
public class MoveState : IState
{
    public void Enter(AEnemy enemy)
    {
        enemy.MoveStateEnter();
    }

    public void Exit(AEnemy enemy)
    {
        enemy.MoveStateExit();
    }

    public void FixedUpdate(AEnemy enemy)
    {
        enemy.MoveStateFixedUpdate();
    }

    public void Update(AEnemy enemy)
    {
        enemy.MoveStateUpdate();
    }
}

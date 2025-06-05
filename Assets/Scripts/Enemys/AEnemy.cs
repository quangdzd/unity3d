using System;
using System.Threading;
using Pathfinding;
using UnityEditor.SceneManagement;
using UnityEngine;
// using Random = UnityEngine.Random;

[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(EnemyHPManager))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(MoneyManager))]




public abstract class AEnemy : MonoBehaviour , IEnergy 
{
   protected Rigidbody rb;
   protected Animator animator;
   protected IState _currentState;
   protected AttackState attackState = new AttackState();
   protected TakedameState takedameState = new TakedameState();
   protected IdleState idleState= new IdleState();
   protected MoveState moveState= new MoveState();
   protected DeathState deathState= new DeathState();
   protected FindTargetState findTargetState= new FindTargetState();
   protected EnemyHPManager enemyHPManager;
   protected MoneyManager moneyManager;
   protected AstarPath astarPath;
   protected Seeker seeker;


   protected AIPath aiPath;

   public Transform target;
   protected Vector3 moveDir;
   protected EnemyHPManager targetHPManage;

   protected int _index;
   public int Index
   {
      get { return _index; }
      set 
      { 
         if (value >= 0)
               _index = value; 
         else
               Debug.LogError("Index không thể là số âm.");
      }
   }



   
   protected float dmg;
   protected float hp;
   protected float attackSpeed;

   protected float attackThreshold;
   protected float speed;


   protected float timer;
   protected float rotationSpeed;
   protected bool canGotoTarget;


   protected virtual  void Awake() {
      animator =  GetComponent<Animator>();
      rb = GetComponent<Rigidbody>();
      enemyHPManager = GetComponent<EnemyHPManager>();
      moneyManager = GetComponent<MoneyManager>();
      if (moneyManager == null)
      {
         moneyManager = gameObject.AddComponent<MoneyManager>();
      }

      aiPath = GetComponent<AIPath>();
      astarPath = GameObject.Find("A*").GetComponent<AstarPath>();
      seeker = GetComponent<Seeker>();

      target = null;
      rotationSpeed = 5f;
      timer = 0;

      _currentState = idleState;
      aiPath.canMove = false;
   }

   protected virtual void Start()  
   {
      seeker.drawGizmos = false;
      GameManager.Instance.OnPlayStarted += FindTarget;
   }
    protected virtual void OnDestroy()
    {
      GameManager.Instance.OnPlayStarted -= FindTarget;
    }
   protected void SetupStat(StatsForEnemy statsEnemy)
   {
      hp = statsEnemy._hp;
      dmg = statsEnemy._dmg;
      attackSpeed = statsEnemy._attackSpeed;
      attackThreshold = statsEnemy._attackThreshold;
      speed = statsEnemy._speed;

      enemyHPManager.SetOriginHp(hp);
      enemyHPManager.HealFullHp();

      moneyManager.SetCost(statsEnemy._coin);

      moneyManager.SetReward(statsEnemy._coin);

      // Debug.Log("Cost" + moneyManager.GetCost());
   }
   public virtual void RechargeEnergy(int energy)
   {

   }
   public virtual void ReduceEnergy(int energy)
   {

   }

   private void ScanGraph()
   {
      // astarPath.Scan();
   }

   protected virtual bool CanGotoTarget()
   {
      return aiPath.GetSearchNode() != 0;
   }
   protected virtual void FindTarget()
   {
      Transform target = EntityManager.Instance.FindTargetClosest(gameObject);

      if(!target)
      {
         ChangeStateToIdleState();
      }
      else if( target)
      {
         ChangeTarget(target);

         if(IsTargetInAttackThreshold())
         {
            ChangeStateToAttackState();
            return;
         }
         
         ChangeStateToMoveState();
      }

   }



   protected virtual void Update() 
   {
      _currentState.Update(this);

   }



   protected virtual void FixedUpdate()
   {
      _currentState.FixedUpdate(this);
   }


   protected virtual void FlipSpriteRenderFollowTarget()
   {

   }

   protected virtual void MovePosition()
   {
      moveDir = (target.position - transform.position).normalized;
      rb.MovePosition(rb.position + moveDir * speed * Time.deltaTime);
      
   }
   protected virtual void RotateTowardsMoveDir()
   {
      Quaternion targetRotation = Quaternion.LookRotation(moveDir);

      transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
   }
   protected virtual bool IsTargetInAttackThreshold()
   {
      if( (transform.position - target.position).magnitude <= attackThreshold ) return true;
      return false;
   }

   public void ChangeTarget(Transform newtarget)
   {
      target = newtarget;
      aiPath.destination = target.position;
   }

   public  void SpawnRandom()
   {
      transform.position = new Vector3( UnityEngine.Random.Range(0,100), 0 , UnityEngine.Random.Range(0,100));
   }

   public Vector3 GetPos()
   {
      return transform.position;
   } 
   public Transform GetTransform()
   {
      return transform;
   }




   public virtual void ChangeStateToIdleState()
   {
      
      _currentState = idleState;
      _currentState.Enter(this);
   }
   public virtual void IdleStateEnter()
   {
      ScanGraph();
      animator.SetTrigger("Idle");

   }
   public virtual void IdleStateExit()
   {
   }

   public virtual void IdleStateUpdate()
   {
      if(target && CanGotoTarget())
      {
         ChangeStateToMoveState();
      }
   }
   public virtual void IdleStateFixedUpdate()
   {

   }


   public virtual void ChangeStateToAttackState()
   {
      ScanGraph();
      _currentState = attackState;
      _currentState.Enter(this);
   }
   public virtual void AttackStateEnter()
   {
      animator.SetTrigger("Attack");
      targetHPManage = target.gameObject.GetComponent<EnemyHPManager>();

   }

   public virtual void AttackStateExit()
   {
   }

   public virtual void AttackStateUpdate()
   {



      
      timer += Time.deltaTime;
      if(!target)
      {
         FindTarget();
         return;

      }
      if(timer >= attackSpeed)
      {
         targetHPManage.TakeDame(dmg);  
         timer = 0;
      }




   }
   public virtual void AttackStateFixedUpdate()
   {
      
   }


   public void RemoveFromTeam()
   {

      if (gameObject.tag == "Blue")
      {
         EntityManager.Instance.RemoveFromBlueTeamByIndex(Index);
      }
      else if (gameObject.tag == "Red")
      {
         EntityManager.Instance.RemoveFromRedTeamByIndex(Index);
         GameManager.Instance.GetReward(moneyManager.GetReward());
         GameManager.Instance.CheckUpdate();
      }
   }
   public virtual void ChangeStateToDeathState()
   {
      // Debug.Log("Tao đã chết");
      _currentState = deathState;
      _currentState.Enter(this);
   }

   public virtual void DeathStateEnter()
   {
      animator.SetTrigger("Death");
      RemoveFromTeam();
      // Debug.Log("Tao da chet"  + gameObject.name);


      DeathStateExit();
   }

   public virtual void DeathStateExit()
   {
      Destroy(gameObject);

   }

   public virtual void DeathStateUpdate()
   {

   }
   public virtual void DeathStateFixedUpdate()
   {
      
   }


   public virtual void ChangeStateToMoveState()
   {
      _currentState = moveState;
      _currentState.Enter(this);
   }
   public virtual void MoveStateEnter()
   {

      animator.SetTrigger("Walk");
      // aiPath.canMove = true;

   }

   public virtual void MoveStateExit()
   {
      aiPath.canMove = false;
   }

   public virtual void MoveStateUpdate()
   {
      if(target)
      {

         if(IsTargetInAttackThreshold())
         {
            MoveStateExit();
            ChangeStateToAttackState();
            return;
         }

         aiPath.destination = target.position;
         RotateTowardsMoveDir();
         MovePosition();

      }
      else{
         MoveStateExit();

         FindTarget();
         return;
      }

      
   }
   public virtual void MoveStateFixedUpdate()
   {

   }



   public virtual void ChangeStateToTakedameState()
   {
      _currentState = takedameState;
      _currentState.Enter(this);
   }
   public virtual void TakedameStateEnter()
   {
   }

   public virtual void TakedameStateExit()
   {
   }

   public virtual void TakedameStateUpdate()
   {
   }
   public virtual void TakedameStateFixedUpdate()
   {
      
   }


   public virtual void ChangeStateToFindTargetState()
   {
      _currentState = findTargetState;
      _currentState.Enter(this);
   }

   public virtual void FindStateEnter()
   {
      throw new NotImplementedException();
   }

   public virtual void FindStateExit()
   {
      throw new NotImplementedException();
   }

   public virtual void FindStateUpdate()
   {
      throw new NotImplementedException();
   }
   public virtual void FindStateFixedUpdate()
   {
      
   }


}

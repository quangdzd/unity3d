using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
public class Knight : AEnemy
{
    private StatsForEnemy _knightstat;
    private bool defend = false;

    private int max_Energy ;
    private int energy;


    protected override void Awake() {
        base.Awake();
        _knightstat = AllEnemyData.Instance.FindEnemyById("2").StatsForEnemies.Find(e => e._name == "Knight");
        SetupStat(_knightstat);
        max_Energy = 100;
        energy = 0;

    }

    protected override void Start()
    {
        base.Start();
        SetupStat(_knightstat);


    }

    public override void DeathStateEnter()
    {
        int  i = Random.Range(0, 2);
        switch (i)
        {
            case 0: 
            {
                animator.SetTrigger("Death1");
                break;
            }
            case 1: 
            {
                 animator.SetTrigger("Death2");
                 break;
            }
        }

        RemoveFromTeam();

        // StartCoroutine(WaitSecond(2f));
        Destroy(gameObject);

    }

    public void Defend()
    {
        animator.SetLayerWeight(1 , 0.8f);
        animator.SetFloat("Move", 0f);
        speed = _knightstat._speed*0.7f;

    }

    public void Run()
    {
        animator.SetLayerWeight(1 , 0.8f);
        animator.SetFloat("Move", 1);
        speed = _knightstat._speed*2f;
    }

    public override void MoveStateEnter()
    {
        base.MoveStateEnter();

        Defend();
        
    }
    public override void MoveStateUpdate()
    {
        base.MoveStateUpdate();

        if(target)
        {

            if(energy >= max_Energy)
            {
                Run();
                
                StartCoroutine(reduceEnergyPerTime(0.1f , 5));
            }
            if(energy <= 0)
            {
                Defend();
                StartCoroutine(autorechargeEnergyPerSeconds(50));
            }

        }

        
    }
    public override void MoveStateExit()
    {
        base.MoveStateExit();
        animator.SetLayerWeight(1 , 0);


    }
    public override void RechargeEnergy(int energy)
    {
        this.energy = Mathf.Min(this.energy + energy, max_Energy);
    }
    public override void ReduceEnergy(int energy)
    {
        this.energy = Mathf.Max(this.energy - energy, 0 );
    }

    IEnumerator autorechargeEnergyPerSeconds (int energy)
    {
        while (this.energy < max_Energy) // Chỉ chạy khi chưa đầy năng lượng
        {
            Debug.Log("hoi nang luong");
            RechargeEnergy(energy);
            yield return new WaitForSeconds(2f);
        }
    }
    IEnumerator reduceEnergyPerTime(float time , int energy)
    {

        ReduceEnergy(energy);
        yield return new WaitForSeconds(time);
    }

  



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : AEnemy
{

    private StatsForEnemy _mageStats;

    private GameObject fireball;


    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject _headWandPoint;
    [SerializeField] private GameObject _witchWand;


    private bool isAttack ;

    protected override void Awake()
    {
        base.Awake();

        _mageStats = AllEnemyData.Instance.FindEnemyById("2").StatsForEnemies.Find(e => e._name == "Mage");
        isAttack = false;
        SetupStat(_mageStats);

        fireball = null;
    }

    protected override void Start()
    {
        base.Start();


    }


    public void Shoot()
    {
        Shoot(this.fireball, target);

        
    }

    public void AttachFireBall()
    {
        this.fireball.transform.position = _headWandPoint.transform.position;
        this.fireball.transform.SetParent(_witchWand.transform, false);
    }
    public void Shoot(GameObject bullet, Transform target)
    {


        FireBall fireBallComponent = bullet.GetComponent<FireBall>();
        fireBallComponent.Reset();

        GameObject _explosionEffect = PoolManager.Instance.GetFromPool("explosion", new Vector3(), new Quaternion(), false);

        fireBallComponent.SetDmg(dmg);
        fireBallComponent.SetEffect(_explosionEffect);
        fireBallComponent.SetTarget(target);

        Vector3 fbPos = fireBallComponent.transform.position;
         Vector3 dir = (this.target.position - fbPos + new Vector3(0, 1, 0)).normalized;
        
        fireBallComponent.SetDir(dir);
        fireBallComponent.transform.SetParent(null);



        fireBallComponent.thisAction += () => fireBallComponent.MoveToTarget();
        fireBallComponent.DestroyBullet(10f);

        this.fireball = null;
        this.isAttack = false;

    }

    
    public override void AttackStateUpdate()
    {
        if(!target)
        {
            FindTarget();
            return;
        }
        else
        {
            if (this.isAttack ==  false && target != null)
            {
                if (this.fireball == null)
                {
                    this.fireball = PoolManager.Instance.GetFromPool("fireball", spawnPoint.transform.position, spawnPoint.transform.rotation);
                }

                
                this.fireball.transform.SetParent(spawnPoint.transform);
                StartCoroutine(GrowFireBall());

                this.isAttack = true;
            }
        }



    }
    public IEnumerator GrowFireBall()
    {
        float scale = 0.1f;
        this.fireball.SetActive(true);

        while (scale < 2f)
        {
            this.fireball.gameObject.transform.localScale = new Vector3(scale, scale, scale);
            scale += 0.2f;


            yield return new WaitForSeconds(0.2f);

        }
        animator.SetTrigger("Shoot");

        SoundManager.Instance.PlaySFXCache("fireball", gameObject.transform);
        
    }

}

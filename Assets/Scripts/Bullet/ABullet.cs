using System;
using UnityEngine;

public abstract class ABullet : MonoBehaviour
{
    protected float speedBullet = 10f;
    protected float dmg;
    protected GameObject impactEffect;

    public delegate void ThisAction();
    public event ThisAction thisAction; 
   public virtual void Update()
    {
        thisAction?.Invoke();
    }
    public void ClearAction()
    {
        thisAction = null;
    }
    public virtual void Damage(GameObject target)
    {
        EnemyHPManager enemyHPManager = target.GetComponent<EnemyHPManager>();
        enemyHPManager.TakeDame(dmg);
    }
    protected virtual void Move(Vector3 dir)
    {
        transform.position += dir * speedBullet * Time.deltaTime;
    }
    public virtual void SetDmg(float dmg)
    {
        this.dmg = dmg;
    }

    public virtual void MoveToTarget()
    {

    }


    public virtual void DestroyBullet(float timelife)
    {

    }


}

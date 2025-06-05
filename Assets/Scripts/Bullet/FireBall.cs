using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : ABullet
{
    [SerializeField]
    private GameObject explosion;

    [SerializeField]
    private Transform target;
    private Vector3 dir;

    [SerializeField]
    public bool pooled = false;
    private bool collided = false;


    public override void MoveToTarget()
    {
        if (this.target != null)
        {
            // Vector3 dir = (this.target.position - transform.position + new Vector3(0, 1, 0)).normalized;
            Move(dir);


            if (Vector3.Distance(this.target.position, transform.position) < 1 && !collided)
            {

                ExplosionEffect explosionEffect = explosion.GetComponent<ExplosionEffect>();
                explosionEffect.transform.position = transform.position;
                explosionEffect.Activate();
                SoundManager.Instance.PlaySFXCache("explore", this.target);

                Damage(this.target.gameObject);
                Reset();
                pooled = true;
                collided = true;

                PoolManager.Instance.AddToPool(gameObject, "fireball");

            }

        }
    }
    public void SetDir(Vector3 dir)
    {
        this.dir = dir;

    }
    
    public void SetEffect(GameObject effect)
    {
        explosion = effect;

    }
    public void SetTarget(Transform target)
    {
        this.target = target;   
    }


    // public void OnTriggerEnter(Collider collision)
    // {
    //     if (collision.CompareTag("Red") && !collided)
    //     {

    //         ExplosionEffect explosionEffect = explosion.GetComponent<ExplosionEffect>();
    //         explosionEffect.Activate();

    //         Damage(collision.gameObject);
    //         PoolManager.Instance.AddToPool(gameObject, "fireball");
    //         collided = true;
    //     }
    // }
    public void Reset()
    {
        collided = false;
        pooled = false;
        explosion = null;
        target = null;
        ClearAction();

    }
    public override void DestroyBullet(float timelife)
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(PoolObj(timelife));
            
        }
    }

    public IEnumerator PoolObj(float timelife)
    {
        
        float count = 0f;
        while (count < timelife)
        {
            count += Time.deltaTime;
            if (  (target == null || !target.gameObject.activeSelf )&& !collided)
            {
                ExplosionEffect explosionEffect = explosion.GetComponent<ExplosionEffect>();
                explosionEffect.transform.position = transform.position;
                explosionEffect.Activate();
                // SoundManager.Instance.PlaySFXCache("explore", this.target);


                Reset();
                pooled = true;
                PoolManager.Instance.AddToPool(gameObject, "fireball");
                yield break;
            }

            yield return null;
        }

        if (gameObject.activeSelf)
        {
            pooled = true;
            Reset();
            PoolManager.Instance.AddToPool(gameObject, "fireball");
        }
    }





}

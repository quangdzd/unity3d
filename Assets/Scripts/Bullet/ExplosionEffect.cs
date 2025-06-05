using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{

    public void Activate()
    {
        
        ParticleSystem ps = gameObject.GetComponent<ParticleSystem>();
        
        StartCoroutine(Explosion(ps.main.duration));
    }


    IEnumerator Explosion(float lifeTime)
    {
        ParticleSystem ps = gameObject.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();

            yield return new WaitForSeconds(lifeTime);

            ps.Stop(true , ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        PoolManager.Instance.AddToPool(gameObject , "explosion");

    }
}

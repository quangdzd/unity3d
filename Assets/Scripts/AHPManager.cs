
using UnityEngine;

public abstract class AHPManager : MonoBehaviour
{
    protected float originHp;
    protected float currentHp;

    protected virtual void Awake()
    {
        HealFullHp();
    }

    public void HealFullHp()
    {
        currentHp = originHp;
    }

    public virtual void TakeDame(float dmg)
    {
        
    }
}

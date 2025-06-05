
public class Skeleton : AEnemy
{
    private StatsForEnemy statSkeleton;

    protected override void Awake()
    {

        base.Awake();
        
        statSkeleton = AllEnemyData.Instance.FindEnemyById("1").StatsForEnemies.Find(e => e._name == "Skeleton");
        SetupStat(statSkeleton);




    }
    protected override void Start()
    {


        base.Start();
    }





}

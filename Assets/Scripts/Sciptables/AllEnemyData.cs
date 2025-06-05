using System.Collections.Generic;


public class AllEnemyData : SingletonDestroy<AllEnemyData>
{
    public List<EnemyData> enemyDatas ;

    // Hàm trả về danh sách EnemyData
    public List<EnemyData> GetAllEnemies()
    {
        return enemyDatas;
    }

    // Tìm EnemyData theo ID
    public EnemyData FindEnemyById(string id)
    {
        return enemyDatas?.Find(e => e.id == id);
    }
}

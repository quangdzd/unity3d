using System.Collections.Generic;
using UnityEngine;

public class EnemyLocationManager 
{

    public void LoadLocation(EnemyData enemyData , Vector3 start , Vector2Int cellsize ,int[,] enemyids)
    {
        List<StatsForEnemy> statsForEnemys = enemyData.StatsForEnemies;
        for (int i = 0; i < enemyids.GetLength(0) ; i++)
        {
            for(int j = 0; j < enemyids.GetLength(1) ; j++)
            {
                int x = enemyids[i, j];
                if(x != 0 )
                {
                    GameObject gameObject =  Object.Instantiate(statsForEnemys[x - 1].prefab);
                    gameObject.transform.position = start + new Vector3(j*cellsize.x , 0 , i*cellsize.y);
                    
                }
            }
        }
    }

}

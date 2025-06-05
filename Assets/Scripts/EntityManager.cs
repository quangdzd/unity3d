using System;

using UnityEngine;

public class EntityManager : SingletonDestroy<EntityManager>
{
    private float _boudary;
    Vector3 positiveZDirection;
    Vector3 negativeZDirection;


    
    public Material _redMaterial;
    public Material _blueMaterial;

    private KdTree<Transform> _redTeam;
    private KdTree<Transform> _blueTeam;


    protected override void Awake() {
        base.Awake();
        _boudary = 50;
        positiveZDirection = Vector3.forward;
        negativeZDirection = Vector3.back;

        _redTeam = new KdTree<Transform>();
        _blueTeam = new KdTree<Transform>();
    }


    public void AddToRedTeam(GameObject gameObject , Vector3 pos)
    {
        gameObject.transform.position = pos;
        // ChangeToRedColor(gameObject);
        ChangeTag(gameObject ,"Red");
        RotateTowardsPositiveZ(gameObject);
        _redTeam.Add(gameObject.transform);
        // Debug.Log("quai duoc them vao team do");
    }

    public int GetRedTeamMenbersCount()
    {
        return _redTeam.Count;
    }
    
    public void AddToBlueTeam(GameObject gameObject, Vector3 pos)
    {
        gameObject.transform.position = pos;
        SpendGold(gameObject);
        // ChangeToBlueColor(gameObject);
        ChangeTag(gameObject, "Blue");
        RotateTowardsNegativeZ(gameObject);
        _blueTeam.Add(gameObject.transform);
        // Debug.Log("quai duoc them vao team xanh");


    }
    public int GetBlueTeamMenbersCount()
    {
        return _blueTeam.Count;
    }
    public void SpendGold(GameObject gameObject)
    {
        MoneyManager moneyManager = gameObject.GetComponent<MoneyManager>();
        GameManager.Instance.SpendMoney(moneyManager.GetCost());
        // Debug.Log("tien cua mage thuc te la " + moneyManager.GetCost());
    }
    
    public void Clear()
    {
        foreach (Transform transform in _redTeam)
        {
            Destroy(transform.gameObject);
        }
        foreach (Transform transform in _blueTeam)
        {
            Destroy(transform.gameObject);
        }
        _redTeam.Clear();
        _blueTeam.Clear();
    }
    public void RemoveFromRedTeamByIndex(int index)
    {
        _redTeam.RemoveAt(index);
    }
    public void RemoveFromBlueTeamByIndex(int index)
    {
        _blueTeam.RemoveAt(index);
    }

    public Transform FindTargetClosest(GameObject gameObject)
    {
        Transform nearest ; 
        if(gameObject.tag == "Blue")
        {
            try
            {
                 nearest = _redTeam.FindClosest(gameObject.transform.position);
                return nearest;
                
            }
            catch (System.Exception)
            {
                
                Debug.LogError("Dell tìm được mục tiêu cho thằng blue");
            }
            
        }
        else
        {
            try
            {
            nearest = _blueTeam.FindClosest(gameObject.transform.position);
            return nearest;
            }
            catch (System.Exception)
            {
                Debug.LogError("Dell tìm được mục tiêu cho thằng red");

            }
            
        }
        return null;
    }

    public void UpdatePositions()
    {
        _redTeam.UpdatePositions();
        _blueTeam.UpdatePositions();
    }



    public void ChangeTag(GameObject gameObject , String tag)
    {
        gameObject.tag = tag;
    }

    public void ChangeToRedColor( GameObject gameObject)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] mats = renderer.materials;
            for (int i = 0; i < mats.Length ; i++)
            {
                mats[i] = _redMaterial;
            }
            renderer.materials = mats;
        }
    }
    public void ChangeToBlueColor(GameObject gameObject)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] mats = renderer.materials;
            for (int i = 0; i < mats.Length ; i++)
            {
                mats[i] = _blueMaterial;
            }
            renderer.materials = mats;
        }
    }    

    public void RotateTowardsPositiveZ(GameObject gameObject)
    {
        gameObject.transform.rotation = Quaternion.LookRotation(negativeZDirection);
    }
    public void RotateTowardsNegativeZ(GameObject gameObject)
    {
        gameObject.transform.rotation = Quaternion.LookRotation(positiveZDirection);

    }

}

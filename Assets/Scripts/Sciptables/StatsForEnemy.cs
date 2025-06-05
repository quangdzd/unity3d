using UnityEngine;

[System.Serializable]
public class StatsForEnemy
{
    [field: SerializeField]
    public string _name{get; private set;}
    
    [field: SerializeField]
    public int _hp{get; private set;}

    [field: SerializeField]
    public int _dmg{get; private set;}


    [field: SerializeField]
    public float _attackThreshold{get; private set;}

    [field: SerializeField]

    public float _attackSpeed{get; private set;}

    [field:SerializeField]
    public float _speed{get; private set;}


    [field:SerializeField]
    public Sprite _image {get; private set;}

    [field:SerializeField]
    public int _coin{ get; private set;}


    [field: SerializeField]
    public GameObject prefab{get; private set;}


}


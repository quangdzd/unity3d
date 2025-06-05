using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveDatas
{
    public int seed_money;
    public List<WaveEntry> waves;
}

[System.Serializable]
public class WaveEntry
{
    public string key;
    public int[] values;
}
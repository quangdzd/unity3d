using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SoundLibrary", menuName = "ScriptableObject/SoundLibrary")]
public class SoundLibrary : ScriptableObject 
{
    public List<Sound> soundUnits;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound Effect",fileName = "New Audio clip")] 
public class SOClip : ScriptableObject
{
    public string clipName;
    public  AudioClip  clip;
    public float volume = 1.0f;
    
   
    
    
}


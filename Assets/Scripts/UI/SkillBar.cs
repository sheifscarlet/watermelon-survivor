using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBar : MonoBehaviour
{
    [SerializeField] private List<Image> skillIcons;
    
    //Components
    [SerializeField] private GameObject player;
    private Giganto gigantoScript;
    private SeedSurge seedSurgeScript;
    private TwirlingSlices twirlingSlicesScript;
    private MelonRush melonRushScript;
    private SeedBombBarrage seedBombBarrageScript;

    private void Awake()
    {
        gigantoScript = player.GetComponent<Giganto>();
        seedSurgeScript = player.GetComponent<SeedSurge>();
        twirlingSlicesScript = player.GetComponent<TwirlingSlices>();
        melonRushScript = player.GetComponent<MelonRush>();
        seedBombBarrageScript = player.GetComponent<SeedBombBarrage>();
    }

    // Update is called once per frame
    void Update()
    {
        if (melonRushScript.isReady)
        {
            skillIcons[0].color = Color.green;
        }
        else
        {
            skillIcons[0].color = Color.red;
        }
        
        if (seedBombBarrageScript.isReady)
        {
            
            skillIcons[1].color = Color.green;
        }
        else
        {
            skillIcons[1].color = Color.red;
        }
        
        if (twirlingSlicesScript.isReady)
        {
            
            skillIcons[2].color = Color.green;
        }
        else
        {
            skillIcons[2].color = Color.red;
        }
        
        if (gigantoScript.isReady)
        {
            
            skillIcons[3].color = Color.green;
        }
        else
        {
            skillIcons[3].color = Color.red;
        }
        
        if (seedSurgeScript.isReady)
        {
            
            skillIcons[4].color = Color.green;
        }
        else
        {
            skillIcons[4].color = Color.red;
        }
    }
}

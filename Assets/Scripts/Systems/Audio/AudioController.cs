using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;
    [SerializeField] private List<SOClip> soundClips;
    public Dictionary<string, SOClip> _soundEffectMap;

    [SerializeField] private GameObject ost;
    
    [SerializeField] private bool isSFXTurnedOn;
    [SerializeField] private bool isMusicTurnedOn;

    private void Awake()
    {
        instance = this;
        _soundEffectMap = new Dictionary<string, SOClip>();
        foreach (var effect in soundClips)
        {
            _soundEffectMap[effect.clipName] = effect;
        }
    }

    private void Update()
    {
        if (isMusicTurnedOn)
        {
            ost.SetActive(true);
        }
        else
        {
            ost.SetActive(false);
        }
    }

    public void PlaySound(string name)
    {
        if (isSFXTurnedOn)
        {
            if (_soundEffectMap.TryGetValue(name, out SOClip effect))
            {
                AudioSource source = gameObject.AddComponent<AudioSource>();
                source.clip = effect.clip;
                source.volume = effect.volume;
                source.Play();
                Destroy(source, effect.clip.length);
            }
        }
        
    }

    public void  SwitchSFX()
    {
        isSFXTurnedOn = !isSFXTurnedOn;
    }

    public void SwitchMusic()
    {
        isMusicTurnedOn = !isMusicTurnedOn;
    }
}

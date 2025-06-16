using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAudioService
{
    public void PlaySounds(string soundName);   
    public void StopSounds(string SoundName);

    public void SetVolumn(float Volume);
}
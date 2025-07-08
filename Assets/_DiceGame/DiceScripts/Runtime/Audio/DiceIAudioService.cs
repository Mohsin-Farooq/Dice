using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGame
{
    public interface DiceIAudioService
    {
        public void PlaySounds(string soundName);
        public void StopSounds(string SoundName);

        public void SetVolumn(float Volume);
    }
}
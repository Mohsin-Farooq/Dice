using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEngine;

namespace DiceGame
{
    public class DiceAudioService : MonoBehaviour, DiceIAudioService
    {
        [SerializeField] private AudioSource SoundSource;
        [SerializeField] private List<AudioClip> audioClips;
        private Dictionary<string, AudioClip> _AudioClipsDic;
        private void Awake()
        {
            _AudioClipsDic = new Dictionary<string, AudioClip>();

            foreach (var clips in audioClips)
            {
                _AudioClipsDic[clips.name] = clips;
            }
            new DiceAudioManager(this);
            MMVibrationManager.iOSInitializeHaptics();
        }

        public void PlaySounds(string soundName)
        {

            if (_AudioClipsDic.TryGetValue(soundName, out var clips))
            {
                //  SoundSource.PlayOneShot(clips);     
                SoundSource.clip = clips;
                SoundSource.Play();
            }
        }
        public void StopSounds(string SoundName)
        {
            SoundSource.Pause();
        }

        public void SetVolumn(float Volume)
        {
            SoundSource.volume = Volume;
        }
    }
}
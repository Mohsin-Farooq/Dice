using UnityEngine;

namespace DiceGame
{
    public class DiceAudioManager : MonoBehaviour
    {
        public static DiceAudioManager _instance { get; private set; }
        private readonly DiceIAudioService _audioServiceProvider;
        public DiceAudioManager(DiceIAudioService audioService)
        {
            _audioServiceProvider = audioService;
            _instance = this;
        }
        public void PlaySound(string SoundName)
        {
            //if (SoundName == "Dice")
            //{
            //    if (Random.Range(0, 15) < 2)
            //    {
            //        _audioServiceProvider.PlaySounds(SoundName);
            //        _audioServiceProvider.SetVolumn(0.5f);
            //    }
            //}
            //else
            //  {
            _audioServiceProvider.SetVolumn(1f);
            _audioServiceProvider.PlaySounds(SoundName);
            //  }
        }

        public void StopSound(string SoundName)
        {
            _audioServiceProvider.StopSounds(SoundName);
        }
    }
}

using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance { get; private set; }
    private readonly IAudioService _audioServiceProvider;
    public AudioManager (IAudioService audioService)
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

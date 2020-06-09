using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace ComplexGame
{
    public class AudioManager : MonoBehaviour
    {
        public List<AudioClip> musics;
        public AudioSource audioListener;
        public bool isActive = true;
    
        public static AudioManager Instance { get; private set; }
  
        private void Awake()
        {
     
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            Instance = this;
            RandomMusic();
        }
   
        void Update () {
            if (!audioListener.isPlaying && isActive)
            {
                RandomMusic();
            }
        }

        public void StopMusic()
        {
            isActive = false;
            audioListener.Pause();
        }

        public void PlayMusic()
        {
            isActive = true;
            audioListener.Play();
            
        }
        public void SetVolume(float volume)
        {
            audioListener.volume = volume;
        }
        private void RandomMusic()
        {
            audioListener.clip = musics[Random.Range(0, musics.Count)];
            audioListener.Play();
        }
    }
}

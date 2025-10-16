using UnityEngine;
using UnityEngine.Audio;
using DataUtils;

namespace Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioMixerGroup mixer;
        
        [Header("Sounds")] [Space(10)]
        [SerializeField] private AudioSource buttonClick;
        [SerializeField] private AudioSource swipeSound;
        [SerializeField] private AudioSource coinSound;
        
        
        private const string _soundKey = "_Sound_Key";
        private const string _musicKey = "_Music_Key";

        protected override void Awake()
        {
            base.Awake();

            if (!PlayerPrefs.HasKey(_soundKey))
                GameSaves.Instance.WriteData(_soundKey, true);

            if (!PlayerPrefs.HasKey(_musicKey))
                GameSaves.Instance.WriteData(_musicKey, true);
        }

        private void Start()
        {
            SetMusic(GetActivityMusic());
            SetSound(GetActivitySound());
        }

        public void ButtonClickSound() => buttonClick.Play();
        public void SwipeSound() => swipeSound.Play();
        public void CoinSound() => coinSound.Play();

        public void SetSound(bool enabled)
        {
            if (enabled)
                mixer.audioMixer.SetFloat("SoundVolume", 0);
            else
                mixer.audioMixer.SetFloat("SoundVolume", -80);

            GameSaves.Instance.WriteData(_soundKey, enabled);
        }

        public void SetMusic(bool enabled)
        {
            if (enabled)
                mixer.audioMixer.SetFloat("MusicVolume", 0);
            else
                mixer.audioMixer.SetFloat("MusicVolume", -80);

            GameSaves.Instance.WriteData(_musicKey, enabled);
        }

        public bool GetActivitySound() => GameSaves.Instance.ReadData<bool>(_soundKey);

        public bool GetActivityMusic() => GameSaves.Instance.ReadData<bool>(_musicKey);

    }
}

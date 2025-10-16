using UnityEngine;
using Managers;
using UnityEngine.UI;

namespace Screens
{
    public class PauseScreen : BaseScreen
    {
        [SerializeField] private Toggle musicToggle;
        [SerializeField] private Toggle soundToggle;

        private void Start()
        {
            SetSoundsToggles();

            musicToggle.onValueChanged.AddListener(OnMusicToggleChange);
            soundToggle.onValueChanged.AddListener(OnSoundToggleChange);
        }
        
        public override void OpenScreen()
        {
            SetSoundsToggles();
            base.OpenScreen();
            Time.timeScale = 0;
        }

        public override void CloseScreen()
        {
            base.CloseScreen();
            Time.timeScale = 1;
        }

        public void OnContinueGameButtonClicked()
        {
            AudioManager.Instance.ButtonClickSound();
            CloseScreen();
        }
        
        private void OnMusicToggleChange(bool isON)
        {
            AudioManager.Instance.ButtonClickSound();
            AudioManager.Instance.SetMusic(isON);
        }

        private void OnSoundToggleChange(bool isON)
        {
            AudioManager.Instance.ButtonClickSound();
            AudioManager.Instance.SetSound(isON);
        }

        private void SetSoundsToggles()
        {
            musicToggle.isOn = AudioManager.Instance.GetActivityMusic();
            soundToggle.isOn = AudioManager.Instance.GetActivitySound();
        }
    }
}

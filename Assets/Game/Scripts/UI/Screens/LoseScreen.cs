using Managers;
using TMPro;
using UnityEngine;

namespace Screens
{
    public class LoseScreen : BaseScreen
    {
        [SerializeField] private TMP_Text coinsText;

        public void Setup(int coinForLevel)
        {
            if(UIManager.Instance.GetScreen<WinScreen>().Canvas.gameObject.activeSelf)
                return;
            
            SetCoinText(coinForLevel);
            OpenScreen();
        }

        public void OnOpenStartScreen()
        {
            AudioManager.Instance.ButtonClickSound();
            UIManager.Instance.CloseScreen<GameScreen>();
            UIManager.Instance.OpenScreen<StartScreen>();
            CloseScreen();
        }

        public void OnRestartButtonClicked()
        {
            AudioManager.Instance.ButtonClickSound();
            UIManager.Instance.CloseScreen<GameScreen>();
            UIManager.Instance.OpenScreen<GameScreen>();
            CloseScreen();
        }

        private void SetCoinText(int coinForLevel)
        {
            coinsText.text = "Coins: " + coinForLevel;
        }
    }
}

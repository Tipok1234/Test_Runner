using UnityEngine;
using TMPro;
using Managers;

namespace Screens
{
    public class WinScreen : BaseScreen
    {
        [SerializeField] private TMP_Text coinsText;

        public void Setup(int coinForLevel)
        {
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

        private void SetCoinText(int coinForLevel)
        {
            coinsText.text = "Coins: " + coinForLevel;
        }
    }
}

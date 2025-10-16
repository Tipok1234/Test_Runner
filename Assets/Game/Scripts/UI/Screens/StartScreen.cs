using Managers;
using UnityEngine;

namespace Screens
{
    public class StartScreen : BaseScreen
    {
        public void OnOpenGameScreen()
        {
            AudioManager.Instance.ButtonClickSound();
            UIManager.Instance.OpenScreen<GameScreen>();
            CloseScreen();
        }

        public void OnExitGame()
        {
            AudioManager.Instance.ButtonClickSound();
            Application.Quit();
        }
    }
}

using System;
using Managers;
using UnityEngine;
using Views;

namespace Screens
{
    public class GameScreen : BaseScreen
    {
        public TimerView Timer => timerView;
        public static event Action StartGameAction;

        [SerializeField] private TimerView timerView;
        
        public override void OpenScreen()
        {
            base.OpenScreen();
            Setup();
        }

        public void OnOpenPauseScreen()
        {
            AudioManager.Instance.ButtonClickSound();
            
            UIManager.Instance.OpenScreen<PauseScreen>();
        }

        private void Setup()
        {
            timerView.StartTimer();
            StartGameAction?.Invoke();
        }
    }
}

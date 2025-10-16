using UnityEngine;
using System;
using TMPro;

namespace Views
{
    public class TimerView : MonoBehaviour
    {
        public static event Action EndTimeAction;
       
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private float gameTime;
        
        private float _remainingTime;
        
        private bool isEndTime;
        private bool isTimerRunning;

        private void OnDisable()
        {
            ResetTimer();
        }

        private void Update()
        {
            if (isTimerRunning && _remainingTime > 0)
            {
                _remainingTime -= Time.deltaTime;
                UpdateTime();

                if (_remainingTime <= 0)
                {
                    _remainingTime = 0;
                    isTimerRunning = false;
                    isEndTime = true;
                    EndTimeAction?.Invoke();
                }
            }
        }

        public void StartTimer()
        {
            SetTime();
            isTimerRunning = true;
        }
        
        public void StopTimer()
        {
            isTimerRunning = false;
        }

        private void SetTime()
        {
            _remainingTime = gameTime;
            isTimerRunning = true;
            UpdateTime();
        }

        private void ResetTimer()
        {
            StopTimer();
            isEndTime = false;
            _remainingTime = gameTime;
        }
        private void UpdateTime()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(_remainingTime);
            string timeString = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            SetTimerText(timeString);
        }

        private void SetTimerText(string time)
        {
            if (timeText)
                timeText.text = time;
        }
    }
}

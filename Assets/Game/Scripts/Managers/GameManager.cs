using Controllers;
using Models;
using Screens;
using UnityEngine;
using Views;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PlayerModel playerModel;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private ObstacleSpawner obstacleSpawner;
        [SerializeField] private CoinSpawner coinSpawner;

        public PlayerModel Player { get; private set; }

        private void Start()
        {
            Subscribe();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            GameScreen.StartGameAction += OnStartGame;
            TimerView.EndTimeAction += OnEndTime;
        }

        private void Unsubscribe()
        {
            GameScreen.StartGameAction -= OnStartGame;
            TimerView.EndTimeAction -= OnEndTime;
        }

        private void OnEndTime()
        {
            UIManager.Instance.GetScreen<WinScreen>().Setup(Player.CollectedCoin);
            Player.WinState();
        }

        private void OnStartGame()
        {
            if (Player)
            {
                Player.ResetState();
                obstacleSpawner.ResetSpawner();
                coinSpawner.ResetSpawner();
            }
            else
            {
                Setup();
            }
        }

        private void Setup()
        {
            SpawnPlayer();
            SetupCamera();
            SetupObstacles();
            SetupCoinSpawner();
        }

        private void SetupCoinSpawner() => coinSpawner.SetupPlayerTransform(Player.transform);
        private void SetupObstacles() => obstacleSpawner.SetPlayerTransform(Player.transform);
        private void SetupCamera() => cameraController.SetPlayerFollow(Player.transform);
        private void SpawnPlayer() => Player = Instantiate(playerModel,gameObject.transform);
    }
}

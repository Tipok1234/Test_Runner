using System;
using System.Collections;
using System.Collections.Generic;
using Controllers;
using Models;
using UnityEngine;

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
            Setup();
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
        private void SpawnPlayer()
        {
            Player = Instantiate(playerModel,gameObject.transform);
            //Player.StartRunning();
        }
    }
}

using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Managers
{
    public class CoinSpawner : MonoBehaviour
    {
        [SerializeField] private CoinModel coinPrefab;
        
        [SerializeField] private LayerMask obstacleMask;
        
        [SerializeField] private int poolSize = 30;
        
        [SerializeField] private float laneWidth = 3f;
        [SerializeField] private float spawnDistance = 30f; 
        [SerializeField] private float despawnDistance = 10f; 
        [SerializeField] private float checkRadius = 1f;
    
        private Transform _player; 
        
        private List<CoinModel> _coins = new List<CoinModel>();
        
        private float _nextSpawnZ = 0f;

        private void Start()
        {
            SpawnPoolCoins();
        }
        
        private void Update()
        {
            if (!_player)
                return;

            if (_player.position.z + spawnDistance > _nextSpawnZ)
            {
                SpawnCoins(new Vector3(0, 0, _nextSpawnZ));
                _nextSpawnZ += 10f; 
            }

            DisableCoins();
        }

        public void SetupPlayerTransform(Transform playerTransform)
        {
            _player = playerTransform;
            _nextSpawnZ = _player.position.z + 10f;
        }

        private void DisableCoins()
        {
            foreach (var coin in _coins)
            {
                if (coin.gameObject.activeInHierarchy && _player.position.z - coin.transform.position.z > despawnDistance)
                {
                    coin.gameObject.SetActive(false);
                }
            }
        }

        private void SpawnCoins(Vector3 startPos)
        {
            int count = Random.Range(2, 6);
            float[] lanes = { -1.5f, 0f, 1.5f };
            float laneX = lanes[Random.Range(0, lanes.Length)];

            for (int i = 0; i < count; i++)
            {
                var pos = new Vector3(laneX, 1f, startPos.z + i * 2f);

                if (Physics.CheckSphere(pos, checkRadius, obstacleMask))
                    continue; 

                var coin = GetFreeCoin();
                if (coin == null)
                    return;

                coin.transform.position = pos;
                coin.gameObject.SetActive(true);
            }
        }
        
        private void SpawnPoolCoins()
        {
            for (int i = 0; i < poolSize; i++)
            {
                var coin = Instantiate(coinPrefab, transform);
                coin.gameObject.SetActive(false);
                _coins.Add(coin);
            }
        }

        private CoinModel GetFreeCoin()
        {
            foreach (var coin in _coins)
            {
                if (!coin.gameObject.activeInHierarchy)
                    return coin;
            }

            return null;
        }
    }
}

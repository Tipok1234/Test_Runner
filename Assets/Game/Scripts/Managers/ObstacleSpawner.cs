using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;

namespace Managers
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private ObstacleModel obstaclePrefab;
        [SerializeField] private int poolSize;

        [SerializeField] private float lineStep;
        [SerializeField] private float yPosition;
        [SerializeField] private float spawnDistance;
        [SerializeField] private float despawnDistance;
        
        private readonly List<ObstacleModel> _pool = new List<ObstacleModel>();
        
        private Transform _playerTransform;
        
        private int _nextSpawnIndex = 0;
        
        private float _lastSpawnZ = 0f;

        private void Start()
        {
            InitializePool();
            
            for (int i = 0; i < poolSize / 2; i++)
                SpawnNext();
        }

        private void Update()
        {
            if (_playerTransform != null && _playerTransform.position.z + spawnDistance > _lastSpawnZ)
            {
                SpawnNext();
            }

            foreach (var obstacle in _pool)
            {
                if (obstacle.gameObject.activeSelf &&
                    _playerTransform.position.z - obstacle.transform.position.z > despawnDistance)
                {
                    obstacle.gameObject.SetActive(false);
                }
            }
        }

        public void SetPlayerTransform(Transform playerTransform) => _playerTransform = playerTransform;
        
        private void InitializePool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                var obj = Instantiate(obstaclePrefab, transform);
                obj.gameObject.SetActive(false);
                _pool.Add(obj);
            }
        }

        private void SpawnNext()
        {
            var obstacle = _pool[_nextSpawnIndex];
            _nextSpawnIndex = (_nextSpawnIndex + 1) % _pool.Count;

            float newZ = _lastSpawnZ + spawnDistance;
            Vector3 pos = new Vector3(RandomLaneX(), yPosition, newZ);
            obstacle.transform.position = pos;
            obstacle.gameObject.SetActive(true);

            _lastSpawnZ = newZ;
        }

        private float RandomLaneX()
        {
            int lane = Random.Range(0, 3); 
            return (lane - 1) * lineStep;      
        }
    }
}

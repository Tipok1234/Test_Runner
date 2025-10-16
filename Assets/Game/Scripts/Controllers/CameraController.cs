using UnityEngine;

namespace Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Vector3 offset;
        
        [SerializeField] private float smoothSpeed;

        private Transform _playerTransform;

        private void LateUpdate() 
        {
            if (_playerTransform == null)
                return;

            Vector3 targetPos = _playerTransform.position + offset;

            targetPos.x = 0;
            targetPos.y = offset.y;

            transform.position = targetPos;
        }
        public void SetPlayerFollow(Transform player) => _playerTransform = player;
    }
}

using DataUtils;
using DG.Tweening;
using UnityEngine;

namespace Models
{
    public class CoinModel : MonoBehaviour
    {
        [SerializeField] private Vector3 scaleSize;
        [SerializeField] private Vector3 baseScale;
        [SerializeField] private float collectedAnimationTime;
        
        private Tween _tween;

        private void OnEnable()
        {
            ResetScale();
        }

        private void OnDisable()
        {
            _tween?.Kill();
        }

        public void CollectedAnimation()
        {
            _tween = transform.DOScale(scaleSize, collectedAnimationTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                _tween = transform.DOScale(Vector3.zero, collectedAnimationTime).SetEase(Ease.Linear).OnComplete(()
                    => gameObject.SetActive(false));
            });
        }

        public void AddCoin() => GameSaves.Instance.AddCoin(1);

        private void ResetScale()
        {
            transform.localScale = baseScale;
        }
    }
}

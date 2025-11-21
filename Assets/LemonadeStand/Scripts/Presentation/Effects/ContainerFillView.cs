//using System;
//using UnityEngine;
//using UnityEngine.UI;
//using DG.Tweening;

//namespace LemonadeStand.Presentation.UI
//{
//    public class ContainerFillView : MonoBehaviour
//    {
//        [Header("Root")]
//        [SerializeField] private GameObject root;

//        [Header("Fill Image")]
//        [SerializeField] private Image fillImage;

//        [Header("Animation")]
//        [SerializeField] private float fillDuration = 0.75f;
//        [SerializeField] private Ease fillEase = Ease.OutQuad;

//        [Header("Optional SFX")]
//        [SerializeField] private Audio.UnityAudioService audioService;
//        [SerializeField] private string waterFillSfxId = "water_fill";

//        private Tween _currentTween;

//        private void Awake()
//        {
//            if (root == null)
//                root = gameObject;

//            ResetFillInstant();
//        }

       
//        public void ResetFillInstant()
//        {
//            if (fillImage != null)
//                fillImage.fillAmount = 0f;
//        }

       
//        public void PlayFillAnimation(Action onCompleted = null)
//        {
//            if (fillImage == null)
//            {
//                onCompleted?.Invoke();
//                return;
//            }

//            root.SetActive(true);

//            _currentTween?.Kill();
//            fillImage.fillAmount = 0f;

//            // Play water SFX if available
//            audioService?.PlaySfx(waterFillSfxId);

//            _currentTween = fillImage
//                .DOFillAmount(1f, fillDuration)
//                .SetEase(fillEase)
//                .OnComplete(() =>
//                {
//                    onCompleted?.Invoke();
//                });
//        }

//        private void OnDisable()
//        {
//            _currentTween?.Kill();
//        }
//    }
//}

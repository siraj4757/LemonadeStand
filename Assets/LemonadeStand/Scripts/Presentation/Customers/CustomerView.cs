using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace LemonadeStand.Presentation.UI
{
    /// <summary>
    /// Handles visual presentation + simple animations for the customer:
    /// - Enters from the left
    /// </summary>
    public class CustomerView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform root;  
        [SerializeField] private Image characterImage; 

        [Header("Tween Settings")]
        [SerializeField] private float enterDuration = 0.35f;
        [SerializeField] private float exitDuration = 0.35f;
        [SerializeField] private float horizontalOffset = 800f;
        [SerializeField] private Ease enterEase = Ease.OutBack;
        [SerializeField] private Ease exitEase = Ease.InBack;

        private Sprite _idleSprite;
        private Sprite _talkSprite;
        private Sprite _happySprite;
        private Sprite _sadSprite;

        private Vector2 _centerAnchoredPos;
        private bool _initialized;


        private void Awake()
        {
            if (root == null)
                root = (RectTransform)transform;

            if (characterImage == null)
                characterImage = GetComponentInChildren<Image>();

            _centerAnchoredPos = root.anchoredPosition;
            _initialized = true;

            root.gameObject.SetActive(false);
        }


        public void SetCustomerSprites(Sprite idle, Sprite talking, Sprite happy, Sprite sad)
        {
            _idleSprite = idle;
            _talkSprite = talking != null ? talking : idle;
            _happySprite = happy != null ? happy : idle;
            _sadSprite = sad != null ? sad : idle;
        }


        public void Show()
        {
            if (!_initialized)
                Awake();

            root.gameObject.SetActive(true);
            root.DOKill();

            // start off-screen to the left
            root.anchoredPosition = _centerAnchoredPos + Vector2.left * horizontalOffset;
            characterImage.sprite = _idleSprite;

            root.DOAnchorPos(_centerAnchoredPos, enterDuration)
                .SetEase(enterEase);
        }


        public void PlayTalk()
        {
            if (_talkSprite != null)
                characterImage.sprite = _talkSprite;
        }


        public void PlayHappy()
        {
            if (_happySprite != null)
                characterImage.sprite = _happySprite;
        }


        public void PlaySad()
        {
            if (_sadSprite != null)
                characterImage.sprite = _sadSprite;
        }


        public void PlayExit(Action onComplete = null)
        {
            root.DOKill();

            root.DOAnchorPos(_centerAnchoredPos + Vector2.left * horizontalOffset, exitDuration)
                .SetEase(exitEase)
                .OnComplete(() =>
                {
                    root.gameObject.SetActive(false);
                    onComplete?.Invoke();
                });
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace LemonadeStand.Presentation.UI
{
    /// <summary>
    /// Visual feedback for correct / wrong answers.
    /// Shows simple icons
    /// </summary>
    public class FeedbackVfxView : MonoBehaviour
    {
        [Header("Root")]
        [SerializeField] private CanvasGroup rootCanvasGroup;

        [Header("Icons")]
        [SerializeField] private Image correctIcon;
        [SerializeField] private Image wrongIcon;

        [Header("Animation")]
        [SerializeField] private float popScale = 1.2f;
        [SerializeField] private float popDuration = 0.2f;
        [SerializeField] private float holdDuration = 0.4f;
        [SerializeField] private float fadeOutDuration = 0.25f;

        private Sequence _currentSequence;
        private Vector3 _originalCorrectScale;
        private Vector3 _originalWrongScale;


        private void Awake()
        {
            if (rootCanvasGroup == null)
                rootCanvasGroup = GetComponent<CanvasGroup>();

            if (rootCanvasGroup != null)
            {
                rootCanvasGroup.alpha = 0f;
                rootCanvasGroup.interactable = false;
                rootCanvasGroup.blocksRaycasts = false;
            }

            if (correctIcon != null)
            {
                _originalCorrectScale = correctIcon.rectTransform.localScale;
                correctIcon.gameObject.SetActive(false);
            }

            if (wrongIcon != null)
            {
                _originalWrongScale = wrongIcon.rectTransform.localScale;
                wrongIcon.gameObject.SetActive(false);
            }
        }


        public void PlayCorrect()
        {
            PlayFeedback(true);
        }


        public void PlayWrong()
        {
            PlayFeedback(false);
        }


        private void PlayFeedback(bool isCorrect)
        {
            if (rootCanvasGroup == null)
                return;

            _currentSequence?.Kill();

            rootCanvasGroup.gameObject.SetActive(true);
            rootCanvasGroup.alpha = 0f;

            if (correctIcon != null)
            {
                correctIcon.gameObject.SetActive(isCorrect);
                correctIcon.rectTransform.localScale = _originalCorrectScale;
            }

            if (wrongIcon != null)
            {
                wrongIcon.gameObject.SetActive(!isCorrect);
                wrongIcon.rectTransform.localScale = _originalWrongScale;
            }

            RectTransform activeIcon = isCorrect
                ? correctIcon != null ? correctIcon.rectTransform : null
                : wrongIcon != null ? wrongIcon.rectTransform : null;

            if (activeIcon == null)
            {
                rootCanvasGroup.alpha = 1f;
                return;
            }

            _currentSequence = DOTween.Sequence();

            _currentSequence
                .AppendCallback(() =>
                {
                    rootCanvasGroup.alpha = 0f;
                })
                .Append(rootCanvasGroup.DOFade(1f, popDuration))
                .Join(activeIcon.DOScale(_originalCorrectScale * popScale, popDuration).SetEase(Ease.OutBack))
                .AppendInterval(holdDuration)
                .Append(rootCanvasGroup.DOFade(0f, fadeOutDuration))
                .OnComplete(() =>
                {
                    if (correctIcon != null) correctIcon.gameObject.SetActive(false);
                    if (wrongIcon != null) wrongIcon.gameObject.SetActive(false);
                    rootCanvasGroup.gameObject.SetActive(false);
                });
        }


        private void OnDisable()
        {
            _currentSequence?.Kill();
        }
    }
}

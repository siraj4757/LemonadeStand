using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace LemonadeStand.Presentation.UI
{
    public class SplashView : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private CanvasGroup rootCanvasGroup;
        [SerializeField] private Button continueButton;

        [Header("Animation")]
        [SerializeField] private float fadeDuration = 0.5f;

        private Action _onContinue;

        /// <summary>
        /// Entry point from GameFlowController.
        /// </summary>
        public void Show(Action onContinue)
        {
            _onContinue = onContinue;

            gameObject.SetActive(true);

            if (rootCanvasGroup != null)
            {
                rootCanvasGroup.alpha = 0f;

#if DOTWEEN
                rootCanvasGroup.DOFade(1f, fadeDuration).SetUpdate(true);
#else
                rootCanvasGroup.alpha = 1f;
#endif
            }

            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(OnContinueClicked);
        }

        private void OnContinueClicked()
        {
            if (rootCanvasGroup != null)
            {
#if DOTWEEN
                rootCanvasGroup.DOFade(0f, fadeDuration)
                    .SetUpdate(true)
                    .OnComplete(NotifyContinue);
#else
                NotifyContinue();
#endif
            }
            else
            {
                NotifyContinue();
            }
        }


        private void NotifyContinue()
        {
            gameObject.SetActive(false);
            _onContinue?.Invoke();
        }
    }
}

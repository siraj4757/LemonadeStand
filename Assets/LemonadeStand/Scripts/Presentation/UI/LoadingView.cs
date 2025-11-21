using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LemonadeStand.Presentation.UI
{
    public class LoadingView : MonoBehaviour
    {
        [Header("Root")]
        [SerializeField] private GameObject root;

        [Header("Progress UI")]
        [SerializeField] private Image progressFillImage;

        [SerializeField] private TextMeshProUGUI loadingText;


        private void Awake()
        {
            if (root == null)
                root = gameObject;

            ResetProgress();
        }


        public void Show()
        {
            root.SetActive(true);
            ResetProgress();
        }


        public void Hide()
        {
            root.SetActive(false);
        }


        public void ResetProgress()
        {
            if (progressFillImage != null)
                progressFillImage.fillAmount = 0f;

            if (loadingText != null)
                loadingText.text = "Loading 0%";
        }

     
        public IEnumerator PlayProgress(float duration)
        {
            ResetProgress();

            if (duration <= 0f)
            {
                if (progressFillImage != null)
                    progressFillImage.fillAmount = 1f;

                if (loadingText != null)
                    loadingText.text = "Loading 100%";
                yield break;
            }

            float t = 0f;
            int lastPercent = -1;

            while (t < duration)
            {
                t += Time.deltaTime;
                float normalized = Mathf.Clamp01(t / duration);

                if (progressFillImage != null)
                    progressFillImage.fillAmount = normalized;

                if (loadingText != null)
                {
                    int percent = Mathf.RoundToInt(normalized * 100f);
                    if (percent != lastPercent)
                    {
                        loadingText.text = $"Loading {percent}%";
                        lastPercent = percent;
                    }
                }

                yield return null;
            }

            if (progressFillImage != null)
                progressFillImage.fillAmount = 1f;

            if (loadingText != null)
                loadingText.text = "Loading 100%";
        }
    }
}

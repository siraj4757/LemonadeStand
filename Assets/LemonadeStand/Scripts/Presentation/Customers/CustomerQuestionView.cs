using DG.Tweening;
using LemonadeStand.Core.Domain;
using LemonadeStand.Core.Game;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LemonadeStand.Presentation.UI
{
    public class CustomerQuestionView : MonoBehaviour
    {
        [Header("Root")]
        [SerializeField] private GameObject root;

        [Header("Question UI")]
        [SerializeField] private TextMeshProUGUI promptLabel;
        [SerializeField] private TextMeshProUGUI modeLabel;

        [Serializable]
        private class AnswerButton
        {
            public Button button;
            public TextMeshProUGUI label;
        }

        [Header("Answer Buttons")]
        [SerializeField] private AnswerButton[] answerButtons;

        [Header("Customer & Effects")]
        [SerializeField] private CustomerView customerView;
        //[SerializeField] private ContainerFillView fillView;
        [SerializeField] private FeedbackVfxView feedbackView;
        [SerializeField] private Audio.UnityAudioService audioService;

        [Header("Customer Profiles")]
        [SerializeField] private CustomerProfile[] customerProfiles;

        [Header("Size Option Sprites")]
        [SerializeField] private Sprite bigSprite;

        [SerializeField] private Sprite smallSprite;

        [SerializeField] private Sprite massiveSprite;

        [Header("Count Lemons Board (Level 2)")]
        [SerializeField] private CountLemonsBoardView countLemonsBoard;

        private CustomerRequest _currentRequest;
        private Action<bool> _onAnswerEvaluated;
        private int[] _numericOptions;


        public void Show(CustomerRequest request, string modeText, Action<bool> onAnswerEvaluated)
        {
            _currentRequest = request;
            _onAnswerEvaluated = onAnswerEvaluated;

            root.SetActive(true);

            if (modeLabel != null)
                modeLabel.text = modeText;

            if (promptLabel != null)
                promptLabel.text = request.Prompt;

            SetupRandomCustomer();
            customerView?.Show();
            customerView?.PlayTalk();

            SetupAnswersForRequest(request);
        }


        private void SetupRandomCustomer()
        {
            if (customerView == null)
                return;

            if (customerProfiles == null || customerProfiles.Length == 0)
                return;

            int index = UnityEngine.Random.Range(0, customerProfiles.Length);
            var profile = customerProfiles[index];

            if (profile == null)
                return;

            customerView.SetCustomerSprites(
                profile.idleSprite,
                profile.talkingSprite,
                profile.happySprite,
                profile.sadSprite);
        }


        private void SetupAnswersForRequest(CustomerRequest request)
        {
            foreach (var a in answerButtons)
            {
                a.button.onClick.RemoveAllListeners();
                a.button.gameObject.SetActive(false);
                a.button.interactable = true;
            }

            switch (request.LevelType)
            {
                case LevelType.SizeSelection:
                    SetupSizeSelection(request);
                    break;

                case LevelType.CountLemons:
                    SetupCountLemonsPhase1(request);
                    break;

                case LevelType.CompareContainers:
                    SetupCompareContainers(request);
                    break;
            }
        }


        private void SetupCountLemonsPhase1(CustomerRequest request)
        {
            foreach (var a in answerButtons)
            {
                a.button.gameObject.SetActive(false);
            }

            if (promptLabel != null)
                promptLabel.text = "Tap the box to add lemons. Count them as they go into the jar.";

            if (countLemonsBoard != null)
            {
                countLemonsBoard.Show(request.TargetLemonCount, () =>
                {
                    if (promptLabel != null)
                        promptLabel.text = "How many lemons fill the jar?";

                    SetupCountLemonsButtons(request); 
                });
            }
            else
            {
                SetupCountLemonsButtons(request);
            }
        }


        private void SetupCountLemonsButtons(CustomerRequest request)
        {
            _numericOptions = new int[Mathf.Min(3, answerButtons.Length)];

            int correct = request.TargetLemonCount;
            int index = 0;

            if (index < _numericOptions.Length)
                _numericOptions[index++] = Mathf.Max(1, correct - 1);
            if (index < _numericOptions.Length)
                _numericOptions[index++] = correct;
            if (index < _numericOptions.Length)
                _numericOptions[index++] = correct + 1;

            for (int i = 0; i < _numericOptions.Length; i++)
            {
                int capturedIndex = i;
                var btn = answerButtons[i];

                var image = btn.button.GetComponent<Image>();
                if (image != null)
                    image.sprite = null; 

                btn.label.text = _numericOptions[i].ToString();
                btn.button.gameObject.SetActive(true);
                btn.button.onClick.AddListener(() => OnAnswerClicked(capturedIndex));
            }
        }


        private void SetupSizeSelection(CustomerRequest request)
        {
            for (int i = 0; i < request.SizeOptions.Length && i < answerButtons.Length; i++)
            {
                int capturedIndex = i;
                var btn = answerButtons[i];

                // 1) Set the button's sprite based on the size word
                var image = btn.button.GetComponent<Image>();
                if (image != null)
                {
                    Sprite sizeSprite = GetSpriteForSize(request.SizeOptions[i]);
                    if (sizeSprite != null)
                        image.sprite = sizeSprite;
                }

                
                 btn.label.text = string.Empty;
                //btn.label.text = request.SizeOptions[i];

                btn.button.gameObject.SetActive(true);
                btn.button.onClick.AddListener(() => OnAnswerClicked(capturedIndex));
            }
        }

        private Sprite GetSpriteForSize(string sizeText)
        {
            if (string.IsNullOrEmpty(sizeText))
                return null;

            string lower = sizeText.ToLowerInvariant();

            if (lower.Contains("small"))
                return smallSprite;

            if (lower.Contains("massive") || lower.Contains("very"))
                return massiveSprite != null ? massiveSprite : bigSprite;

            if (lower.Contains("big"))
                return bigSprite;

            return null;
        }


        private void SetupCompareContainers(CustomerRequest request)
        {
            int count = Mathf.Min(request.ContainerVolumes.Length, answerButtons.Length);

            for (int i = 0; i < count; i++)
            {
                int capturedIndex = i;
                var btn = answerButtons[i];

                var image = btn.button.GetComponent<Image>();
                if (image != null &&
                    request.ContainerSprites != null &&
                    i < request.ContainerSprites.Count)
                {
                    var sprite = request.ContainerSprites[i];
                    if (sprite != null)
                    {
                        image.sprite = sprite;
                        image.preserveAspect = true;
                    }
                }

                if (btn.label != null)
                {
                    char letter = (char)('A' + i);
                    btn.label.text = letter.ToString();
                }

                btn.button.gameObject.SetActive(true);
                btn.button.onClick.AddListener(() => OnAnswerClicked(capturedIndex));
            }

            // Hides any extra buttons not used by this level
            for (int i = count; i < answerButtons.Length; i++)
            {
                answerButtons[i].button.gameObject.SetActive(false);
            }
        }


        private void OnAnswerClicked(int chosenIndex)
        {
            bool isCorrect = EvaluateAnswer(chosenIndex);

            if (isCorrect)
            {
                audioService?.PlaySfx("correct");
                customerView?.PlayHappy();
                feedbackView?.PlayCorrect();

                if (_currentRequest.LevelType == LevelType.SizeSelection)
                {
                    //fillView?.PlayFillAnimation();
                }
            }
            else
            {
                audioService?.PlaySfx("wrong");
                customerView?.PlaySad();
                feedbackView?.PlayWrong();
            }

            foreach (var a in answerButtons)
            {
                a.button.interactable = false;
            }

            if (_currentRequest.LevelType == LevelType.CountLemons && countLemonsBoard != null)
            {
                countLemonsBoard.Hide();
            }

            AdvanceAfterCustomerExit(isCorrect);
        }

        private void AdvanceAfterCustomerExit(bool isCorrect)
        {
            if (customerView != null)
            {
                const float reactionDelay = 0.5f;

                DOVirtual.DelayedCall(reactionDelay, () =>
                {
                    customerView.PlayExit(() =>
                    {
                        _onAnswerEvaluated?.Invoke(isCorrect);
                    });
                });
            }
            else
            {
                _onAnswerEvaluated?.Invoke(isCorrect);
            }
        }

        private bool EvaluateAnswer(int chosenIndex)
        {
            switch (_currentRequest.LevelType)
            {
                case LevelType.SizeSelection:
                    // Right now correct option is always index 0 in sizeOptions (Big or Small depending on level)
                    return chosenIndex == 0;

                case LevelType.CountLemons:
                    if (_numericOptions == null || chosenIndex >= _numericOptions.Length)
                        return false;
                    return _numericOptions[chosenIndex] == _currentRequest.TargetLemonCount;

                case LevelType.CompareContainers:
                    return chosenIndex == _currentRequest.CorrectContainerIndex;

                default:
                    return false;
            }
        }

        public void Hide()
        {
            root.SetActive(false);
        }
    }
}

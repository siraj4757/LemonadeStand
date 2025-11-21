using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LemonadeStand.Presentation.UI
{
    /// <summary>
    /// Screen shown after all practice levels are completed.
    /// </summary>
    public sealed class PracticeCompleteView : MonoBehaviour
    {
        [Header("Root")]
        [SerializeField] private GameObject root;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI messageText;

        [Header("Buttons")]
        [SerializeField] private Button startTestButton;
        [SerializeField] private Button replayPracticeButton;

        private Action _onStartTest;
        private Action _onReplayPractice;


        private void Awake()
        {
            if (root == null)
                root = gameObject;

            if (startTestButton != null)
                startTestButton.onClick.AddListener(OnStartTestClicked);

            if (replayPracticeButton != null)
                replayPracticeButton.onClick.AddListener(OnReplayPracticeClicked);
        }


        public void Show(string studentName, int correctAnswers,int totalQuestions,Action onStartTest,Action onReplayPractice)
        {
            _onStartTest = onStartTest;
            _onReplayPractice = onReplayPractice;

            if (titleText != null)
                titleText.text = "Practice Complete!";

            if (messageText != null)
            {
                messageText.text =
                    $"{studentName}, you answered {correctAnswers} / {totalQuestions} questions correctly.\n" +
                    "When you’re ready, start the test!";
            }
            root.SetActive(true);
        }


        public void Hide()
        {
            root.SetActive(false);
        }


        private void OnStartTestClicked()
        {
            _onStartTest?.Invoke();
        }


        private void OnReplayPracticeClicked()
        {
            _onReplayPractice?.Invoke();
        }
    }
}

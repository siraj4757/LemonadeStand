using System;
using UnityEngine;
using UnityEngine.UI;
using LemonadeStand.Core.Domain;
using TMPro;

namespace LemonadeStand.Presentation.UI
{
    /// <summary>
    /// Final score screen after test mode is finished.
    /// </summary>
    public class ScoreView : MonoBehaviour
    {
        [Header("Root")]
        [SerializeField] private GameObject root;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI studentNameLabel;
        [SerializeField] private TextMeshProUGUI scoreLabel;
        [SerializeField] private TextMeshProUGUI summaryLabel;

        [Header("Buttons")]
        [SerializeField] private Button replayPracticeButton;
        [SerializeField] private Button exitButton;

        private Action _onReplayPractice;
        private Action _onExit;


        public void Show(PlayerProfile profile, Action onReplayPractice = null, Action onExit = null)
        {
            _onReplayPractice = onReplayPractice;
            _onExit = onExit;

            root.SetActive(true);

            if (studentNameLabel != null)
                studentNameLabel.text = profile.StudentName;

            if (scoreLabel != null)
                scoreLabel.text = $"Score: {profile.TotalScore}";

            if (summaryLabel != null)
                summaryLabel.text = GetSummaryText(profile.TotalScore);

            replayPracticeButton.onClick.RemoveAllListeners();
            exitButton.onClick.RemoveAllListeners();

            if (_onReplayPractice != null)
                replayPracticeButton.onClick.AddListener(OnReplayPracticeClicked);
            else
                replayPracticeButton.gameObject.SetActive(false);

            if (_onExit != null)
                exitButton.onClick.AddListener(OnExitClicked);
        }


        private string GetSummaryText(int score)
        {
            // Simple example for later use
            if (score >= 80)
                return "Amazing! You're a Lemonade Pro! ";
            if (score >= 50)
                return "Great job! Keep practicing! ";
            return "Good start! Try again to improve! ";
        }


        private void OnReplayPracticeClicked()
        {
            root.SetActive(false);
            _onReplayPractice?.Invoke();
        }


        private void OnExitClicked()
        {
            root.SetActive(false);
            _onExit?.Invoke();
        }
    }
}

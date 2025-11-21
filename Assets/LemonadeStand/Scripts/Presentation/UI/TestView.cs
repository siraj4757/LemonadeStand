using System;
using UnityEngine;
using UnityEngine.UI;
using LemonadeStand.Core.Domain;
using LemonadeStand.Core.Game;

namespace LemonadeStand.Presentation.UI
{
    /// <summary>
    /// High-level view for test mode.
    /// </summary>
    public class TestView : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private CustomerQuestionView questionView;
        [SerializeField] private SettingsView settingsView;

        [SerializeField] private Button settingsButton;


        public void Show(CustomerRequest request, Action<bool> onAnswerEvaluated)
        {
            root.SetActive(true);
            settingsButton.onClick.RemoveAllListeners();
            settingsButton.onClick.AddListener(OnSettingsButtonClicked);

            questionView.Show(request,modeText: "Test",
            onAnswerEvaluated: (isCorrect) =>
            {
                root.SetActive(false);
                onAnswerEvaluated?.Invoke(isCorrect);
            });
        }


        public void OnSettingsButtonClicked()
        {
            if (settingsView != null)
                settingsView.Show();
        }
    }
}

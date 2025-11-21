using System;
using UnityEngine;
using LemonadeStand.Core.Domain;
using LemonadeStand.Core.Game;

namespace LemonadeStand.Presentation.UI
{
    /// <summary>
    /// High-level view for practice mode.
    /// Delegates actual question rendering to CustomerQuestionView.
    /// </summary>
    public class PracticeView : MonoBehaviour
    {
        [SerializeField] private GameObject root;
        [SerializeField] private CustomerQuestionView questionView;

        public void Show(CustomerRequest request, Action<bool> onAnswerEvaluated)
        {
            root.SetActive(true);

            questionView.Show(request,modeText: "Practice",
            onAnswerEvaluated: (isCorrect) =>
            {
                root.SetActive(false);
                onAnswerEvaluated?.Invoke(isCorrect);
            });
        }


        public void Hide()
        {
            root.SetActive(false);
        }
    }
}

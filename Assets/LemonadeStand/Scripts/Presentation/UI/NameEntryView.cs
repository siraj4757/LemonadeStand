using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LemonadeStand.Presentation.UI
{
    /// <summary>
    /// Simple view that asks student to type their name.
    /// </summary>
    public class NameEntryView : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject root;
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private Button confirmButton;
        [SerializeField] private TextMeshProUGUI errorLabel;          

        private Action<string> _onNameConfirmed;

        public void Show(Action<string> onNameConfirmed)
        {
            _onNameConfirmed = onNameConfirmed;

            root.SetActive(true);
            errorLabel.gameObject.SetActive(false);
            nameInputField.text = string.Empty;

            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(OnConfirmClicked);
        }

        private void OnConfirmClicked()
        {
            string name = nameInputField.text.Trim();

            if (string.IsNullOrEmpty(name))
            {
                errorLabel.text = "Please type your name ";
                errorLabel.gameObject.SetActive(true);
                return;
            }

            confirmButton.onClick.RemoveListener(OnConfirmClicked);
            root.SetActive(false);
            _onNameConfirmed?.Invoke(name);
        }

    }
}

using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LemonadeStand.Presentation.UI
{
    public class SettingsView : MonoBehaviour
    {
        [Header("Root")]
        [SerializeField] private GameObject root;

        [Header("Toggles")]
        [SerializeField] private Toggle musicToggle;
        [SerializeField] private Toggle sfxToggle;

        [Header("Buttons")]
        [SerializeField] private Button closeButton;




        private void Awake()
        {
            if (root == null)
                root = gameObject;

            bool musicOn = true;
            bool sfxOn = true;

            if (UnityAudioService.Instance != null)
            {
                musicOn = UnityAudioService.Instance.GetMusicEnabled();
                sfxOn = UnityAudioService.Instance.GetSfxEnabled();
            }

            if (musicToggle != null)
            {
                musicToggle.isOn = musicOn;
                musicToggle.onValueChanged.AddListener(OnMusicToggleChanged);
            }

            if (sfxToggle != null)
            {
                sfxToggle.isOn = sfxOn;
                sfxToggle.onValueChanged.AddListener(OnSfxToggleChanged);
            }

            if (closeButton != null)
                closeButton.onClick.AddListener(Hide);

            ApplyMusic(musicOn);
            ApplySfx(sfxOn);
        }


        public void Show()
        {
            root.SetActive(true);
        }


        public void Hide()
        {
            root.SetActive(false);
        }


        private void OnMusicToggleChanged(bool isOn)
        {
            if (UnityAudioService.Instance != null)
                UnityAudioService.Instance.SetMusicEnabled(isOn);
        }


        private void OnSfxToggleChanged(bool isOn)
        {
            if (UnityAudioService.Instance != null)
                UnityAudioService.Instance.SetSfxEnabled(isOn);
        }

       
        private void ApplyMusic(bool isOn)
        {
            if (Audio.UnityAudioService.Instance != null)
                Audio.UnityAudioService.Instance.SetMusicEnabled(isOn);
        }


        private void ApplySfx(bool isOn)
        {
            if (Audio.UnityAudioService.Instance != null)
                Audio.UnityAudioService.Instance.SetSfxEnabled(isOn);
        }
    }
}

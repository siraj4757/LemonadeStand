using System;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    /// <summary>
    /// Simple audio service for this project:
    /// - One music AudioSource
    /// - One SFX AudioSource (PlayOneShot)
    /// </summary>
    public class UnityAudioService : MonoBehaviour
    {
        [Serializable]
        private class AudioClipEntry
        {
            public string key;
            public AudioClip clip;
            public bool isMusic; 
        }

        public static UnityAudioService Instance { get; private set; }

        [Header("Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;

        [Header("Clips")]
        [SerializeField] private AudioClipEntry[] clipEntries;

        [Header("Startup")]
        [SerializeField] private string initialMusicKey = "bgm";

        private const string MusicPrefKey = "settings.musicOn";
        private const string SfxPrefKey = "settings.sfxOn";

        private readonly Dictionary<string, AudioClip> _musicClips = new Dictionary<string, AudioClip>();
        private readonly Dictionary<string, AudioClip> _sfxClips = new Dictionary<string, AudioClip>();

        private bool _musicEnabled = true;
        private bool _sfxEnabled = true;


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            BuildLookup();

            bool musicOn = PlayerPrefs.GetInt(MusicPrefKey, 1) == 1;
            bool sfxOn = PlayerPrefs.GetInt(SfxPrefKey, 1) == 1;

            _musicEnabled = musicOn;
            _sfxEnabled = sfxOn;
        }

        private void Start()
        {
            if (!string.IsNullOrEmpty(initialMusicKey))
            {
                PlayMusic(initialMusicKey, loop: true);
            }
        }


        private void BuildLookup()
        {
            _musicClips.Clear();
            _sfxClips.Clear();

            if (clipEntries == null) return;

            foreach (var entry in clipEntries)
            {
                if (entry == null || string.IsNullOrEmpty(entry.key) || entry.clip == null)
                    continue;

                if (entry.isMusic)
                    _musicClips[entry.key] = entry.clip;
                else
                    _sfxClips[entry.key] = entry.clip;
            }
        }

        public bool GetMusicEnabled() => _musicEnabled;
        public bool GetSfxEnabled() => _sfxEnabled;

        public void SetMusicEnabled(bool enabled)
        {
            _musicEnabled = enabled;

            PlayerPrefs.SetInt(MusicPrefKey, enabled ? 1 : 0);
            PlayerPrefs.Save();

            if (!enabled)
            {
                StopMusic();
            }
            else if (!string.IsNullOrEmpty(initialMusicKey))
            {
                PlayMusic(initialMusicKey, loop: true);
            }
        }

        public void SetSfxEnabled(bool enabled)
        {
            _sfxEnabled = enabled;

            PlayerPrefs.SetInt(SfxPrefKey, enabled ? 1 : 0);
            PlayerPrefs.Save();
        }

        public void PlayMusic(string key, bool loop = true)
        {
            if (!_musicEnabled || musicSource == null) return;
            if (!_musicClips.TryGetValue(key, out var clip) || clip == null) return;

            if (musicSource.clip == clip && musicSource.isPlaying)
                return;

            musicSource.clip = clip;
            musicSource.loop = loop;
            musicSource.Play();
        }


        public void StopMusic()
        {
            if (musicSource != null)
                musicSource.Stop();
        }


        public void PlaySfx(string key)
        {
            if (!_sfxEnabled || sfxSource == null) return;
            if (!_sfxClips.TryGetValue(key, out var clip) || clip == null) return;

            sfxSource.PlayOneShot(clip);
        }


       
    }
}

using UnityEngine;
using System.Collections.Generic;

namespace DZAssets.AudioManager
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Audio Data")]
        [SerializeField] private AudioData bgmData;
        [SerializeField] private AudioData sfxData;

        [Header("Audio Sources")]
        [SerializeField] private AudioSource bgmAudioSource;
        [SerializeField] private AudioSource sfxAudioSource;

        [Header("Settings")]
        [SerializeField] private bool useLog = true;

        private static AudioManager instance;
        
        // Cache untuk menghindari pencarian berulang
        private Dictionary<string, AudioClip> bgmCache = new Dictionary<string, AudioClip>();
        private Dictionary<string, AudioClip> sfxCache = new Dictionary<string, AudioClip>();
        private bool cacheInitialized = false;
        public static AudioManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindFirstObjectByType<AudioManager>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("AudioManager");
                        instance = go.AddComponent<AudioManager>();
                    }
                }
                return instance;
            }
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeAudioSources();
                if (useLog) Debug.Log("[DZAssets] AudioManager: Initialized successfully");
            }
            else if (instance != this)
            {
                if (useLog) Debug.LogWarning("[DZAssets] AudioManager: Duplicate instance destroyed");
                Destroy(gameObject);
            }

            bgmAudioSource.volume = PlayerPrefs.GetFloat("BGM_VOLUME", 1f);
            sfxAudioSource.volume = PlayerPrefs.GetFloat("SFX_VOLUME", 1f);
        }

        void InitializeAudioSources()
        {
            if (bgmAudioSource == null)
            {
                bgmAudioSource = gameObject.AddComponent<AudioSource>();
                bgmAudioSource.loop = true;
                bgmAudioSource.playOnAwake = false;
            }

            if (sfxAudioSource == null)
            {
                sfxAudioSource = gameObject.AddComponent<AudioSource>();
                sfxAudioSource.loop = false;
                sfxAudioSource.playOnAwake = false;
            }
            
            InitializeCache();
        }

        void InitializeCache()
        {
            if (cacheInitialized) return;
            
            // Cache BGM
            if (bgmData?.audioList != null)
            {
                bgmCache.Clear();
                foreach (var entry in bgmData.audioList)
                {
                    if (!string.IsNullOrEmpty(entry.nameAudio) && entry.audioClip != null)
                        bgmCache[entry.nameAudio] = entry.audioClip;
                }
            }
            
            // Cache SFX
            if (sfxData?.audioList != null)
            {
                sfxCache.Clear();
                foreach (var entry in sfxData.audioList)
                {
                    if (!string.IsNullOrEmpty(entry.nameAudio) && entry.audioClip != null)
                        sfxCache[entry.nameAudio] = entry.audioClip;
                }
            }
            
            cacheInitialized = true;
        }

        #region BGM Methods
        public void PlayBGM(string audioName)
        {
            if (string.IsNullOrEmpty(audioName)) return;
            
            if (!cacheInitialized) InitializeCache();
            
            if (bgmCache.TryGetValue(audioName, out AudioClip clip))
            {
                if (useLog) Debug.Log($"[DZAssets] AudioManager: Playing BGM '{audioName}'");
                bgmAudioSource.clip = clip;
                bgmAudioSource.Play();
            }
            else
            {
                if (useLog) Debug.LogWarning($"[DZAssets] AudioManager: BGM '{audioName}' not found in BGM cache!");
            }
        }

        public void StopBGM()
        {
            if (bgmAudioSource != null)
            {
                if (useLog) Debug.Log("[DZAssets] AudioManager: Stopping BGM");
                bgmAudioSource.Stop();
                bgmAudioSource.clip = null;
            }
        }

        public void PauseBGM()
        {
            if (bgmAudioSource != null)
            {
                bgmAudioSource.Pause();
            }
        }

        public void ResumeBGM()
        {
            if (bgmAudioSource != null)
            {
                bgmAudioSource.UnPause();
            }
        }
        #endregion

        #region SFX Methods
        public void PlaySFX(string audioName)
        {
            if (string.IsNullOrEmpty(audioName)) return;
            
            if (!cacheInitialized) InitializeCache();
            
            if (sfxCache.TryGetValue(audioName, out AudioClip clip))
            {
                if (useLog) Debug.Log($"[DZAssets] AudioManager: Playing SFX '{audioName}'");
                sfxAudioSource.PlayOneShot(clip);
            }
            else
            {
                if (useLog) Debug.LogWarning($"[DZAssets] AudioManager: SFX '{audioName}' not found in SFX cache!");
            }
        }

        public void StopSFX()
        {
            if (sfxAudioSource != null)
            {
                if (useLog) Debug.Log("[DZAssets] AudioManager: Stopping SFX");
                sfxAudioSource.Stop();
            }
        }
        #endregion

        #region Helper Methods
        // Method ini masih diperlukan untuk GetAvailableAudioNames
        private AudioClip GetAudioClip(AudioData audioData, string audioName)
        {
            if (audioData == null || audioData.audioList == null) return null;

            foreach (var entry in audioData.audioList)
            {
                if (entry.nameAudio == audioName)
                {
                    return entry.audioClip;
                }
            }
            return null;
        }
        
        public void RefreshCache()
        {
            cacheInitialized = false;
            InitializeCache();
            if (useLog) Debug.Log("[DZAssets] AudioManager: Cache refreshed");
        }

        public void SetBGMVolume(float volume)
        {
            if (bgmAudioSource != null)
            {
                bgmAudioSource.volume = Mathf.Clamp01(volume);
            }
        }

        public void SetSFXVolume(float volume)
        {
            if (sfxAudioSource != null)
            {
                sfxAudioSource.volume = Mathf.Clamp01(volume);
            }
        }

        private string GetAvailableAudioNames(AudioData audioData)
        {
            if (audioData == null || audioData.audioList == null || audioData.audioList.Count == 0)
                return "None";

            System.Collections.Generic.List<string> names = new System.Collections.Generic.List<string>();
            foreach (var entry in audioData.audioList)
            {
                if (!string.IsNullOrEmpty(entry.nameAudio))
                    names.Add(entry.nameAudio);
            }
            return names.Count > 0 ? string.Join(", ", names) : "None";
        }
        #endregion
    }
}
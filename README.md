# DZ Assets - Audio Manager

A simple and reusable audio manager system for Unity projects. Easily manage Background Music (BGM) and Sound Effects (SFX) with caching support for better performance.

![Unity](https://img.shields.io/badge/Unity-2021.3%2B-blue)
![Version](https://img.shields.io/badge/version-0.1.0-green)

## ✨ Features

- 🎵 **BGM Management** - Play, Stop, Pause, and Resume background music
- 🔊 **SFX Management** - Play and Stop sound effects
- 📦 **ScriptableObject Based** - Easy to manage audio data
- ⚡ **Audio Caching** - Improved performance with dictionary-based caching
- 💾 **Volume Persistence** - Automatically saves volume settings using PlayerPrefs
- 🔄 **Singleton Pattern** - Easy access from anywhere with `AudioManager.Instance`
- 📝 **Debug Logging** - Optional logging for debugging

## 📦 Installation

### Via Unity Package Manager (Git URL)

1. Open **Window > Package Manager**
2. Click the **+** button and select **Add package from git URL...**
3. Enter the following URL:
   ```
   https://github.com/danyzaky/com.dzassets.audiomanager.git
   ```

### Manual Installation

1. Download or clone this repository
2. Copy the folder to your Unity project's `Packages` folder

## 🚀 Quick Start

### 1. Create Audio Data

Create ScriptableObjects to store your audio clips:

1. Right-click in Project window
2. Select **Create > Audio > AudioData**
3. Name it (e.g., "BGM Data" or "SFX Data")
4. Add your audio clips with unique names

### 2. Setup AudioManager

1. Drag the `[DZAssets] AudioManager` prefab into your first scene
2. Assign your BGM Data and SFX Data ScriptableObjects to the AudioManager
3. The AudioManager will persist across scenes automatically

### 3. Play Audio

```csharp
using DZAssets.AudioManager;

// Play BGM
AudioManager.Instance.PlayBGM("bgm-menu");

// Play SFX
AudioManager.Instance.PlaySFX("button");
```

## 📖 API Reference

### BGM Methods

| Method                       | Description                    |
| ---------------------------- | ------------------------------ |
| `PlayBGM(string audioName)`  | Play background music by name  |
| `StopBGM()`                  | Stop current background music  |
| `PauseBGM()`                 | Pause current background music |
| `ResumeBGM()`                | Resume paused background music |
| `SetBGMVolume(float volume)` | Set BGM volume (0.0 - 1.0)     |

### SFX Methods

| Method                       | Description                |
| ---------------------------- | -------------------------- |
| `PlaySFX(string audioName)`  | Play sound effect by name  |
| `StopSFX()`                  | Stop current sound effect  |
| `SetSFXVolume(float volume)` | Set SFX volume (0.0 - 1.0) |

### Utility Methods

| Method           | Description                                                         |
| ---------------- | ------------------------------------------------------------------- |
| `RefreshCache()` | Refresh the audio cache (call after modifying AudioData at runtime) |

## 📁 AudioData ScriptableObject

The `AudioData` ScriptableObject contains a list of audio entries:

```csharp
[CreateAssetMenu(fileName = "AudioData", menuName = "Audio/AudioData", order = 1)]
public class AudioData : ScriptableObject
{
    public List<AudioEntry> audioList;
}

[System.Serializable]
public class AudioEntry
{
    public string nameAudio;    // Unique identifier for the audio
    public AudioClip audioClip; // The actual audio clip
}
```

## 💡 Usage Examples

### Playing BGM with Scene Changes

```csharp
using UnityEngine;
using DZAssets.AudioManager;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        // Play menu music
        AudioManager.Instance.PlayBGM("bgm-menu");
    }

    public void StartGame()
    {
        // Switch to gameplay music
        AudioManager.Instance.PlayBGM("bgm-gameplay");
    }
}
```

### Playing SFX on Button Click

```csharp
using UnityEngine;
using UnityEngine.UI;
using DZAssets.AudioManager;

public class ButtonSound : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => {
            AudioManager.Instance.PlaySFX("button");
        });
    }
}
```

### Volume Control with UI Slider

```csharp
using UnityEngine;
using UnityEngine.UI;
using DZAssets.AudioManager;

public class VolumeSettings : MonoBehaviour
{
    public Slider bgmSlider;
    public Slider sfxSlider;

    void Start()
    {
        bgmSlider.value = PlayerPrefs.GetFloat("BGM_VOLUME", 1f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX_VOLUME", 1f);

        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    void OnBGMVolumeChanged(float value)
    {
        AudioManager.Instance.SetBGMVolume(value);
        PlayerPrefs.SetFloat("BGM_VOLUME", value);
    }

    void OnSFXVolumeChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
        PlayerPrefs.SetFloat("SFX_VOLUME", value);
    }
}
```

## ⚙️ Configuration

### AudioManager Settings

| Setting              | Description                                          |
| -------------------- | ---------------------------------------------------- |
| **BGM Data**         | ScriptableObject containing background music entries |
| **SFX Data**         | ScriptableObject containing sound effect entries     |
| **BGM Audio Source** | AudioSource for BGM (auto-created if not assigned)   |
| **SFX Audio Source** | AudioSource for SFX (auto-created if not assigned)   |
| **Use Log**          | Enable/disable debug logging                         |

## 📋 Requirements

- Unity 2021.3 or higher

## 👤 Author

**Dany Zaky**

## 📄 License

This project is available for use in your Unity projects.

using UnityEngine;
using System.Collections.Generic;

namespace DZAssets.AudioManager
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "DZAssets/AudioData", order = 1)]
    public class AudioData : ScriptableObject
    {
        [System.Serializable]
        public class AudioEntry
        {
            public string nameAudio;
            public AudioClip audioClip;
        }

        public List<AudioEntry> audioList = new List<AudioEntry>();
    }
}

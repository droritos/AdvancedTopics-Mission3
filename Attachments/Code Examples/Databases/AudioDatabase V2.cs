    using System.Collections.Generic;
    using UnityEngine;

    namespace GameAudio
    {
        [CreateAssetMenu(menuName = "Database/Audio Database")]
        public class AudioDatabase : ScriptableObject
        {
            [Header("Baked entries")]
            public List<AudioEntry> entries = new List<AudioEntry>();
        }
    }
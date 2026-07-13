using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameAudio
{
[CreateAssetMenu(menuName = "Database/Audio Database")]
public class AudioDatabase : ScriptableObject
{
    [Serializable]
    public class AudioEntry
    {
        public string id;       // usually clip.name
        public AudioClip clip;
    }

    [Header("Baked entries")]
    public List<AudioEntry> entries = new List<AudioEntry>();

    // Runtime lookup cache (built on demand)
    private Dictionary<string, AudioClip> cache;

    public void BuildCache()
    {
        cache = new Dictionary<string, AudioClip>(StringComparer.Ordinal);

        foreach (var e in entries)
        {
            if (e == null || string.IsNullOrEmpty(e.id) || e.clip == null) continue;
            cache[e.id] = e.clip; // if duplicates, last one wins
        }
    }

    public bool TryGet(string id, out AudioClip clip)
    {
        if (cache == null) BuildCache();
        return cache.TryGetValue(id, out clip);
    }
}
}

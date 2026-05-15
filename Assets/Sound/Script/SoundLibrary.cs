using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audio/Sound Library")]
public class SoundLibrary : ScriptableObject
{
    public SoundEntry[] entries;
    private Dictionary<string, SoundEntry> _map;

    public SoundEntry Get(string key)
    {
        if (_map == null)
        {
            _map = new Dictionary<string, SoundEntry>();
            foreach (var e in entries) _map[e.key] = e;
        }
        return _map.TryGetValue(key, out var val) ? val : null;
    }
}
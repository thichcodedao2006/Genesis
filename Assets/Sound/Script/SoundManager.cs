using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private SoundLibrary globalLibrary;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource footstepSource; 
    private SoundLibrary localLibrary;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ── Library ──────────────────────────────────────
    public void LoadLocalLibrary(SoundLibrary lib) => localLibrary = lib;
    public void UnloadLocalLibrary() => localLibrary = null;

    private SoundEntry GetEntry(string key)
    {
        var entry = localLibrary?.Get(key) ?? globalLibrary.Get(key);
        if (entry == null) Debug.LogWarning($"[SoundManager] Key not found: {key}");
        else Debug.Log($"[SoundManager] Found: {key}, clip: {entry.clip}");
        return entry;
    }

    // ── SFX ──────────────────────────────────────────
    public void PlaySFX(string key, bool randomPitching = false)
    {
        var entry = GetEntry(key);
        if (entry == null) return;
        sfxSource.PlayOneShot(entry.clip, entry.volume);
    }
    public void PlayFootstep(string key)
    {
        
        var entry = GetEntry(key);
        if (entry == null) return;
        if (footstepSource.isPlaying && footstepSource.clip == entry.clip) return;
        Debug.Log("đang chạy trên nền " + entry.key);
        footstepSource.clip = entry.clip;
        footstepSource.loop = true;
        footstepSource.Play();
    }

    public void StopFootstep() => footstepSource.Stop();
    // ── BGM ──────────────────────────────────────────
    public void PlayBGM(string key, bool loop = true)
    {
        var entry = GetEntry(key);
        if (entry == null) return;
        if (bgmSource.clip == entry.clip && bgmSource.isPlaying) return;
        bgmSource.clip = entry.clip;
        bgmSource.loop = loop;
        bgmSource.volume = entry.volume;
        bgmSource.Play();
    }

    public void StopBGM() => bgmSource.Stop();
    public void PauseBGM() => bgmSource.Pause();
    public void ResumeBGM() => bgmSource.UnPause();

    public void SetUpLocalLibraryAndPlayBM(SoundLibrary soundLibrary , string soundKey) 
    {
        LoadLocalLibrary(soundLibrary);
        PlayBGM(soundKey);
    }
    public void ResetLocalLibraryAndPlayBM()
    {
        StopBGM();
        UnloadLocalLibrary(); 
    }

    // Cái nào hay sài viết tắt luôn =)))

    public static void PlayClickUI() => Instance.PlaySFX(SoundKey.ClickUI);
    public static void PlayOpenDoor() => Instance.PlaySFX(SoundKey.OpenDoor);

    public static void PlayCloseDoor() => Instance.PlaySFX(SoundKey.CloseDoor);

    public static void PlayOpenBackPack() => Instance.PlaySFX(SoundKey.OpenBackpack);
    public static void PlayCloseBackPack() => Instance.PlaySFX(SoundKey.CloseBackpack);

    public static void PlayOpenPhone() => Instance.PlaySFX(SoundKey.OpenPhone);

    public static void PlayCompleteLevel() => Instance.PlaySFX(SoundKey.CompleteLevel); 
}
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource voiceSource;

    [Header("Music Transition")]
    [SerializeField] private float musicFadeDuration = 1.0f;

    private Coroutine musicFadeRoutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Continuously sync volumes from GameSettings
        if (GameSettings.Instance == null) return;

        musicSource.volume = GameSettings.Instance.musicVolume;
        sfxSource.volume = GameSettings.Instance.sfxVolume;
        voiceSource.volume = GameSettings.Instance.voiceVolume;
    }

    private void InitializeSources()
    {
        // Music source
        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
        }

        // SFX source
        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }

        // Voice source
        if (voiceSource == null)
        {
            voiceSource = gameObject.AddComponent<AudioSource>();
            voiceSource.loop = false;
            voiceSource.playOnAwake = false;
        }

    }

    // Music

    public void PlayMusic(AudioClip clip, bool restartIfSame = false)
    {
        if (clip == null) return;

        if (musicSource.clip == clip && musicSource.isPlaying && !restartIfSame)
            return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
        musicSource.clip = null;
    }

    public void PlayMusicOneShot(AudioClip clip, float volumeMultiplier = 1f)
    {
        if (clip == null) return;

        musicSource.PlayOneShot(clip, volumeMultiplier);
    }

    public void SwitchMusic(AudioClip newClip, float fadeDuration = -1f)
    {
        if (newClip == null)
            return;

        if (musicSource.clip == newClip)
            return;

        if (musicFadeRoutine != null)
            StopCoroutine(musicFadeRoutine);

        float duration = fadeDuration > 0 ? fadeDuration : musicFadeDuration;
        musicFadeRoutine = StartCoroutine(FadeMusicRoutine(newClip, duration));
    }

    private System.Collections.IEnumerator FadeMusicRoutine(AudioClip newClip, float duration)
    {
        float startVolume = musicSource.volume;

        // Fade out
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / duration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();

        // Fade in
        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, GameSettings.Instance.musicVolume, t / duration);
            yield return null;
        }

        musicSource.volume = GameSettings.Instance.musicVolume;
        musicFadeRoutine = null;
    }


    // SFX

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip, float volumeMultiplier)
    {
        if (clip == null) return;
        sfxSource.PlayOneShot(clip, volumeMultiplier);
    }

    // Voice

    public void PlayVoice(AudioClip clip, bool interrupt = true)
    {
        if (clip == null) return;

        if (voiceSource.isPlaying && !interrupt)
            return;

        voiceSource.Stop();
        voiceSource.clip = clip;
        voiceSource.Play();
    }

    public void StopVoice()
    {
        voiceSource.Stop();
        voiceSource.clip = null;
    }

    public void PlayVoiceOneShot(AudioClip clip, float volumeMultiplier = 1f)
    {
        if (clip == null) return;

        voiceSource.PlayOneShot(clip, volumeMultiplier);
    }

    public bool IsVoicePlaying()
    {
        return voiceSource.isPlaying;
    }
}

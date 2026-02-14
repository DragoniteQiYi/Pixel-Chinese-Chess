using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] public AudioClip BUTTON_CLICK;
    [SerializeField] public AudioClip CHESS_FLOAT;
    [SerializeField] public AudioClip CHESS_MOVE;
    [SerializeField] public AudioClip CHESS_CAPTURE;
    [SerializeField] public AudioClip GAME_ALARM;
    [SerializeField] public AudioClip GAME_OVER;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = gameObject.GetComponent<SoundManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 播放指定音效
    /// </summary>
    public void PlaySoundEffect(AudioClip clip)
    {
        GameObject gameObject = new GameObject("TempAudio");
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        Destroy(gameObject, clip.length * 2f);
    }
}

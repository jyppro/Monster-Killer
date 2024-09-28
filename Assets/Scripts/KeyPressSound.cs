using UnityEngine;

public class KeyPressSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips; // 사운드 배열
    private AudioSource audioSource; // 오디오 소스

    void Start()
    {
        // AudioSource를 추가하고 설정
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void PlayKeyPressSound(int index)
    {
        // 인덱스가 유효할 경우 오디오 클립을 재생
        if (audioClips != null && index >= 0 && index < audioClips.Length)
        {
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }
    }

    public AudioClip[] GetAudioClips()
    {
        return audioClips;
    }
}

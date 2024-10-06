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
        // 인덱스가 유효하고 audioClips가 비어있지 않은 경우 오디오 클립을 재생
        if (audioClips != null && audioClips.Length > 0 && index >= 0 && index < audioClips.Length)
        {
            audioSource.PlayOneShot(audioClips[index]); // PlayOneShot을 사용하여 겹치는 소리 처리
        }
    }

    public AudioClip[] GetAudioClips()
    {
        return audioClips;
    }
}

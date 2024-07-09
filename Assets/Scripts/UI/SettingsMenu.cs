using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider; // 음량 조절을 위한 슬라이더
    private float volume;
    private void Start()
    {
        // 설정 값 불러오기
        volume = PlayerPrefs.GetFloat("Volume", 1f);

        // 슬라이더에 설정 값 반영
        volumeSlider.value = volume;
    }
    private void Update() { SetVolume(volume); }

    public void SetVolume(float volume)
    {
        // 음량 설정 값 저장
        PlayerPrefs.SetFloat("Volume", volume);

        // 실제 음량 조절 로직
        // 볼륨을 volume 값에 따라 조절하는 코드 작성
        volume = volumeSlider.value;
        AudioListener.volume = volume;
    }
}

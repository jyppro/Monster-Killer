using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider; // 음량 조절을 위한 슬라이더

    private void Start()
    {
        // 설정 값 불러오기 및 슬라이더에 적용
        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        volumeSlider.value = volume;
        SetVolume(volume); // 초기 볼륨 설정
    }

    // 슬라이더 값이 변경될 때 호출되는 메서드
    public void OnVolumeSliderChanged(float value)
    {
        SetVolume(value);
    }

    public void SetVolume(float volume)
    {
        // 음량 설정 값 저장
        PlayerPrefs.SetFloat("Volume", volume);

        // 실제 음량 조절
        AudioListener.volume = volume;
    }
}

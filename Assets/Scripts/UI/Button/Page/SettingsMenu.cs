using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider; // 음량 조절을 위한 슬라이더

    private void Start()
    {
        // 씬이 로드될 때 PlayerPrefs에서 저장된 음량 값을 불러옴
        LoadVolume();

        // 슬라이더의 이벤트를 코드에서 연결
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
    }

    // 슬라이더 값이 변경될 때 호출되는 메서드
    public void OnVolumeSliderChanged(float value)
    {
        SetVolume(value);
    }

    public void SetVolume(float volume)
    {
        // 실제 음량 조절
        AudioListener.volume = volume;

        // 음량 설정 값 저장
        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();  // 즉시 저장
    }

    // 저장된 볼륨을 불러와서 적용하는 함수
    private void LoadVolume()
    {
        // 저장된 음량 값을 불러옴 (저장된 값이 없으면 기본값 1.0f 사용)
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1.0f);

        // 슬라이더 값을 저장된 값으로 설정
        volumeSlider.value = savedVolume;

        // AudioListener의 볼륨도 저장된 값으로 설정
        AudioListener.volume = savedVolume;
    }

    private void OnDisable()
    {
        // 설정창이 닫힐 때 현재 볼륨 값을 저장
        PlayerPrefs.SetFloat("Volume", AudioListener.volume);
        PlayerPrefs.Save();
    }
}

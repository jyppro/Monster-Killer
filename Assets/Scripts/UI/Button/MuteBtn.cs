using UnityEngine;
using UnityEngine.UI;

public class MuteBtn : MonoBehaviour
{
    public Slider volumeSlider;  // 볼륨 슬라이더 참조
    public Sprite mutedImage;    // 음소거 상태 이미지
    public Sprite unmutedImage;  // 음소거 해제 상태 이미지
    private bool isMuted = false; // 음소거 상태 저장
    private Image buttonImage;    // 버튼의 이미지 참조

    void Start()
    {
        // 버튼의 Image 컴포넌트 참조
        buttonImage = GetComponent<Image>();

        // 버튼 클릭 시 ToggleMute 함수 호출하도록 설정
        GetComponent<Button>().onClick.AddListener(ToggleMute);

        // 음소거 상태 불러오기
        LoadMuteState();

        // 초기 상태에서 이미지 및 슬라이더 상태 설정
        UpdateButtonImage();
        UpdateSliderInteractable(); // 슬라이더의 활성화 상태 업데이트
    }

    // 버튼 클릭 시 호출할 함수
    public void ToggleMute()
    {
        if (isMuted)
        {
            // 음소거 해제
            AudioListener.volume = volumeSlider.value; // 슬라이더 값을 사용
        }
        else
        {
            // 음소거 처리
            AudioListener.volume = 0f;
            volumeSlider.value = 0f; // 슬라이더 값도 0으로 설정
        }

        isMuted = !isMuted; // 음소거 상태 토글
        UpdateButtonImage(); // 버튼 이미지 업데이트
        UpdateSliderInteractable(); // 슬라이더 활성화 상태 업데이트
        SaveMuteState();     // 음소거 상태 저장
    }

    // 버튼 이미지를 음소거 상태에 따라 업데이트하는 함수
    private void UpdateButtonImage()
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = isMuted ? mutedImage : unmutedImage;
        }
    }

    // 음소거 상태에 따라 슬라이더의 활성화 상태를 업데이트하는 함수
    private void UpdateSliderInteractable()
    {
        if (volumeSlider != null)
        {
            // 음소거 상태일 때 슬라이더 비활성화, 음소거 해제 시 활성화
            volumeSlider.interactable = !isMuted;
        }
    }

    // 음소거 상태를 PlayerPrefs에 저장하는 함수
    private void SaveMuteState()
    {
        // 현재 음소거 상태 저장
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        
        // 현재 볼륨 값 저장 (음소거 상태가 아니면 현재 볼륨을 저장)
        if (!isMuted)
        {
            PlayerPrefs.SetFloat("Volume", AudioListener.volume);
        }
        
        PlayerPrefs.Save(); // PlayerPrefs 저장
    }

    // 음소거 상태와 볼륨을 PlayerPrefs에서 불러오는 함수
    private void LoadMuteState()
    {
        // 저장된 음소거 상태를 불러오기 (저장된 값이 없으면 0으로 설정)
        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;

        // 저장된 볼륨 값을 불러오기 (저장된 값이 없으면 1.0f 사용)
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1.0f);

        // 불러온 상태에 따라 AudioListener 볼륨 설정
        if (isMuted)
        {
            AudioListener.volume = 0f;
        }
        else
        {
            AudioListener.volume = savedVolume;
            volumeSlider.value = savedVolume; // 슬라이더 값을 저장된 볼륨으로 설정
        }

        // 불러온 상태에 따라 슬라이더 활성화 상태 업데이트
        UpdateSliderInteractable();
    }
}

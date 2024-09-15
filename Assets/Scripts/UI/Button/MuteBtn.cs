using UnityEngine;
using UnityEngine.UI; // Slider와 Button을 사용하기 위해 추가

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

        // 슬라이더 값 초기화
        if (volumeSlider != null)
        {
            volumeSlider.value = AudioListener.volume; // 슬라이더를 현재 볼륨으로 초기화
        }
        
        // 초기 상태에서 이미지 설정
        UpdateButtonImage();
    }

    // 버튼 클릭 시 호출할 함수
    public void ToggleMute()
    {
        if (isMuted)
        {
            // 음소거 해제
            AudioListener.volume = 1f;
            volumeSlider.value = 1f; // 슬라이더 값도 1로 설정
        }
        else
        {
            // 음소거 처리
            AudioListener.volume = 0f;
            volumeSlider.value = 0f; // 슬라이더 값도 0으로 설정
        }

        isMuted = !isMuted; // 음소거 상태 토글
        UpdateButtonImage(); // 버튼 이미지 업데이트
    }

    // 버튼 이미지를 음소거 상태에 따라 업데이트하는 함수
    private void UpdateButtonImage()
    {
        if (buttonImage != null)
        {
            buttonImage.sprite = isMuted ? mutedImage : unmutedImage;
        }
    }
}

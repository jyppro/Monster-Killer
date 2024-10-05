using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public float lifeTime = 1.5f;  // 데미지 텍스트가 유지될 시간

    void Start()
    {
        Destroy(gameObject, lifeTime);  // lifeTime 후에 데미지 텍스트 삭제
    }
}


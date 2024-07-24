using UnityEngine;

public static class PlayerPrefsX
{
    // 배열을 저장하는 메서드
    public static void SetIntArray(string key, int[] values)
    {
        // 배열의 길이를 저장
        PlayerPrefs.SetInt(key + "_Length", values.Length);

        // 각 배열의 요소를 저장
        for (int i = 0; i < values.Length; i++)
        {
            PlayerPrefs.SetInt(key + "_" + i, values[i]);
        }
    }

    // 배열을 불러오는 메서드
    public static int[] GetIntArray(string key, int[] defaultValues)
    {
        int length = PlayerPrefs.GetInt(key + "_Length", defaultValues.Length);

        int[] values = new int[length];
        for (int i = 0; i < length; i++)
        {
            values[i] = PlayerPrefs.GetInt(key + "_" + i, defaultValues[i]);
        }

        return values;
    }
}

/*
파이어베이스 데이터베이스의 자바스크립트 라이블러리와 연결
*/
using System.Runtime.InteropServices;

namespace FirebaseWebGL.Scripts.FirebaseBridge
{
    public static class FirebaseDatabase
    {
        [DllImport("__Internal")]
        public static extern void SaveJSON(int playerID, int rank, int power, int gold, float time, string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void LoadJSON(int playerID, string path, string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void PostJSON(string path, string value, string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void GetJSON(string path, string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void PushJSON(string path, string value, string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void UpdateJSON(string path, string value, string objectName, string callback, string fallback);

        [DllImport("__Internal")]
        public static extern void DeleteJSON(string path, string objectName, string callback, string fallback);
        
        [DllImport("__Internal")]
        public static extern void ListenForValueChanged(string path, string objectName, string onValueChanged, string fallback);

        [DllImport("__Internal")]
        public static extern void StopListeningForValueChanged(string path, string objectName, string callback, string fallback);
    }
}
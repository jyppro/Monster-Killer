/*
파이어베이스 데이터베이스의 자바스크립트 라이블러리와 연결
*/
using System.Runtime.InteropServices;

namespace FirebaseWebGL.Scripts.FirebaseBridge
{
    public static class FirebaseDatabase
    {
        [DllImport("__Internal")]
        public static extern void LoadGameData(int playerID, string objectName, string callback, string fallback);
        
        [DllImport("__Internal")]
        public static extern void SaveGameData(int playerID, 
        int rank, 
        int power, 
        int maxHP, 
        int currentHP, 
        int gold, 
        int sumScore,
        float time, 
        string objectName, string callback, string fallback);
    }
}
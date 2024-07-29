/*
파이어베이스 데이터베이스 오류 출력
*/
using System;

namespace FirebaseWebGL.Scripts.Objects
{
    [Serializable]
    public class FirebaseError
    {
        public string code;
        public string message;
        public string details;
    }
}
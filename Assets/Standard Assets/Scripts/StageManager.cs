using UnityEngine;

public class StageManager : MonoBehaviour
{
    void Update() { GameManager.Instance.GetStage(); }
}

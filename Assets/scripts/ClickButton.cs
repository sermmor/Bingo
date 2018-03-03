using UnityEngine;

public class ClickButton : MonoBehaviour
{
    void OnMouseDown()
    {
        GameMNG.Instance.StartGame();
    }
}

using UnityEngine;
using Commons;

public class TitleManager : MonoBehaviour
{
    void Update()
    {
        UnityCommonsFunc.EndGame();
    }

    // OnApplicationQuitが呼ばれた際の処理
    void OnApplicationQuit()
    {
        Application.Quit(); // アプリを終了する
    }
}
using UnityEngine;
using Commons;

public class TitleManager : MonoBehaviour
{
    void Update()
    {
        UnityCommonsFunc.EndGame();
    }

    void OnApplicationQuit()
    {
        Application.Quit();
    }
}
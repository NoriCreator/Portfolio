using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShot_Fanction : MonoBehaviour
{
    // 開始時スクリーンショットを保存
    void Start()
    {
        ScreenCapture.CaptureScreenshot ("Assets/Images/Image.jpg");
    }

    int i = 0; // 画像ファイルのナンバー指定
    private void Update()
    {
        // スペースキーが押されたら
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // スクリーンショットを保存
            CaptureScreenShot("Assets/Images/ScreenShot(" + i + ").jpg");
            i = i + 1;
        }
    }

    // 画面全体のスクリーンショットを保存する
    private void CaptureScreenShot(string filePath)
    {
        ScreenCapture.CaptureScreenshot(filePath);
    }
}

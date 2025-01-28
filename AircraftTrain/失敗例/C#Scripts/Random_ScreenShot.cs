using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Random_ScreenShot : MonoBehaviour
{
    public int i = 0; // 画像ファイルのナンバー指定

    private void Update()
    {
        
        // スクリーンショットを保存
        CaptureScreenShot("Assets/Images/ScreenShot(" + i + ").jpg");
        
        i = i + 1; // 画像ファイルのナンバーを変更

        // スペースキーが押されたら
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("画像ファイルが" + (i + 1)  + "枚生成されました");
            UnityEditor.EditorApplication.isPlaying = false; // 強制終了
        }
    }

    // 画面全体のスクリーンショットを保存する
    private void CaptureScreenShot(string filePath)
    {
        ScreenCapture.CaptureScreenshot(filePath);
    }
}

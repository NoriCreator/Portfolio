using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back_Color_tkrw19 : MonoBehaviour
{
    // グローバル変数iの値をTakeoff_Rw19スクリプトから取得
    Takeoff_Rw19 Takeoff_Rw19;

    // 背景色一覧
    List<List<byte>> ColorList = new List<List<byte>>(){
        new List<byte>(){11, 28, 39},
        new List<byte>(){22, 57, 79},
        new List<byte>(){33, 86, 119},
        new List<byte>(){44, 115, 159},
        new List<byte>(){56, 144, 198},
        new List<byte>(){95, 166, 210},
        new List<byte>(){137, 189, 222},
        new List<byte>(){175, 210, 232},
        new List<byte>(){215, 232, 243},
        new List<byte>(){255, 255, 255},
        new List<byte>(){11, 28, 39}
    };

    void Update()
    {
        GameObject obj = GameObject.Find("Aircraft_Object");
        Takeoff_Rw19 = obj.GetComponent<Takeoff_Rw19>();
        int ColorNum = Takeoff_Rw19.num;

        // 背景色を指定
        GetComponent<UnityEngine.Camera>().backgroundColor = new Color32(ColorList[ColorNum][0], ColorList[ColorNum][1], ColorList[ColorNum][2], 100);
    }
}

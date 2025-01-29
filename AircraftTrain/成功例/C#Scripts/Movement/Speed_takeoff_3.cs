using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speed_chakuriku : MonoBehaviour
{
    // 航空機の速度(移動ベクトル)を指定
    Vector3 speed = new Vector3(0f, 5.24f, 100f);

    void Update()
    {
        // 1秒毎の航空機の座標を変更
        this.transform.position += speed * Time.deltaTime;
    }
}

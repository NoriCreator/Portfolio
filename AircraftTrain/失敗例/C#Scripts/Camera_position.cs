using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_position : MonoBehaviour
{
    // 航空機の情報を取得
    public GameObject Aircraft;

    void Update()
    {
        // 航空機の座標を取得
        Vector3 AircraftPos = Aircraft.transform.position;
        
        // カメラの位置をランダムに変更
        transform.position = new Vector3(Random.Range(-10f, 10f), -15, Random.Range(-10f, 10f));
        // カメラから飛行機の方向ベクトルを取得
        Vector3 direction = AircraftPos - this.transform.position;
        // オブジェクトをベクトル方向に従って回転させる
        transform.rotation = Quaternion.LookRotation(direction);

    }
}

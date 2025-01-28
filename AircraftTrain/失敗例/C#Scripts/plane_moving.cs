using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plane_moving : MonoBehaviour
{
    void Update()
    {
        // 航空機を前進移動
        transform.position += new Vector3(0, 0, 15) * Time.deltaTime;
    }
}

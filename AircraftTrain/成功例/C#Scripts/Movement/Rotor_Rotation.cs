using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotor_Rotation : MonoBehaviour
{
    public float Angle = 0f;
    void Update()
    {
        while(Angle >= -1500f)
        {
            Angle -= 4f * Time.deltaTime;
        }

        this.transform.Rotate ( 0, Angle, 0 );
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static Vector3 AimPos;

    private Vector3 curPos;

    private Transform myT;

    void Start()
    {
        myT = transform;
        curPos = myT.position;
        AimPos = myT.position;
    }

    void FixedUpdate()
    {
        curPos = Vector3.Lerp(curPos, AimPos, Time.fixedDeltaTime * 5);
        myT.position = curPos;
    }
}

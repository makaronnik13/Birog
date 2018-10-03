using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
    private static float _speed = 3f;

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Camera.main.transform.position - transform.position), Time.deltaTime/_speed);
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.up,
            Camera.main.transform.rotation * Vector3.up);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    void Start()
    {
        var cam = FindObjectOfType<Camera>();
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, -10.0f);
    }
}

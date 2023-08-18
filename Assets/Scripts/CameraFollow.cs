using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private Player target;

    private void LateUpdate()
    {
        transform.position = target.transform.position + offset;
    }
}

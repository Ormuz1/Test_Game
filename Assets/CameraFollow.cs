using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform objectToFollow;
    [SerializeField] private float smoothing;
    private Vector3 velocity;
    [SerializeField] private Vector3 offset;
    private void LateUpdate() {
        Vector3 newPosition = Vector3.SmoothDamp(transform.position, objectToFollow.position + offset, ref velocity, smoothing);
        transform.position = newPosition;
    }   
}

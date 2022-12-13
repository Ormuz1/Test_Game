using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearPathNode : MonoBehaviour
{
    public Vector3 Position {get => thisTransform.position; }
    public Quaternion Rotation {get => thisTransform.rotation; }
    private Transform thisTransform;

    private void Awake() {
        thisTransform = transform;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;

    public bool useStratOffset = true;

    public Vector3 offset;

    [Range(0f, 1f)]
    public float rotationLerp = 0.1f;

    public float minRotationOffsetToStartLerp = 3f;

    void Start()
    {
        if (useStratOffset)
        {
            offset = gameObject.transform.position;
        }
    }

    void Update()
    {
        transform.position = target.transform.rotation * offset + target.gameObject.transform.position;
        //Vector3 targetRotation = target.transform.rotation.eulerAngles;
        //Vector3 currentRotation = transform.rotation.eulerAngles;

        //if (Mathf.Abs(targetRotation.y - currentRotation.y) > minRotationOffsetToStartLerp)
        //{
        //    transform.rotation = Quaternion.Euler(
        //        new Vector3(currentRotation.x, Mathf.Lerp(currentRotation.y, targetRotation.y, rotationLerp), currentRotation.z));
        //}

        transform.rotation = Quaternion.Lerp(transform.rotation, target.transform.rotation, rotationLerp);
    }
}

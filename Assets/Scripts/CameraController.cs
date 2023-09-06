using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraStyle {Fixed, Free}

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public CameraStyle cameraStyle;
    public Transform pivot;
    public float rotationSpeed = 1f;

    private Vector3 offset;
    private Vector3 pivotOffset;

    // Start is called before the first frame update
    void Start()
    {
        // offset of the pivot from player
        pivotOffset = pivot.position -player.transform.position;
        // set the offset of the camera based on the player
        offset = transform.position - player.transform.position;
    }

    private void LateUpdate()
    {
        // if using fixed camera mode
        if (cameraStyle == CameraStyle.Fixed)
        {
            // set camera position to players postion plus offset
            transform.position = player.transform.position + offset;
        }

        // if using free camera mode
        if (cameraStyle == CameraStyle.Free)
        {
            // make pivot position follow player
            pivot.transform.position = player.transform.position + pivotOffset;
            //work out angle from mouse input as a quaternion
            Quaternion turnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse Y") * rotationSpeed, Vector3.up);
            //modify offsetby the turn angle
            offset = turnAngle * offset;
            // set camera position to pivot plus offset
            transform.position = pivot.transform.position + offset;
            // set camera look at pivot
            transform.LookAt(pivot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get the cameras transform position to get that of the players transform position
        transform.position = player.transform.position + offset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    
    public Transform target; //target is the object we want the camera to look at, make sure to set this in the inspector!

    public float smoothSpeed = 0.125f; //smoothSpeed adjusts the movement of the camera so it doesn't snap

    public Vector3 offset;//this is the amount the camera is offset from the target

    /*Explanation
     * Even though we're working within a 2D scene, the camera is offset and exists within 3D space, so we use Vector3
     * A Lerp is "linear interpolation", which finds values between two designated points. 
     * This is helpful for camera movement, where we want the camera to move smoothly between two points.
     */

    // Update is called once per frame
    void Update()
    {
        //Work out where the camera should be
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        //move the camera to that location
        transform.position = smoothedPosition;

        //Ensure the camera is still looking at the target
        //We need this because the Lerp and smooth mean the camera will sometimes be slightly behind the character movement.
        transform.LookAt(target);
    }
}

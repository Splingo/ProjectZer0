using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour
{
    public Camera mainCamera; //reference to main camera
    public float lerpSpeed = 5.0f; // speed of movement (interpolated, see comments below)

    private bool toRight = true; // toggle for right-/leftside movement

    // a coroutine is a method by unity to handle actions over multiple frames (movement, animation, etc.)
    // for this script to work properly the current Coroutine must be tracked (and stoped before another coroutine starts)
    private Coroutine currentLerp;

    public RectTransform arrowRectTransform;

    public AudioSource audioSourceRightClick;
    public AudioSource audioSourceLeftClick;

    public void SnapCameraToRight()
    {
        Vector3 newCameraPosition; // vector for the NEXT camera position 
        
        if (toRight) // move camera to right
        {
            newCameraPosition = new Vector3(38.0f, -5.0f, -10f);
            arrowRectTransform.localRotation = Quaternion.Euler(0f, 0f, 180f);
            toRight = false;
            audioSourceRightClick.Play();
        }
        else // move camera to left
        {
            newCameraPosition = new Vector3(0f, -5.0f, -10f);
            arrowRectTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            toRight = true;
            audioSourceLeftClick.Play();
        }

        if (currentLerp != null) // if first Coroutine you skip this
        {
            // stop the current Coroutine if it's running
            StopCoroutine(currentLerp);
        }

        // set currentLerp to current Coroutine
        currentLerp = StartCoroutine(LerpToPosition(newCameraPosition));
    }
    
    // interpolation algorithm for the camera movement, while loop tracks the distance and changes speed of movement for a smooth transition
    private IEnumerator LerpToPosition(Vector3 newCameraPosition)
    {
        float journeyLength = Vector3.Distance(mainCamera.transform.position, newCameraPosition);
        float startTime = Time.time;

        while (Time.time < startTime + journeyLength / lerpSpeed)
        {
            float distanceCovered = (Time.time - startTime) * lerpSpeed;
            float fractionOfJourney = distanceCovered / journeyLength;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, newCameraPosition, fractionOfJourney);
            yield return null;
        }
        mainCamera.transform.position = newCameraPosition; // final camera position
    }
}

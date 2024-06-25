using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveScript : MonoBehaviour
{
    public Camera mainCamera; //reference to main camera
    public float positionLerpSpeed = 0.25f; // speed of movement (interpolated, see comments below)
    public float zoomLerpSpeed = 0.25f; // speed of zoom
    public float zoomLevel = 10.0f; // zoom level (how far it zooms in)
    private bool toRight = true; // toggle for right-/leftside movement
    private bool zoomedIn = false; // track zoomin/zoomout

    private bool enabled = true;

    // a coroutine is a method by unity to handle actions over multiple frames (movement, animation, etc.)
    // for this script to work properly the current Coroutine must be tracked (and stoped before another coroutine starts)
    private Coroutine currentLerp;
    public RectTransform arrowRectTransform; // reference to UI arrow
    public AudioSource audioSourceRightClick;
    public AudioSource audioSourceLeftClick;

    public void SnapCameraToRight()
    {
        if (!enabled)
            return;

        Vector3 newCameraPosition; // vector for the NEXT camera position 

        if (toRight) // move camera to right
        {
            newCameraPosition = new Vector3(8.5f, -0.1f, -10f);
            arrowRectTransform.localRotation = Quaternion.Euler(0f, 0f, 180f);
            toRight = false;
            audioSourceRightClick.Play();
        }
        else // move camera to left
        {
            newCameraPosition = new Vector3(-10.6f, -0.1f, -10f);
            arrowRectTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            toRight = true;
            audioSourceLeftClick.Play();
        }

        if (currentLerp != null) // if first Coroutine you skip this
        {
            // stop the current Coroutine if it's running
            StopCoroutine(currentLerp);
        }

        if (zoomedIn)
        {
            currentLerp = StartCoroutine(LerpToPosition(newCameraPosition, 7.5f, positionLerpSpeed, zoomLerpSpeed));
            zoomedIn = false;
        }
        else
        {
            // if not zoomed in, zoom in
            currentLerp = StartCoroutine(LerpToPosition(newCameraPosition, zoomLevel, positionLerpSpeed, zoomLerpSpeed));
            zoomedIn = true;
        }
    }

    // algorithm for smooth animation / movement
    private IEnumerator LerpToPosition(Vector3 targetPosition, float targetZoom, float positionSpeed, float zoomSpeed)
    {
        float startTime = Time.time;

        Vector3 initialPosition = mainCamera.transform.position;
        float initialZoom = mainCamera.orthographicSize;

        while (Time.time < startTime + positionSpeed)
        {
            float t_pos = (Time.time - startTime) / positionSpeed;
            float t_zoom = (Time.time - startTime) / zoomSpeed;

            // calculate separate t values for position and zoom
            float positionT = Mathf.SmoothStep(0f, 1f, t_pos);
            float zoomT = Mathf.SmoothStep(0f, 1f, t_zoom);

            // lerp for position
            mainCamera.transform.position = Vector3.Lerp(initialPosition, targetPosition, positionT);

            // lerp for zoom
            mainCamera.orthographicSize = Mathf.Lerp(initialZoom, targetZoom, zoomT);

            yield return null;
        }
        // Ensure the camera is at the exact target position and zoom level
        mainCamera.transform.position = targetPosition;
        mainCamera.orthographicSize = targetZoom;
    }

    public void setEnabled(bool enabled)
    {
        this.enabled = enabled;
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class friendly_angel : BaseUnit_Script
{
    private float healRange = 1.2f;
    private float healAmount = 2f;
    private new bool waiting = false;

    private GameObject rangeCircle;
    private LineRenderer lineRenderer;

    private new void Start()
    {
        // call Start in BaseUnit class
        base.Start();

        // Create a new GameObject for the healing circle and configure it
        rangeCircle = new GameObject("RangeCircle");
        rangeCircle.transform.SetParent(transform);
        rangeCircle.transform.localPosition = Vector3.zero;

        // Add and configure the LineRenderer component
        lineRenderer = rangeCircle.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 100; // Number of points in the circle
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.loop = true;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = new Color(0, 1, 0, 0.7f); // Semi-transparent green
        lineRenderer.endColor = new Color(0, 1, 0, 0.7f); // Semi-transparent green
        lineRenderer.sortingLayerName = "New Layer 3";
        lineRenderer.useWorldSpace = false;
        DrawHealingRadius(lineRenderer);
        StartCoroutine(FadeOutLineRenderer(rangeCircle.GetComponent<LineRenderer>(), 4.0f));
    }
    private void Update()
    {
        if (transform.hasChanged)
        {
            SetOccupiedCells(); // Wenn sich die Position geändert hat, rufe die Funktion auf, um den Layer zu aktualisieren
            transform.hasChanged = false; // Setze transform.hasChanged zurück, um weitere Änderungen zu erkennen
        }
        if (IsAnyFriendlyInRange())
        {
            if (waiting == false)
            {
                StartCoroutine(HealWithDelay());
                waiting = true;
            }
        }
    }

    IEnumerator HealWithDelay()
    {
        DrawHealingRadius(lineRenderer);
        StartCoroutine(FadeOutLineRenderer(lineRenderer, 0.5f));
        HealNearbyUnits();
        yield return new WaitForSeconds(attackSpeed);
        waiting = false;
    }

    private void HealNearbyUnits()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, healRange);

        foreach (Collider2D collider in colliders)
        {
            // Check if the collider is a friendly unit with the correct tag
            if (collider.CompareTag("FriendlyUnit") && collider.GetComponent<friendly_angel>() == null)
            {
                BaseUnit_Script friendlyUnitScript = collider.GetComponent<BaseUnit_Script>();
                if (friendlyUnitScript != null)
                {
                    friendlyUnitScript.Heal(healAmount);
                }
            }
        }
    }

    protected bool IsAnyFriendlyInRange()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, healRange);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("FriendlyUnit") && collider.GetComponent<friendly_angel>() == null)
            {
                return true;
            }
        }
        return false;
    }


    new public void TakeDamage(float damage)
    {
        currentHP -= damage;

        // Update the HP bar
        UpdateHPBar();

        if (currentHP <= 0f)
        {
            // Implement logic for enemy death
            Destroy(gameObject);
        }
    }

    private void DrawHealingRadius(LineRenderer lineRenderer)
    {
        float deltaTheta = (2f * Mathf.PI) / lineRenderer.positionCount;
        float theta = 0f;

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            float x = healRange * Mathf.Cos(theta);
            float y = healRange * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, y, 0);
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }

        // Reset opacity
        lineRenderer.startColor = new Color(0, 1, 0, 0.1f);
        lineRenderer.endColor = new Color(0, 1, 0, 0.1f);
    }


    IEnumerator FadeOutLineRenderer(LineRenderer lineRenderer, float fadeDuration)
    {
        float elapsedTime = 0f;
        Color startColor = lineRenderer.startColor;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            Color currentColor = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            lineRenderer.startColor = currentColor;
            lineRenderer.endColor = currentColor;
            yield return null;
        }

        lineRenderer.startColor = endColor;
        lineRenderer.endColor = endColor;
    }

}

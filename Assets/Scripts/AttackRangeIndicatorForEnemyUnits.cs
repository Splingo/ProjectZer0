using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeIndicatorForEnemyUnits : MonoBehaviour
{
    public GameObject indicatorPrefab; // Prefab of the indicator circle
    private GameObject indicator; // Reference to the instantiated indicator
    private Enemy enemyScript; // Reference to the enemy script

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate the indicator and set it inactive initially
        indicator = Instantiate(indicatorPrefab, transform.position, Quaternion.identity);
        indicator.SetActive(false);

        // Get the enemy script
        enemyScript = GetComponent<Enemy>();
        if (enemyScript == null)
        {
            Debug.LogError("Enemy script not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update the indicator position
        indicator.transform.position = transform.position;

        // Show or hide the indicator based on mouse interaction
        HandleMouseInteraction();
    }

    private void HandleMouseInteraction()
    {
        // Raycast to detect mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        // Check if the hit collider is the same as the collider of this unit
        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            ShowIndicator(); // Show the indicator when hovering over the enemy
        }
        else
        {
            HideIndicator(); // Hide the indicator when not hovering over the enemy
        }
    }

    private void SetIndicatorSize()
{
    // Get the attack range from the enemy's script
    float attackRange = enemyScript.attackRange;

    // Set the size of the indicator based on the attack range
    Vector3 scale = new Vector3(attackRange * 2, attackRange * 2, 1);

    // Set the position of the indicator to match the enemy unit's position
    indicator.transform.position = new Vector3(transform.position.x, transform.position.y, indicator.transform.position.z);
    indicator.transform.localScale = scale;
}

    public void ShowIndicator()
    {
        SetIndicatorSize(); // Update the indicator size
        indicator.SetActive(true);
    }

    public void HideIndicator()
    {
        indicator.SetActive(false);
    }
}
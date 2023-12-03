using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRangeIndicatorForFriendlyUnits : MonoBehaviour
{
    public GameObject indicatorPrefab; // Prefab of the indicator circle
    private GameObject indicator; // Reference to the instantiated indicator
    private BaseUnit_Script unitScript; // Reference to the unit script

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate the indicator and set it inactive initially
        indicator = Instantiate(indicatorPrefab, transform.position, Quaternion.identity);
        indicator.SetActive(false);
        unitScript = GetComponent<BaseUnit_Script>(); // Get the unit script
    }

    // Update is called once per frame
    void Update()
    {
        // Raycast to detect mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        // Check if the hit collider is the same as the collider of this unit
        if (hit.collider != null && hit.collider.gameObject == gameObject)
        {
            SetIndicatorSize();
            indicator.SetActive(true);
        }
        else
        {
            indicator.SetActive(false);
        }
    }

    private void SetIndicatorSize()
    {
        // Get the attack range from the unit's script
        float attackRange = unitScript.attackRange;

        // Set the size of the indicator based on the attack range
        Vector3 scale = new Vector3(attackRange, attackRange, 1);
        indicator.transform.localScale = scale;
    }
}
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Transform zone1; // Die Zone, in der sich das Objekt nicht befinden soll, um einen Klon zu erstellen
    public Transform zone2; // Die Zielzone, in die das Objekt platziert werden kann

    private bool isDragging = false;
    private Vector3 offset;
    private GameObject clone;
    private Vector3 initialPosition;

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        initialPosition = transform.position;
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        if (!IsInZone(zone1)) 
        {
            CreateClone();
        }
        if(!IsInZone(zone2)){
            Destroy(clone);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;

        if (clone != null)
        {
            if (IsInZone(zone2)) 
            {
                if (IsAlreadyOccupied(zone2))
                {
                    Destroy(clone);
                }
                else
                {
                    transform.position = initialPosition;
                }
            }
            else
            {
                Destroy(clone);
                transform.position = initialPosition;
            }
        }
    }

    void Update()
    {
        if (isDragging && clone != null)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            clone.transform.position = new Vector3(newPosition.x, newPosition.y, transform.position.z);
        }
    }

    void CreateClone()
    {
        clone = Instantiate(gameObject, transform.parent);
        clone.GetComponent<CanvasGroup>().blocksRaycasts = false;
        clone.GetComponent<DragAndDrop>().enabled = false;
    }

    bool IsInZone(Transform zone)
    {
        if (zone != null && clone != null)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(zone.position, zone.localScale, 0);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject == gameObject || (clone != null && collider.gameObject == clone))
                {
                    return true;
                }
            }
        }
        return false;
    }

    bool IsAlreadyOccupied(Transform zone)
    {
        if (zone != null)
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(zone.position, zone.localScale, 0);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject != null && collider.gameObject != clone)
                {
                    return true;
                }
            }
        }
        return false;
    }
}

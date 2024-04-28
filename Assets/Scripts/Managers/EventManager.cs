using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    /// <summary>
    /// This event can be used when an enemy has been killed by the player
    /// </summary>
    public static UnityEvent EnemyKilledEvent = new UnityEvent();

    /// <summary>
    /// This event can be used when an enemy has reached the city
    /// </summary>
    public static UnityEvent EnemyDespawnedEvent = new UnityEvent();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    // This class is used to define UnityEvents that we can use to 
    /// <summary>
    /// This event can be used when an enemy has been killed by the player
    /// </summary>
    public static UnityEvent EnemeyKilledEvent = new UnityEvent();

    /// <summary>
    /// This event can be used when an enemy has reached the city
    /// </summary>
    public static UnityEvent EnemyDiedEvent = new UnityEvent();
}

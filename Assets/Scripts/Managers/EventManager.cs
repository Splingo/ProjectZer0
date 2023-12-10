using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static UnityEvent EnemeyKilledEvent = new UnityEvent();
    public static UnityEvent EnemyDiedEvent = new UnityEvent();
}

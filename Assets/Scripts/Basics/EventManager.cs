using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action PlayerDied;
    public static Action PlayerCreated;
    public static Action PlayerFighting;
    public static Action PlayerRevived;
    public static Action<object[]> PlayerAttacked;
    public static Action PlayerAttacking;

    public static Action<Unit> UnitDied;

    public static Action GameStarted;
    public static Action GameEnded;
    public static Action GameDefeat;
    public static Action GamePaused;

    public static Action<GameObject> SoundPlayed;
    public static Action<GameObject> SoundPlaying;
}

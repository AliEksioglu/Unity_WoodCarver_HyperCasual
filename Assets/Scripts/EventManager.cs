using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
public static class EventManager
{
    //Start
    public static event Action OnStartLevel;
    public static void Event_OnStartLevel() { OnStartLevel?.Invoke(); }

    //Finish
    public static event Action OnLevelFinish;
    public static void Event_OnLevelFinish() { OnLevelFinish?.Invoke(); }

    public static event Action FinishFirstTouch;
    public static void Event_FinishFirstTouch() { FinishFirstTouch?.Invoke(); }

    //Mekanik
    public static event Action<WoodScript> OnWoodAdded;
    public static void Event_OnWoodAdded(WoodScript wood) { OnWoodAdded?.Invoke(wood); }

    //Score
    public static event Action<int> OnIncreaseScore;
    public static void Event_OnIncreaseScore(int value) { OnIncreaseScore?.Invoke(value); }

    public static event Action<int> OnLastScore;
    public static void Event_OnLastScore(int puan) { OnLastScore?.Invoke(puan); }

    public static event Action<int> OnRestScore;
    public static void Event_OnRestScore(int puan) { OnRestScore?.Invoke(puan); }
}

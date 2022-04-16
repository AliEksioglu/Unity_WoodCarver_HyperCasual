using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyOther : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        EventManager.Event_OnIncreaseScore(other.GetComponent<WoodScript>().WoodPuan);
        Destroy(other.gameObject,0.05f);
    }
}

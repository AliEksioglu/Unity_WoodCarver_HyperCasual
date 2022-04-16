using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{

    private Collider myCollider;

    void Start()
    {
        myCollider = transform.GetComponent<Collider>();
    }



}

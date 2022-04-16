using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PoolingObje : MonoBehaviour
{
    [SerializeField] string name;

    void Start()
    {
        Destroy(this.gameObject,1f);
        transform.DOScale(0, 5f);
        //Invoke("GeriDondur", 1f);
    }

    void GeriDondur()
    {
        ObjectifPool.singleton.ReturnModel(name, this.gameObject);
    }

}

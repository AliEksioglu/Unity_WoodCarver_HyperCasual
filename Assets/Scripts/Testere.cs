using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Testere : MonoBehaviour
{
    [SerializeField] Transform _modelTestere;
    [SerializeField] Transform move;
    [SerializeField] Vector3 move2;

    private void Start()
    {
        _modelTestere.DOLocalMove(move.localPosition, 0.5f).SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        transform.Rotate(move2,Space.Self);
    }

}

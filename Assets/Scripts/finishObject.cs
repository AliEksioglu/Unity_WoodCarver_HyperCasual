using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class finishObject : MonoBehaviour
{
    [SerializeField] ToplanmaYeri toplanmaYeri;
    [SerializeField] PlayerSetting settings;
    Transform ghost;
    Transform buildObejct;
    float diveded;

    private void Start()
    {
        ghost = transform.GetChild(0).GetChild(settings.howManyObjectsOpend).transform;
        buildObejct = transform.GetChild(1).GetChild(settings.howManyObjectsOpend).transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(Layers.collectWood))
        {
            //switch (other.gameObject.GetComponent<WoodScript>().tagIndex)
            //{
            //    case 1:
            //        diveded = 10;
            //        break;
            //    case 2:
            //        diveded = 9;
            //        break;
            //    case 3:
            //        diveded = 5;
            //        break;
            //    case 4:
            //        diveded = 6;
            //        break;
            //    case 5:
            //        diveded = 5;
            //        break;
            //}
            Destroy(other.gameObject);

            if (ghost.localScale.x >buildObejct.localScale.x)
            {
                buildObejct.DOScaleX(buildObejct.localScale.x + ghost.localScale.x / 5, 0.1f);
                //if (ghost.localScale.x <= buildObejct.localScale.x)
                //{
                //    buildObejct.localScale = ghost.localScale;
                //    toplanmaYeri.ObjectControl();
                //}

                //if (ghost.localScale.x > buildObejct.localScale.x)
                //{
                //    toplanmaYeri.NotFinished(buildObejct.localScale.x,"x");
                //}
            }

            else if (ghost.localScale.y > buildObejct.localScale.y)
            {
                buildObejct.DOScaleY(buildObejct.localScale.y + ghost.localScale.y / 5, 0.1f);
                //if (ghost.localScale.y <= buildObejct.localScale.y)
                //{
                //    buildObejct.localScale = ghost.localScale;
                //    toplanmaYeri.ObjectControl();
                //}
                //if (ghost.localScale.y > buildObejct.localScale.y)
                //{
                //    toplanmaYeri.NotFinished(buildObejct.localScale.y, "y");
                //}
            }

            else if (ghost.localScale.z > buildObejct.localScale.z)
            {
                buildObejct.DOScaleZ(buildObejct.localScale.z + ghost.localScale.z / 5, 0.1f);
                //if (ghost.localScale.z <= buildObejct.localScale.z)
                //{
                //    buildObejct.localScale = ghost.localScale;
                //    toplanmaYeri.ObjectControl();
                //}
                //else if (ghost.localScale.z > buildObejct.localScale.z)
                //{
                //    toplanmaYeri.NotFinished(buildObejct.localScale.z, "z");
                //}
            }
            toplanmaYeri.ObjectControl(ghost,buildObejct);

        }
    }
}

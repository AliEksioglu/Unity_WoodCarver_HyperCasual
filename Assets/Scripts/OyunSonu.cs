using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

public class OyunSonu : MonoBehaviour
{
    Models models;
    [SerializeField] PlayerSetting settings;
    List<GameObject> ghosts = new List<GameObject>();
    [SerializeField] List<List<ModelParts>> oyunSonu = new List<List<ModelParts>>();
    Vector3 firstPoint;
    Vector3 firstScale;
    [SerializeField] Transform pointToGo;
    [SerializeField] GameObject puff;

    public string matName;
    public bool startMove
    {
        set
        {
            _startMove = value;
            if (value)
            {
                MoveToPoint();
            }
            else GetBack();
        }
        get { return _startMove; }
    }
    bool _startMove;


    void Start()
    {
        //Invoke("StartSetup", 0.5f);
        models = GameObject.Find("Modeller").GetComponent<Models>();
        ObjectToBuild();
        StartSetup();
        if (settings.index == 0 && settings.howManyObjectsOpend != 0)
        {
            models.modelParts[0].buildObje.transform.GetChild(1).GetChild(settings.howManyObjectsOpend - 1).gameObject.SetActive(true);
            models.modelParts[0].buildObje.transform.GetChild(0).GetChild(settings.howManyObjectsOpend - 1).gameObject.SetActive(false);
        }

    }


    public GameObject ObjectToBuild()
    {
        GameObject obje = models.modelParts[settings.index].buildObje;
        return obje;
    }

    void MoveToPoint()
    {
        firstPoint = ObjectToBuild().transform.position;
        firstScale = ObjectToBuild().transform.localScale;
        ObjectToBuild().transform.DOMove(pointToGo.position, 0.5f);
        if (settings.index == 1)
        {
            ObjectToBuild().transform.DORotate(Vector3.zero, 0.5f);
        }

        ObjectToBuild().transform.DOScale(Vector3.one * 2, 0.5f);
    }

    void GetBack()
    {
        ObjectToBuild().transform.DOMove(firstPoint, 0.5f).OnComplete(ComplateObject);
        if (settings.index == 1)
        {
            ObjectToBuild().transform.DORotate(Vector3.up * 90, 0.5f).OnComplete(ComplateObject);

        }
        ObjectToBuild().transform.DOScale(firstScale, 0.5f);

    }

    void ComplateObject()
    {
        if (settings.howManyObjectsOpend == 3)
        {
            ObjectToBuild().transform.DOPunchScale(ObjectToBuild().transform.localScale * 1.5f, 0.5f, 1).OnComplete(OnComplite);
            //Instantiate(puff, ObjectToBuild().transform.position, Quaternion.identity);
            settings.howManyObjectsOpend = 0;
            settings.index++;
        }
        else InvokeFinish();
    }

    void OnComplite()
    {
        InvokeFinish();
    }

    void InvokeFinish()
    {
        Invoke("FinishScren", 1.5f);
    }
    void FinishScren()
    {
        EventManager.Event_OnLevelFinish();
    }

    void StartSetup()
    {
        for (int i = 0; i <= settings.index; i++)
        {
            GameObject obje = models.modelParts[i].buildObje;
            if (settings.index != i)
            {
                for (int j = 0; j < obje.transform.GetChild(0).childCount; j++)
                {
                    // string name = i.ToString() + "." + j.ToString() + ".mat";
                    obje.transform.GetChild(1).GetChild(j).localScale = obje.transform.GetChild(0).GetChild(j).localScale;
                    obje.transform.GetChild(1).GetChild(j).gameObject.SetActive(true);
                    //obje.transform.GetChild(1).GetChild(j).GetComponent<Renderer>().material = Resources.Load<Material>("Assets/InGameMaterial/" + name);
                }
                obje.transform.GetChild(0).gameObject.SetActive(false);
            }

            else
            {
                for (int j = 0; j <= settings.howManyObjectsOpend; j++)
                {
                    if (j == settings.howManyObjectsOpend)
                    {
                        Vector3 objeScale = obje.transform.GetChild(1).GetChild(j).localScale;

                        if (settings.axis == "x")
                        {
                            objeScale = new Vector3(settings.scaleNumber, objeScale.y, objeScale.z);

                        }

                        else if (settings.axis == "y")
                        {
                            objeScale = new Vector3(objeScale.x, settings.scaleNumber, objeScale.z);

                        }

                        else if (settings.axis == "z")
                        {
                            objeScale = new Vector3(objeScale.x, objeScale.z, settings.scaleNumber);
                        }
                        obje.transform.GetChild(1).GetChild(j).localScale = objeScale;
                        obje.transform.GetChild(1).GetChild(j).gameObject.SetActive(true);
                        //obje.transform.GetChild(0).GetChild(j).gameObject.SetActive(false);

                    }
                    else if (j < settings.howManyObjectsOpend)
                    {
                        obje.transform.GetChild(1).GetChild(j).localScale = obje.transform.GetChild(0).GetChild(j).localScale;
                        obje.transform.GetChild(1).GetChild(j).gameObject.SetActive(true);
                        obje.transform.GetChild(0).GetChild(j).gameObject.SetActive(false);
                    }

                }
            }

        }
    }
}

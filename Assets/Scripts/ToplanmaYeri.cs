using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using Cinemachine;
using UnityEditor;

public class ToplanmaYeri : MonoBehaviour
{
    OyunSonu oyunSonu;
    [SerializeField] UiManager ui;
    [SerializeField] List<Transform> stackTransform = new List<Transform>(); // Kanka buraya parçalarýn stackleneceði tranformlarý atacaz sýralamasý önemli olur düzgün gözükmesi için
    public Transform positionToGo;
    bool start = false;

    public List<List<WoodScript>> oyunSonuObjectList = new List<List<WoodScript>>();

    public Material duplicate;

    bool objectFinished;

    private GameManager manager;
    [SerializeField] private PlayerSetting settings;
    private WoodStack woodStack;
    private bool allowCorutine = false;
    int hit;
    bool odenGeldimi = false;

    void Start()
    {
        oyunSonu = GetComponent<OyunSonu>();

        for (int i = 0; i < Tags.taglar.Length - 1; i++)
        {
            oyunSonuObjectList.Add(new List<WoodScript>());
        }

        hit = 0;

    }

    private void Update()
    {
        if (!settings.isPlaying && allowCorutine)
        {
            //objectsToBuild[0].SetActive(true);
            StartCoroutine(ObjectCreate());

        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(Layers.collectWood))
        {
            ui.objective.transform.parent.gameObject.SetActive(false);
            WoodScript collectWood = other.gameObject.GetComponent<WoodScript>();
            switch (collectWood.tagIndex)
            {
                case 0:
                    collectWood.transporter.woods.Remove(collectWood);
                    other.gameObject.transform.parent = null;
                    other.gameObject.transform.DOMove(new Vector3(transform.position.x + 20, transform.position.y, transform.position.z), 1f);
                    Destroy(other.gameObject, 2);
                    break;
                default:
                    {
                        odenGeldimi = true;
                        int index = collectWood.tagIndex - 1; //eksi olma sebebi tagsindex i 1 olmasý çünkü tree tagý buraya hiç gelmiyor
                        collectWood.transporter.woods.Remove(collectWood);
                        collectWood.transform.parent = null;
                        Vector3 pos = stackTransform[index].position;
                        collectWood.transform.DOMove(new Vector3(pos.x, pos.y + oyunSonuObjectList[index].Count / 2f, pos.z), 0.5f);
                        collectWood.transform.DORotate(new Vector3(0, 0, -90), 0.5f);
                        oyunSonuObjectList[index].Add(collectWood);
                        EventManager.Event_OnLastScore(collectWood.WoodPuan);
                        break;
                    }
            }
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            settings.isPlaying = false;
            if (odenGeldimi)
            {
                StartCoroutine(ObjectCreate());
            }
            else Invoke("finishLeve", 2f);
        }
    }

    IEnumerator ObjectCreate()
    {
        //chooseMat();
        positionToGo = oyunSonu.ObjectToBuild().transform.GetChild(1).transform;
        oyunSonu.startMove = true;
        //if (settings.howManyObjectsOpend == 0)
        //{
        //    settings.howManyObjectsOpend++;
        //}
        yield return new WaitForSeconds(1.5f);
        oyunSonu.ObjectToBuild().transform.GetChild(1).GetChild(settings.howManyObjectsOpend).gameObject.SetActive(true);
        for (int i = oyunSonuObjectList.Count - 1; i > -1; --i)
        {
            for (int j = oyunSonuObjectList[i].Count - 1; j > -1; j--)
            {
                oyunSonuObjectList[i][j].transform.DOMove(positionToGo.position, 0.5f);
                oyunSonuObjectList[i][j].transform.DORotate(new Vector3(0, 0, 90), 0.5f);
                yield return new WaitForSeconds(0.2f);
            }
            oyunSonuObjectList[i].Clear();
        }
        StartCoroutine(FinishMove());
    }

    public void ObjectControl(Transform ghost, Transform build)
    {
        hit++;
        if (hit == 5)
        {
            build.localScale = ghost.localScale;
            start = true;
            oyunSonu.ObjectToBuild().transform.GetChild(0).GetChild(settings.howManyObjectsOpend).gameObject.SetActive(false);
            positionToGo = GameObject.FindGameObjectWithTag("z").transform;
            settings.scaleNumber = 0;
            settings.axis = null;
            settings.howManyObjectsOpend++;
        }



        // kanka 5 tane obje yapýalcak sandalyaye gidince odunlar  canvastaki scora doðru gidiyor  ve sandalyenini geri dönme iþlemini baþlatýyoruz
    }

    public void NotFinished(float scale, string axisName)
    {
        settings.scaleNumber = scale;
        settings.axis = axisName;
    }

    IEnumerator FinishMove()
    {
        yield return new WaitForSeconds(2);
        oyunSonu.startMove = false;
    }



    void chooseMat()
    {
        int index = selectionSort(oyunSonuObjectList);
        Debug.Log(index);
        duplicate = oyunSonuObjectList[index][0].GetComponent<WoodScript>().getChildMat();
    }
    int selectionSort(List<List<WoodScript>> array)
    {
        int max = 0;

        for (int i = 1; i < array.Count; i++)
        {
            if (array[i].Count > array[max].Count)
            {
                max = i;
            }
        }
        return max;
    }
    void finishLeve()
    {
        EventManager.Event_OnLevelFinish();
    }

}

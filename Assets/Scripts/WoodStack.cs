using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using System.Linq;

public class WoodStack : MonoBehaviour
{
    [SerializeField] private PlayerSetting settings;
    [SerializeField] Transform ToplanacakOdunlar;
    [SerializeField] Transform TwoodTakip;
    [SerializeField] string woodTag;
    [SerializeField] Transform StartT;
    [SerializeField] float DistanceObject;
    public float movementDelay = 0.25f;

    public List<WoodScript> woods;
    public UnityEvent OnShake;

    private CharacterMove characterT;


    void Start()
    {
        characterT = transform.GetComponentInParent<CharacterMove>();
        woods = new List<WoodScript>();
        TwoodTakip.position = StartT.position;
    }

    private void Update()
    {
        WoodTakip();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(Layers.obstacle))
        {
            if (woods.Count > 1) DropWood(woods[0]);
            else { AddforcePlayer(); }

            return;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer(Layers.wood))
        {
            EventManager.Event_OnWoodAdded(other.GetComponent<WoodScript>());
        }
    }

    /// <summary>
    /// <param name="obj"> -- Deðdiðimiz odun
    /// <summary>

    float radius = 1;

    void AddWoodList(WoodScript wood)
    {
        wood.transporter = this;
        wood.IdleAnimPlay(false);
        woods.Add(wood);
        wood.gameObject.layer = LayerMask.NameToLayer(Layers.collectWood);
        wood.transform.SetParent(TwoodTakip, true);      //Dünya pozisyonu açýk korunsun
        if (woods.Count == 0)
        {
            wood.transform.localPosition = new Vector3(StartT.position.x, 0, woods.Count * (radius + DistanceObject));
        }
        else
        {
            Vector3 lastPos = woods.Last().transform.position; //Son odunun pozisyonu
            wood.transform.localPosition = new Vector3(lastPos.x, 0, woods.Count * (radius + DistanceObject));
        }

        OnShake.Invoke();
        EventManager.Event_OnIncreaseScore(wood.GetComponent<WoodScript>().WoodPuan);

    }

    float sonOynatmaZaman = Mathf.NegativeInfinity;
    [SerializeField] float animIgnoreTime = 0.09f;
    public void ShakeWood()
    {
        if (settings.isPlaying)
        {
            if (Time.time - sonOynatmaZaman < animIgnoreTime) return;

            float waitTime = 0f;
            for (int index = woods.Count - 1; index > -1; index--)
            {
                woods[index].ShakeProcessStart(waitTime);
                waitTime += 0.05f;
            }
            sonOynatmaZaman = Time.time;
        }

    }

    //IEnumerator waitSeconds(float time, int index)
    //{
    //    yield return new WaitForSeconds(time);
    //    woods[index].GetComponent<WoodScript>().AnimationScaleWood();
    //}
    [SerializeField] public float speed;

    Vector3 takipT; //Odunlarýn Parentenin takip ettiði nokta

    void WoodTakip()
    {
        takipT = StartT.position;
        TwoodTakip.position = new Vector3(0, takipT.y, takipT.z);

        //Vector3 takip = new Vector3(StartT.position.x, 0f, );

        for (int index = 0; index < woods.Count; index++)
        {
            //Vector3 pos = woods[index].transform.localPosition;
            //pos.x = woods[index - 1].transform.localPosition.x;
            //woods[index].transform.DOLocalMove(pos, 0.2f);
            // if(index == 0) woods[index].transform.localPosition = new Vector3(StartT.position.x, 0, radius + DistanceObject);
            //float distance;
            //if (index == 0) distance = StartT.position.x - woods[index].transform.position.x;
            //if (index == 0) woods[index].transform.DOMoveX(StartT.position.x, movementDelay*Time.deltaTime);
            if (index == 0) woods[index].transform.position = new Vector3(Mathf.Lerp(woods[index].transform.position.x, StartT.position.x, 10f *Time.deltaTime), woods[index].transform.position.y, woods[index].transform.position.z);
            // else distance = woods[index - 1].transform.position.x - woods[index].transform.position.x;
            // else woods[index].transform.DOMoveX(woods[index - 1].transform.position.x, movementDelay * Time.deltaTime);
            else woods[index].transform.position = new Vector3(Mathf.Lerp(woods[index].transform.position.x, woods[index - 1].transform.position.x, 0.65f), woods[index].transform.position.y, woods[index].transform.position.z);

            //float direction = Mathf.Sign(distance);
            //float gidilenHiz = direction * Time.deltaTime * speed * Mathf.Abs(distance);

            //if (Mathf.Abs(distance) < Mathf.Abs(gidilenHiz))
            //{
            //    gidilenHiz = distance;
            //}

            //woods[index].transform.position += new Vector3(gidilenHiz, 0f, 0f);

        }

    }

    private void AddforcePlayer()
    {
        //EnableIsPlay(false);
        characterT.MousePosRest();
        characterT.mouseDif = Vector3.zero;
        settings.canMove = false;
        characterT.transform.DOMoveZ(characterT.transform.position.z - 12f, 0.9f, false).OnComplete(canMove);
    }
    public void EnableIsPlay(bool value)
    {
        settings.isPlaying = value;
    }

    public void DropWood(WoodScript wood)
    {
        if (woods[0].gameObject != wood.gameObject)
        {
            if (!GameManager.levelFinish)
            {
                AddforcePlayer();
            }

            int id = wood.GetInstanceID();
            int index = woods.FindIndex(woodS => woodS.GetInstanceID() == id);
            woods.RemoveAt(index);
            wood.DestRoyWood();
            for (int i = woods.Count - 1; i >= index; i--)
            {
                woods[i].gameObject.layer = LayerMask.NameToLayer(Layers.wood);
                woods[i].DropStackList();
                woods[i].transporter = null;
                woods[i].transform.parent = null;
                woods.RemoveAt(i);
            }
            if (!GameManager.levelFinish)
            {
                CollectScoreRest();
            }
        }

    }
    public void CollectScoreRest()
    {
        int newPuan = 0;

        foreach (WoodScript wood in woods)
        {

            newPuan += wood.WoodPuan;
        }
        EventManager.Event_OnRestScore(newPuan);
    }


    // void MoveListElements()
    // {
    //    woods[0].transform.DOMove(gameObject.transform.position, movementDelay);
    //    for (int i = 1; i < woods.Count; i++)
    //    {
    //        Vector3 pos = woods[i].transform.position;
    //        pos.x = woods[i - 1].transform.position.x;
    //        woods[i].transform.DOMove(pos, movementDelay);
    //    }
    // }

    //private void MoveOrigin()
    //{
    //    for (int i = 1; i < woods.Count; i++)
    //    {
    //        Vector3 pos = woods[i].transform.localPosition;
    //        pos.x = woods[0].transform.localPosition.x;
    //        woods[i].transform.DOLocalMove(pos, movementDelay);
    //    }
    //}

    private void OnEnable()
    {
        EventManager.OnWoodAdded += AddWoodList;
    }
    private void OnDisable()
    {
        EventManager.OnWoodAdded -= AddWoodList;
    }

    private void canMove()
    {
        settings.canMove = true;
    }

}

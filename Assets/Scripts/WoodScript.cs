using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using DG.Tweening;
using System.Linq;

public class WoodScript : MonoBehaviour
{
    //[SerializeField] ObjectifPool objectPool;
    [SerializeField] Transform ModelContainerT;
    [SerializeField] GameObject destroyableTree;
    [SerializeField] PlayerSettings settings;
    //[SerializeField] GameObject Cila;
    [SerializeField] Animator Animator;
    [SerializeField] WoodStack stack;
    RaycastHit hit;
    public LayerMask mask;
    public Material[] mats;
    public ParticleSystem explosionEffect;
    public int tagIndex;
    public int WoodPuan;
    [SerializeField] GameObject particul;
    public bool turn = true;
    public bool upgradeIsDone;

    public WoodStack transporter
    {
        set
        {
            _transporter = value;
            if (value == null)
            {
                ModelContainerT.localScale = new Vector3(1f, 1f, 1f);
                IdleAnimPlay(true);
            }
        }
        get { return _transporter; }
    }

    private WoodStack _transporter;

    private List<string> DoorsName;
    private Collider MyCollider;
    private Models modeller;
    private GameObject Model;

    private void Start()
    {
        modeller = FindObjectOfType<Models>();
        MyCollider = GetComponent<Collider>();
        stack = FindObjectOfType<WoodStack>();
        DoorsName = new List<string>();

        SpawnModel(tagIndex);
        IdleAnimPlay(true);

        mats = new Material[2];
    }
    private void Update()
    {
        if (turn && tagIndex == 0)
        {
            if (Physics.Raycast(transform.position + new Vector3(0.6f, 0f, 0f), Vector3.forward, 5f, mask))
            {
                turn = false;
                transform.DORotate(Vector3.right * -90, 0.5f).OnComplete(CanTurn);
            }

            else if (Physics.Raycast(transform.position + new Vector3(-0.6f, 0f, 0f), Vector3.forward, 5f, mask))
            {
                turn = false;
                transform.DORotate(Vector3.right * -90, 0.5f).OnComplete(CanTurn);

            }

            else if (Physics.Raycast(transform.position + new Vector3(0f, 0f, 0f), Vector3.forward, 5f, mask))
            {
                turn = false;
                transform.DORotate(Vector3.right * -90, 0.5f).OnComplete(CanTurn);

            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(Layers.wood))
        {
            EventManager.Event_OnWoodAdded(other.GetComponent<WoodScript>());
        }

        if ((other.gameObject.layer == LayerMask.NameToLayer(Layers.obstacle)) && transporter != null)
        {
            if (stack.woods.Count > 1)
            {
                transporter.DropWood(this);
            }
        }
    }

    public void SpawnModel(int indexModel = 0)
    {
        if (modeller == null) modeller = FindObjectOfType<Models>();
        tagIndex = indexModel;
        gameObject.tag = Tags.taglar[indexModel];
        WoodPuan = tagIndex + 5;
        Model = modeller.Modeller[tagIndex];
        if (ModelContainerT.childCount != 0) Destroy(ModelContainerT.GetChild(0)?.gameObject);

        GameObject model = Instantiate(Model, ModelContainerT);
        model.transform.localPosition = Vector3.zero;

    }

    public void IdleAnimPlay(bool value)
    {
        Animator.SetBool("IdleAnim", value);
        if (!value)
        {
            ModelContainerT.localPosition = Vector3.zero;
        }
    }
    
    public void DestRoyWood()
    {
        if (stack.woods[0].gameObject == this.gameObject)
        {
            return;
        }
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    public void DropStackList()
    {
        MyCollider.enabled = false;
        Invoke("EnableCollider", 1f);
        Vector3 paunch = new Vector3(transform.position.x + Random.Range(2f, -2f), transform.position.y, transform.position.z + Random.Range(1f, 3f));
        transform.DOJump(paunch, 2f, 1, 1f);
    }

    private void EnableCollider()
    {
        MyCollider.enabled = true;
    }

    public void UpGrade(string name, Material _material = null)
    {
        upgradeIsDone = false;
       
        if (modeller == null) modeller = FindObjectOfType<Models>();

        if (!DoorsName.Contains(name))
        {
            DoorsName.Add(name);// arda arda etkileþimi engellemek için
            switch (tagIndex)
            {
                case 0:
                    {
                        CutTree();
                        break;
                    }
                case 1:
                    {
                        cutWood();
                        break;
                    }
                case 2:
                    {
                        ChangeColor(_material);
                        break;
                    }
                //case 3:
                //    {
                //        Polish(_material);
                //        break;
                //    }
                //case 4:
                //    {
                //        Pattern(pattern, patternMetal);
                //        break;
                //    }
                default:
                    {
                        Debug.LogError("tagIndex olmamasý gereken bir deðerde !!");
                        break;
                    }
            }


            tagIndex++;
            gameObject.tag = Tags.taglar[tagIndex];

            if (tagIndex < modeller.Modeller.Length)
            {
                Model = modeller.Modeller[tagIndex];
            }
            if (transform.childCount != 0)
            {
                Destroy(transform.GetChild(0).GetChild(0).gameObject);
            }
            int tutucu = WoodPuan;
            WoodPuan = (tagIndex * 5) + WoodPuan;

            GameObject model = Instantiate(Model, ModelContainerT);
            model.transform.localPosition = Vector3.zero;
            AnimationScaleWood();
            EventManager.Event_OnIncreaseScore(WoodPuan - tutucu);
            

        }

        upgradeIsDone = true;

    }


    public void ShakeProcessStart(float waitTime)
    {
        StartCoroutine(AnimTrigger(waitTime));
    }

    IEnumerator AnimTrigger(float time)
    {
        yield return new WaitForSeconds(time);
        AnimationScaleWood();
    }

    public void AnimationScaleWood()
    {
        Animator.Play("ScaleWood");
    }

    private void CutTree()
    {
        GameObject parcalananAgac = ObjectifPool.singleton.getModel("Agac");
        parcalananAgac.transform.position = transform.position + (Vector3.down);
        parcalananAgac.transform.rotation =Quaternion.Euler(90, 0, 0);
        parcalananAgac.SetActive(true);
    }

    private void cutWood()
    {
        GameObject parcalananOdun = ObjectifPool.singleton.getModel("Wood");
        parcalananOdun.transform.position = transform.position;
        parcalananOdun.SetActive(true);
        //Instantiate(particul, transform.position, Quaternion.identity);
    }

    public void Polish(Material _material)
    {
        Material mat = ModelContainerT.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
        _material.color = mat.color;
        StartCoroutine(changeMat(0.01f, _material));
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void Pattern(Texture pattern, Texture patternMetal)
    {
        mats[0] = new Material(mats[0]);
        mats[1] = mats[0];
        transform.GetChild(0).gameObject.SetActive(false);
        foreach (Material item in mats)
        {
            item.SetTexture("_MainTex", pattern);
        }
        StartCoroutine(changeMat(0.01f, null));
    }

    public void ChangeColor(Material _material)
    {
        mats[0] = _material;
        mats[1] = mats[0];
        StartCoroutine(changeMat(0.01f, _material));
    }

    IEnumerator changeMat(float time, Material changeMat = null)
    {
        yield return new WaitForSeconds(time);
        if (changeMat != null)
        {
            ModelContainerT.GetChild(0).GetChild(0).GetComponent<Renderer>().material = changeMat;
        }
        else
        {
            Material[] mat = ModelContainerT.GetChild(0).GetChild(0).GetComponent<Renderer>().materials;
            for (int i = 0; i < mat.Length; i++)
            {
                mat[i] = mats[i];

            }
            ModelContainerT.GetChild(0).GetChild(0).GetComponent<Renderer>().materials = mat;
        }
    }
    public Material getChildMat()
    {
        return ModelContainerT.GetChild(0).GetChild(0).GetComponent<Renderer>().material;
    }
    void CanTurn()
    {
        turn = true;
        transform.DORotate(Vector3.zero, 0.5f);

    }
}

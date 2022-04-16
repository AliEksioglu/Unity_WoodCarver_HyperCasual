using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [SerializeField] private PlayerSetting settings;
    [SerializeField] Animator animPlayer;
    [SerializeField] Camera ortho;
    [SerializeField] WoodStack stack;
    public LayerMask mask;
    public Vector3 mouseDif;
    Camera cam;
    public LayerMask maskGround;

    private Vector3 mousePos;
    private Vector3 firstPos;
    private bool rundStart;

    private void Start()
    {
        rundStart = settings.isPlaying;
        cam = Camera.main;
        settings.canMove = true;
        //myRB.centerOfMass = Vector3.zero; //Devrilmemesi için


    }
    public void MousePosRest()
    {
        firstPos = Vector3.zero;
        mousePos = Vector3.zero;
    }
    private void Update()
    {
        if (settings.isPlaying)
        {

            Move();

            if (!GameManager.levelFinish)
            {
                firstPos = Vector3.Lerp(firstPos, mousePos, 0.9f);

                if (Input.GetMouseButtonDown(0))
                    MouseDown(Input.mousePosition);
                else if (Input.GetMouseButton(0))
                    MouseHold(Input.mousePosition);
                else
                {
                    mouseDif = Vector3.zero;
                }
            }
        }

        if (GameManager.levelFinish)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }

        

        if (Input.GetMouseButtonDown(0) && !rundStart)
        {
            rundStart = true;
            EventManager.Event_OnStartLevel();
        }
    }

    private void FixedUpdate()
    {
        if (settings.isPlaying)
        {
            float xPos = Mathf.Clamp(transform.position.x + mouseDif.x, -7f, 7f);

            transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
        }
    }
    void Move()
    {
        
        if (settings.canMove)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + settings.ForwardSpeed * Time.fixedDeltaTime);
        }
    }

    private void Move2()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.transform.localPosition.z;
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 50, maskGround))
        {
            Vector3 hitVec = hit.point;
            float xPos = Mathf.Clamp(hitVec.x, -7f, 7f);
            hitVec.x = xPos;
            hitVec.y = transform.localPosition.y;
            hitVec.z = transform.localPosition.z;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, hitVec, Time.deltaTime * settings.sensitivity);
        }
        //GameObject firstCube = ATMRUS..instance.cubes[0];

    }

    private void MouseDown(Vector3 inputPos)
    {
        mousePos = ortho.ScreenToWorldPoint(inputPos);
        firstPos = mousePos;
    }

    private void MouseHold(Vector3 inputPos)
    {
        mousePos = ortho.ScreenToWorldPoint(inputPos);
        mouseDif = mousePos - firstPos;
        mouseDif *= settings.sensitivity * Time.deltaTime;
    }


}
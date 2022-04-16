using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using TMPro;


public class GameManager : MonoBehaviour
{
    [SerializeField] public PlayerSetting settings;
    [SerializeField] Text animText;
    [SerializeField] Text finishScore;
    [SerializeField] GameObject secondCam;
    [SerializeField] GameObject startCam;
    [SerializeField] WoodStack woodStack;

    public static bool levelFinish;
    public int score;
    public UiManager UImanager;

    private int collectscore;
    private int AnimPuan;

    [SerializeField] Animation anim;

    private void Awake()
    {
        settings.isPlaying = false;
        settings.oyunSonuMats =new List<List<Material>>();
    }
    void Start()
    {
        UImanager.StartPanelEnable(true);
        startCam.SetActive(true);
        UImanager = FindObjectOfType<UiManager>();
        levelFinish = false;
        //finishScore.gameObject.GetComponent<Animation>();
    }

    public void LevelStart()
    {
        UImanager.StartPanelEnable(false);
        startCam.SetActive(false);
        //startCam.GetComponent<cin>
        StartCoroutine(wait(0.4f));
    }
    IEnumerator wait(float time)
    {
        yield return new WaitForSeconds(time);
        settings.isPlaying = true;
    }
    public void LevelFinish()
    {
        UImanager.LevelFinished();   //level complete paneli açýlmasý
    }

    public void FinishFirstTouch()
    {
        levelFinish = true;
        secondCam.SetActive(true);
    }
    public int InstantieWood()
    {
        List<WoodScript> listWood = woodStack.woods;

        float InstantieModelindex = (float)(collectscore / listWood.Count);
        int tutucu = (int)InstantieModelindex;
        if (InstantieModelindex > (tutucu + 0.3f))
        {
            return tutucu + 1;
        }
        else
        {
            return tutucu;
        }
    }

    public void waitAnimPuan() // Increase Skordaki animText animasyon eventi
    {
        AnimPuan = 0;
        animText.gameObject.SetActive(false);
    }
    public void IncreaseScore(int puan)
    {
        CancelInvoke();

        AnimPuan += puan;
        settings.score += puan;
        settings.TotalScore += puan;
        animText.color = Color.yellow;
        animText.text = "+" + AnimPuan.ToString();
        animText.gameObject.SetActive(false);
        animText.gameObject.SetActive(true);
        Invoke("waitAnimPuan", 0.4f);

    }
    public void RestScore(int puan)
    {
        animText.color = Color.red;
        animText.text = "-" + (settings.score - puan);
        settings.TotalScore -= settings.score - puan;
        settings.score = puan;
        animText.gameObject.SetActive(false);
        animText.gameObject.SetActive(true);
    }

    public void LastScore(int puan)
    {
        score += puan;
    }

    private void OnEnable()
    {
        EventManager.OnStartLevel += LevelStart;
        EventManager.OnIncreaseScore += IncreaseScore;
        EventManager.OnLastScore += LastScore;
        EventManager.OnLevelFinish += LevelFinish;
        EventManager.OnRestScore += RestScore;
        EventManager.FinishFirstTouch += FinishFirstTouch;
    }
    private void OnDisable()
    {
        EventManager.OnStartLevel -= LevelStart;
        EventManager.OnIncreaseScore -= IncreaseScore;
        EventManager.OnLevelFinish -= LevelFinish;
        EventManager.OnLastScore -= LastScore;
        EventManager.OnRestScore -= RestScore;
        EventManager.FinishFirstTouch -= FinishFirstTouch;

    }
}

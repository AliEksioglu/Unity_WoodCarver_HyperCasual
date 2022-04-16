using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings")]
public class PlayerSetting : ScriptableObject
{
    public bool isPlaying;
    public bool isDeath;
    public bool canMove;
    public float ForwardSpeed;
    public float sensitivity;
    public int score;
    public int finishScore;
    public int level;
    public int playingCountLevel;
    public int howManyObjectsOpend;
    public int index = 0;
    public int levelcount = 0;
    public int TotalScore;
    public float scaleNumber;
    public string axis;

    public List<List<Material>> oyunSonuMats;


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManagment : MonoBehaviour
{
    [SerializeField] PlayerSetting settings;
    [SerializeField] GameObject level1;
    [SerializeField] GameObject level2;
    [SerializeField] GameObject level3;

    private void Awake()
    {
        if (settings.levelcount == 1)
        {
            level1.SetActive(true);
            level2.SetActive(false);
            level3.SetActive(false);
        }
        else if (settings.levelcount == 2)
        {
            level1.SetActive(false);
            level2.SetActive(true);
            level3.SetActive(false);
        }
        else if (settings.levelcount == 3)
        {
            level1.SetActive(false);
            level2.SetActive(false);
            level3.SetActive(true);
        }
    }
    public void NextLevel()
    {
        settings.levelcount++;
        if (settings.levelcount == 4)
        {
            settings.levelcount = 1;
            settings.index = 0;
            settings.howManyObjectsOpend = 0;
        }
        if (settings.levelcount == 1)
        {
            level1.SetActive(true);
            level2.SetActive(false);
            level3.SetActive(false);
        }
        else if (settings.levelcount == 2)
        {
            level1.SetActive(false);
            level2.SetActive(true);
            level3.SetActive(false);
        }
        else if (settings.levelcount == 3)
        {
            level1.SetActive(false);
            level2.SetActive(false);
            level3.SetActive(true);
        }
        SceneManager.LoadScene(0);

    }
}

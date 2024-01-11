using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelButton : MenuButton
{
    public string sceneName;
    public GameObject icon;

    public void Start()
    {
        icon.SetActive(true);
    }
    public override void OnArrowHit()
    {
        SceneManager.LoadScene(sceneName);
    }
}

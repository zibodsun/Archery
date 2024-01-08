using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MenuButton
{
    public override void OnArrowHit()
    {
        Debug.Log("Start Game");
        SceneManager.LoadScene("Brown");
    }
}

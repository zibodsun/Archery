using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MenuButton
{
    public override void OnArrowHit()
    {
        Debug.Log("Reset Level");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

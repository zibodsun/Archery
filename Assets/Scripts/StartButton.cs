using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MenuButton
{
    public override void OnArrowHit()
    {
        Debug.Log("Start Game");
    }
}

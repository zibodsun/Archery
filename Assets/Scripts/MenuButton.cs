using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    public virtual void OnArrowHit() {
        Debug.Log("generic arrow hit message");
    }
}

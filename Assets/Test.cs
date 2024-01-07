using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Quaternion rotation;
    public int controller;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotation = transform.rotation;
        if (controller == 1) {
            transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
    }
}

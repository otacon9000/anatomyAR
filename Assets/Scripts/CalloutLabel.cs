using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CalloutLabel : MonoBehaviour
{
    public LineRenderer line;
    public GameObject target;


    private void Start()
    {
        line.startWidth = 0.01f;
        line.endWidth = 0.01f;
    }

    private void Update()
    {
            line.SetPosition(0, this.transform.position);
            line.SetPosition(1, target.transform.position);
    }
}

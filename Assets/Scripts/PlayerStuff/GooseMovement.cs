using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GooseMovement : BaseMovement
{
    [SerializeField] private int extraJumps = 0;
    // Start is called before the first frame update
    void Start()
    {
        ReceiveVariables();
        SetJumps(extraJumps);
    }

    // Update is called once per frame
    void Update()
    {
        ReceiveInput();
    }
}

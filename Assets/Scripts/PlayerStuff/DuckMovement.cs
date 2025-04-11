using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckMovement : BaseMovement
{
    [SerializeField] private int extraJumps = 1;
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

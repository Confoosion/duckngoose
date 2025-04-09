using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] public GameObject phoneUI;
    private bool enabled = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (enabled)
            {
                phoneUI.SetActive(false);
                enabled = !enabled;
            }
            else
            {
                phoneUI.SetActive(true);
                enabled = !enabled;

            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestructionTest : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("A TEST SCRIPT IS ACTIVE ON " + gameObject.name);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    SendMessage("DoDamage", 5);
                }
            }
        }
    }
}

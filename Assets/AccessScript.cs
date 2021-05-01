using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessScript : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            gameObject.SetActive(false);
        }
    }
}

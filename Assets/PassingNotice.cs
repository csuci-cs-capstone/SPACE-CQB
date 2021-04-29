using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassingNotice : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Disable();
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}

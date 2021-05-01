using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] float incrementamount;
    [SerializeField] float r;
    [SerializeField] float g;
    [SerializeField] float b;
    // Start is called before the first frame update

    private void Start()
    {
        r = text.color.r;
        g = text.color.g;
        b = text.color.b;
        StartBlinking();
    }

    private void Blink()
    {
        text.color = new Color(text.color.r + incrementamount, text.color.g + incrementamount, text.color.b + incrementamount, 255);
        if (text.color.r == 255 || text.color.r == 0)
        {
            incrementamount *= -1;
        }
    }

    private void StartBlinking()
    {
        while(true)
        {
            Invoke("Blink",0.5f);
        }
    }

    private void StopBlinking()
    {
        StopCoroutine("Blink");
    }

    private void Update()
    {
        
    }
}

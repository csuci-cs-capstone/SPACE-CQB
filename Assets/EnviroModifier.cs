using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviroModifier : MonoBehaviour
{
    [SerializeField] private Modifiers.EnviroModifier Modifier;

    public Modifiers.EnviroModifier GetModifier()
    {
        return Modifier;
    }
}

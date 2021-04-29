using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardModifier : MonoBehaviour
{
    [SerializeField] private Modifiers.CardModifiers Modifier;

    public Modifiers.CardModifiers GetModifier()
    {
        return Modifier;
    }

    public bool HasAbility()
    {
        return GetModifier() != Modifiers.CardModifiers.None;
    }
}

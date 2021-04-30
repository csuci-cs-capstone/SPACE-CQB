using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierDescription : MonoBehaviour
{
    [SerializeField] private GameObject selectedObject;
    [SerializeField] private int xoffset;
    [SerializeField] private int yoffset;
    [SerializeField] private GameObject Spy;
    [SerializeField] private GameObject BattleIon;
    [SerializeField] private GameObject BattleAssault;
    [SerializeField] private GameObject HunterPack;
    [SerializeField] private GameObject Decoy;
    [SerializeField] private GameObject QuickDeploy;
    [SerializeField] private GameObject SuperNova;
    [SerializeField] private GameObject PowerCap;
    [SerializeField] private GameObject NoCapital;
    [SerializeField] private GameObject JunkYard;
    [SerializeField] private GameObject ECM;
    [SerializeField] private GameObject Blackhole;
    [SerializeField] private GameObject ActiveDescriptor;
    [SerializeField] private GameObject PlayerHand;
    [SerializeField] private GameObject PlayerField;
    [SerializeField] private GameObject PreGame1;
    [SerializeField] private GameObject PreGame2;
    [SerializeField] private GameObject PreGame3;
    [SerializeField] private GameObject OpponentField;

    private void Start()
    {
        Deactivate(BattleAssault);
        Deactivate(BattleIon);
        Deactivate(Spy);
    }

    void Update()
    {
        GameObject hitObject = GetObject();
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
        if (hitObject != null && selectedObject == null)
        {
            selectedObject = hitObject;
            if (selectedObject.GetComponent<CardModifier>() != null && (selectedObject.transform.IsChildOf(PlayerHand.transform) || selectedObject.transform.IsChildOf(PlayerField.transform) || selectedObject.transform.IsChildOf(OpponentField.transform) || selectedObject.transform.IsChildOf(PreGame1.transform) || selectedObject.transform.IsChildOf(PreGame2.transform) || selectedObject.transform.IsChildOf(PreGame3.transform)))
            {
                Modifiers.CardModifiers modifier = selectedObject.GetComponent<CardModifier>().GetModifier();
                switch (modifier)
                {
                    case Modifiers.CardModifiers.BattleBuddiesAssault:
                        ActiveDescriptor = BattleAssault;
                        break;
                    case Modifiers.CardModifiers.BattleBuddiesIon:
                        ActiveDescriptor = BattleIon;
                        break;
                    case Modifiers.CardModifiers.SPY:
                        ActiveDescriptor = Spy;
                        break;
                    case Modifiers.CardModifiers.HunterPack:
                        ActiveDescriptor = HunterPack;
                        break;
                    case Modifiers.CardModifiers.DECOY:
                        ActiveDescriptor = Decoy;
                        break;
                    case Modifiers.CardModifiers.Quick_Deploy:
                        ActiveDescriptor = QuickDeploy;
                        break;
                    default:
                        ActiveDescriptor = null;
                        break;
                }
            }
            else if(selectedObject.GetComponent<EnviroModifier>() != null)
            {
                Modifiers.EnviroModifier modifier = selectedObject.GetComponent<EnviroModifier>().GetModifier();
                switch (modifier)
                {
                    case Modifiers.EnviroModifier.SuperNova:
                        ActiveDescriptor = SuperNova;
                        break;
                    case Modifiers.EnviroModifier.PowerCap:
                        ActiveDescriptor = PowerCap;
                        break;
                    case Modifiers.EnviroModifier.NoCapital:
                        ActiveDescriptor = NoCapital;
                        break;
                    case Modifiers.EnviroModifier.JunkYard:
                        ActiveDescriptor = JunkYard;
                        break;
                    case Modifiers.EnviroModifier.ECM:
                        ActiveDescriptor = ECM;
                        break;
                    case Modifiers.EnviroModifier.Blackhole:
                        ActiveDescriptor = Blackhole;
                        break;
                    default:
                        ActiveDescriptor = null;
                        break;
                }
            }
            /*if (ActiveDescriptor != null)
            {
                if (!ActiveDescriptor.activeSelf)
                    Activate(ActiveDescriptor);
                ActiveDescriptor.transform.position = new Vector3(mousePosition.x + xoffset, mousePosition.y + yoffset, mousePosition.z);
            }*/
        }
        else if (hitObject != selectedObject && selectedObject != null)
        {
            selectedObject = null;
            if(ActiveDescriptor != null)
                Deactivate(ActiveDescriptor);
            ActiveDescriptor = null;
        }
        if (ActiveDescriptor != null)
        {
            if (!ActiveDescriptor.activeSelf)
                Activate(ActiveDescriptor);
            ActiveDescriptor.transform.position = new Vector3(mousePosition.x + xoffset, mousePosition.y + yoffset, mousePosition.z);
        }
    }

    private GameObject GetObject()
    {
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        RaycastHit2D[] objectsOnMousePosition = Physics2D.RaycastAll(mousePosition, Vector2.zero);
        foreach (RaycastHit2D objectHit in objectsOnMousePosition)
        {
            if (objectHit.collider.gameObject.GetComponent<CardModifier>() != null || objectHit.collider.gameObject.GetComponent<EnviroModifier>() != null)
            {
                return objectHit.collider.gameObject;
            }
        }
        return null;
    }

    private void Activate(GameObject localObject)
    {
        localObject.SetActive(true);
    }

    private void Deactivate(GameObject localObject)
    {
        if(localObject.activeSelf)
        {
            localObject.SetActive(false);
        }
    }
}

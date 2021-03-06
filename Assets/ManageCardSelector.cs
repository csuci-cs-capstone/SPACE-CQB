using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCardSelector : MonoBehaviour
{
    [SerializeField] private GameObject hitcard;
    [SerializeField] private GameObject ConfirmPopup;
    [SerializeField] private Manage_Card_Collection Collection;
    [SerializeField] private Manage_Card_Collection Deck;
    [SerializeField] private GameObject NoResponse;


    [SerializeField] private int clicks;
    private float lastTimer;
    private float currentTimer;

    private void Start()
    {
        DeactivateResponse();
    }
    public GameObject GetClickedCard()
    {
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        RaycastHit2D[] objectsOnMousePosition = Physics2D.RaycastAll(mousePosition, Vector2.zero);
        foreach (RaycastHit2D objectHit in objectsOnMousePosition)
        {
            if (objectHit.collider.gameObject.GetComponent<CQBCard>())
            {
                hitcard = objectHit.collider.gameObject;

                Debug.Log(hitcard.name);
                return objectHit.collider.gameObject;
            }
        }
        return null;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clicks += 1;
            if (clicks == 1)
            {
                Debug.Log("Set timer");
                lastTimer = Time.unscaledTime;
            }
            if (clicks >= 2)
            {
                currentTimer = Time.unscaledTime;

                float difference = currentTimer - lastTimer;

                if (difference <= 0.2f)
                {
                    clicks = 0;
                    GameObject card = GetClickedCard();
                    if (card != null)
                    {
                        if (card.transform.IsChildOf(Collection.gameObject.transform))
                        {
                            if(Deck.gameObject.transform.childCount > 0)
                            {
                                GameObject siblingCard = Deck.gameObject.transform.GetChild(0).gameObject;
                                if (siblingCard == null || card.GetComponent<CQBCard>().GetFaction() != siblingCard.GetComponent<CQBCard>().GetFaction())
                                {
                                    ActivateResponse();
                                    Invoke("DeactivateResponse", 3);
                                    return;
                                }
                            }
                            card.transform.SetParent(Deck.gameObject.transform, false);
                            Deck.Reorganize();
                            Collection.Reorganize();
                            Deck.UpdateList();
                            
                        }
                        else if(card.transform.IsChildOf(Deck.gameObject.transform))
                        {
                            if(Collection.gameObject.transform.childCount > 0)
                            {

                                GameObject siblingCard = Collection.gameObject.transform.GetChild(0).gameObject;
                                if (siblingCard == null || card.GetComponent<CQBCard>().GetFaction() != siblingCard.GetComponent<CQBCard>().GetFaction())
                                {
                                    ActivateResponse();
                                    Invoke("DeactivateResponse", 3);
                                    return;
                                }
                            }
                            card.transform.SetParent(Collection.gameObject.transform, false);
                            Deck.Reorganize();
                            Collection.Reorganize();
                            Deck.UpdateList();
                        }
                    }
                }
                else
                {
                    clicks = 0;
                }
            }
        }
        else
        {
            if (clicks < 2)
            {
                currentTimer = Time.unscaledTime;
                float difference = currentTimer - lastTimer;

                if (difference > .2f)
                {
                    clicks = 0;
                }
            }
        }
    }

    public void SetActiveDeck(Manage_Card_Collection deck)
    {
        this.Deck = deck;
    }

    public void SetActiveCollection(Manage_Card_Collection collection)
    {
        Collection = collection;
    }

    private void DeactivateResponse()
    {
        if (NoResponse.activeSelf)
        {
            NoResponse.SetActive(false);
        }
    }

    private void ActivateResponse()
    {
        NoResponse.SetActive(true);
    }
}

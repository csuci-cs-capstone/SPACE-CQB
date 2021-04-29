using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelection : MonoBehaviour
{
    [SerializeField] private Player_Behavior Player;
    [SerializeField] private GameMaster GameMaster;
    [SerializeField] private int clicks;
    [SerializeField] private float lastTimer;
    [SerializeField] private float currentTimer;
    [SerializeField] GameObject selectedCard;
    [SerializeField] SP_CardPile playfield;

    // Update is called once per frame
    void Update()
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
                    if (!Player.GetPlayerState().isPlayersTurn())
                    {
                        return;
                    }
                    selectedCard = GetClickedCard();
                    if (selectedCard != null)
                    {
                        if (selectedCard.transform.parent.tag.Contains("Enemy Hand"))
                        {
                            return;
                        }
                        if (Player.GetHand().CardIsInPile(selectedCard))
                        {
                            playfield = Player.GetPlayField().GetComponent<SP_CardPile>();
                            AnimateCardToField(playfield); 
                            Invoke("TransferCard", .8f);
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

    private GameObject GetClickedCard()
    {
        Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        RaycastHit2D[] objectsOnMousePosition = Physics2D.RaycastAll(mousePosition, Vector2.zero);

        foreach (RaycastHit2D objectHit in objectsOnMousePosition)
        {
            if (objectHit.collider.gameObject.GetComponent<CQBCard>() != null)
            {
                return objectHit.collider.gameObject;
            }
        }
        return null;
    }

    private void AnimateCardToField(SP_CardPile field)
    {
        GetComponent<CardPositionAnimator>().AnimateCardToPosition(selectedCard, field.gameObject.transform.position);
        GetComponent<CardScaleAnimator>().AnimateCardToScale(selectedCard);
    }

    private void TransferCard()
    {
        playfield.TransferCardToCardPile(selectedCard);
        GameMaster.SetPlayedCard(selectedCard);
        GameMaster.Continue();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private Player_Behavior Opponent;
    [SerializeField] private GameObject CardSelection;
    [SerializeField] private GameObject card;
    [SerializeField] private GameMaster GameMaster;
    [SerializeField] private int PassChance;
    [SerializeField] private int roll;
    [SerializeField] private bool Making_Move;

    private void Start()
    {
        Making_Move = false;
    }

    public void make_a_move()
    {
        Making_Move = true;
        Invoke("move", .5f);
    }

    private void ChooseCard()
    {
        List<GameObject> cards = Opponent.GetHand().GetCardsInCardPile();
        int index = Random.Range(0, cards.Count - 1);

        card = cards[index];
    }
    private void TransferCard()
    {
        Opponent.GetPlayField().GetComponent<SP_CardPile>().TransferCardToCardPile(card);
        GameMaster.SetPlayedCard(card);
        Making_Move = false;
        GameMaster.Continue();
    }

    private void move()
    {
        card = null;
        int seed = (int)(Time.realtimeSinceStartup * 100);
        Random.InitState(seed);
        roll = Random.Range(1, 100);
        if (roll <= PassChance)
        {
            Opponent.GetPlayerState().SetPassing();
        }
        else
        {
            ChooseCard();

            card.GetComponent<CQBCard>().ActivatePlayable();
            CardSelection.GetComponent<CardPositionAnimator>().AnimateCardToPosition(card, Opponent.GetPlayField().transform.position);
            CardSelection.GetComponent<CardScaleAnimator>().AnimateCardToScale(card);
            Invoke("TransferCard", .8f);
        }
    }

    public bool IsMakingMove()
    {
        return Making_Move;
    }
}

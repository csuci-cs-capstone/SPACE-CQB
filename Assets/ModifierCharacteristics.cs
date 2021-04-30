using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierCharacteristics : MonoBehaviour
{
    [SerializeField] private Modifiers.EnviroModifier Modifier;
    [SerializeField] private GameObject Token;
    [SerializeField] private GameObject Descriptor;
    [SerializeField] private GameObject AbilityIneractionField;
    [SerializeField] static private int MAXQUICKDEPLOY = 5;
    [SerializeField] static private int SPYREINFORCE = 2;
    [SerializeField] static private int JUNKYARDCHANCE = 30;



    public GameObject CreateToken()
    {
        GameObject token = Instantiate(Token, null);
        return token;
    }

    public GameObject ActivateDescriptor()
    {
        Descriptor.SetActive(true);
        return CreateToken();
    }

    public void DeactivateDescriptor()
    {
        if (Descriptor.activeSelf)
        {
            Debug.Log("Trying to deactivate Descriptor: " + Descriptor.name);
            Descriptor.SetActive(false);
        }
    }

    public Modifiers.EnviroModifier GetModifier()
    {
        return this.Modifier;
    }

    static public void Blackhole(GameObject card)
    {
        CQBCard.UnitType type = card.GetComponent<CQBCard>().GetUnitType();
        if (type == CQBCard.UnitType.CAPITAL || type == CQBCard.UnitType.FRIGATE)
        {
            Debug.Log("Applying Blackhole to " + card.name);
            int modifiedPower = card.GetComponent<CQBCard>().GetBasePower() / 2;
            card.GetComponent<CQBCard>().ActivateNegativeCost(modifiedPower);
        }
    }

    static public void SuperNova(GameObject card)
    {
        CQBCard.UnitType type = card.GetComponent<CQBCard>().GetUnitType();
        if (type == CQBCard.UnitType.FIGHTER || type == CQBCard.UnitType.CORVETTE)
        {
            Debug.Log("Applying Supernova to " + card.name);
            int modifiedPower = card.GetComponent<CQBCard>().GetBasePower() / 2;
            card.GetComponent<CQBCard>().ActivateNegativeCost(modifiedPower);
        }
    }
    static public void PowerCap(GameObject card)
    {
        Debug.Log("Applying PowerCap to " + card.name);
        int modifiedPower = card.GetComponent<CQBCard>().GetBasePower();
        if(modifiedPower > 5)
        {
            modifiedPower = 5;
            card.GetComponent<CQBCard>().ActivateNegativeCost(modifiedPower);
        }
    }
    static public void ECM(GameObject card)
    {
        if(card.GetComponent<CardModifier>().HasAbility())
        { 
            card.GetComponent<CQBCard>().ActivateNegativeSymbol();
        }
    }

    static public void JunkYard(Player_Behavior player, Player_Behavior opponent)
    {
        Random.InitState((int)Time.realtimeSinceStartup * 1000);
        int roll = Random.Range(1, 100);
        if(roll <= JUNKYARDCHANCE)
        {
            Player_Behavior combatant;
            roll = Random.Range(1, 100);
            if(roll % 2 == 0)
                combatant = player;
            else
                combatant = opponent;
            List<GameObject> cards = combatant.GetPlayField().GetComponent<SP_CardPile>().GetCardsInCardPile();
            roll = Random.Range(0, cards.Count - 1);
            cards[roll].transform.SetParent(combatant.GetDiscard().transform);
        }

    }

    static public void BattleBuddiesAssault(GameObject current_card, Player_Behavior combatant)
    {
        List<GameObject> playfield = combatant.GetPlayField().GetComponent<SP_CardPile>().GetCardsInCardPile();
        foreach(GameObject card in playfield)
        {
            CQBCard cardCharacteristics = card.GetComponent<CQBCard>();
            if (card.GetComponent<CardModifier>().GetModifier() == Modifiers.CardModifiers.BattleBuddiesIon && !cardCharacteristics.IsBuddy()) ///////////////////////////
            {
                int IonIndex = card.transform.GetSiblingIndex() - 1;
                int IonSiblingIndex = card.transform.GetSiblingIndex();
                int AssaultIndex = current_card.transform.GetSiblingIndex() - 1;

                CQBCard currentCharacteristics = current_card.GetComponent<CQBCard>();
                GameObject IonSibling = combatant.GetHand().gameObject.transform.GetChild(IonSiblingIndex).gameObject;

                currentCharacteristics.ActivatePositiveSymbol();
                currentCharacteristics.ActivatePositiveCost(currentCharacteristics.GetCurrentPower() + 3);
                currentCharacteristics.SetBuddy();

                cardCharacteristics.ActivatePositiveSymbol();
                cardCharacteristics.ActivatePositiveCost(cardCharacteristics.GetCurrentPower() + 3);
                cardCharacteristics.SetBuddy();

                if(IonSiblingIndex != AssaultIndex)
                {
                    IonSiblingIndex = AssaultIndex;
                    AssaultIndex = IonIndex;
                    IonIndex = AssaultIndex + 1;

                    IonSibling.transform.SetSiblingIndex(IonSiblingIndex);
                }
                else
                {
                    AssaultIndex = IonIndex;
                    IonIndex = AssaultIndex + 1;
                }
                current_card.transform.SetSiblingIndex(AssaultIndex);
                card.transform.SetSiblingIndex(IonIndex);
            }
        }
    }

    static public void BattleBuddiesIon(GameObject current_card, Player_Behavior combatant)
    {
        List<GameObject> playfield = combatant.GetPlayField().GetComponent<SP_CardPile>().GetCardsInCardPile();
        foreach (GameObject card in playfield)
        {
            CQBCard cardCharacteristics = card.GetComponent<CQBCard>();
            if (card.GetComponent<CardModifier>().GetModifier() == Modifiers.CardModifiers.BattleBuddiesAssault && !cardCharacteristics.IsBuddy())      ////////////////////////////////////
            {
                int AssaultIndex = card.transform.GetSiblingIndex();
                int AssaultSiblingIndex = card.transform.GetSiblingIndex() + 1;
                int IonIndex = current_card.transform.GetSiblingIndex();

                CQBCard currentCharacteristics = current_card.GetComponent<CQBCard>();
                GameObject IonSibling = combatant.GetHand().gameObject.transform.GetChild(AssaultSiblingIndex).gameObject;

                currentCharacteristics.ActivatePositiveSymbol();
                currentCharacteristics.ActivatePositiveCost(currentCharacteristics.GetCurrentPower() + 3);
                currentCharacteristics.SetBuddy();

                cardCharacteristics.ActivatePositiveSymbol();
                cardCharacteristics.ActivatePositiveCost(cardCharacteristics.GetCurrentPower() + 3);
                cardCharacteristics.SetBuddy();

                if (AssaultSiblingIndex != IonIndex)
                {
                    AssaultSiblingIndex = IonIndex;
                    IonIndex = AssaultIndex + 1;

                    IonSibling.transform.SetSiblingIndex(AssaultSiblingIndex);
                    current_card.transform.SetSiblingIndex(IonIndex);
                }
            }
        }
    }

    static public void Anti_Fighter(Player_Behavior opponent)
    {
        List<GameObject> opponentPlayfield = opponent.GetPlayField().GetComponent<SP_CardPile>().GetCardsInCardPile();
        foreach(GameObject card in opponentPlayfield)
        {
            CQBCard cardCharacteristics = card.GetComponent<CQBCard>();
            if (cardCharacteristics.GetUnitType() == CQBCard.UnitType.FIGHTER && !cardCharacteristics.GetDebuff())
            {
                cardCharacteristics.SetPower(cardCharacteristics.GetCurrentPower() / 2);
                cardCharacteristics.SetDebuff();
                return;
            }
        }
    }

    static public void Anti_Frigate(Player_Behavior opponent)
    {
        List<GameObject> opponentPlayfield = opponent.GetPlayField().GetComponent<SP_CardPile>().GetCardsInCardPile();
        foreach (GameObject card in opponentPlayfield)
        {
            CQBCard cardCharacteristics = card.GetComponent<CQBCard>();
            if (cardCharacteristics.GetUnitType() == CQBCard.UnitType.FRIGATE && !cardCharacteristics.GetDebuff())
            {
                cardCharacteristics.SetPower(cardCharacteristics.GetCurrentPower() / 2);
                cardCharacteristics.SetDebuff();
                return;
            }
        }
    }

    static public void Anti_Capital(Player_Behavior opponent)
    {
        List<GameObject> opponentPlayfield = opponent.GetPlayField().GetComponent<SP_CardPile>().GetCardsInCardPile();
        foreach (GameObject card in opponentPlayfield)
        {
            CQBCard cardCharacteristics = card.GetComponent<CQBCard>();
            if (cardCharacteristics.GetUnitType() == CQBCard.UnitType.CAPITAL)
            {
                cardCharacteristics.SetPower(cardCharacteristics.GetCurrentPower() / 3);
                return;
            }
        }
    }

    static public void HunterPack(GameObject current_card, Player_Behavior combatant)
    {
        List<GameObject> playfield = combatant.GetPlayField().GetComponent<SP_CardPile>().GetCardsInCardPile();
        int siblingIndex = 0;
        int new_power;
        int index;
        List<GameObject> pack = new List<GameObject>();
        foreach (GameObject card in playfield)
        {
            if (current_card != card && current_card.GetComponent<CardModifier>().GetModifier() == card.GetComponent<CardModifier>().GetModifier())
            {
                pack.Add(card);
            }
        }

        if(pack.Count > 0)
        {
            new_power = pack[0].GetComponent<CQBCard>().GetCurrentPower() + current_card.GetComponent<CQBCard>().GetCurrentPower();
            current_card.GetComponent<CQBCard>().ActivatePositiveCost(new_power);
            foreach (GameObject pack_mate in pack)
            {
                pack_mate.GetComponent<CQBCard>().ActivatePositiveCost(new_power);
                siblingIndex++;
            }

            siblingIndex = pack[pack.Count - 1].transform.GetSiblingIndex();
            index = current_card.transform.GetSiblingIndex() - 1;
            if (siblingIndex != index)
            {
                GameObject tempSibling = pack[pack.Count - 1].transform.parent.gameObject.transform.GetChild(siblingIndex).gameObject;
                tempSibling.transform.SetSiblingIndex(index);
                current_card.transform.SetSiblingIndex(siblingIndex);
            }
        }
    }

    static public void Quick_Deploy(Player_Behavior player)
    {
        List<GameObject> cards = player.GetDeck().GetCards();
        int deployed = 0;
        for(int i = 0; i < cards.Count && deployed < MAXQUICKDEPLOY; i++)
        {
            if(cards[i].GetComponent<CQBCard>().GetUnitType() == CQBCard.UnitType.FIGHTER)
            {
                cards[i].GetComponent<CQBCard>().ActivatePlayable();
                cards[i].transform.SetParent(player.GetPlayField().transform);
                deployed++;
            }
        }
        cards = player.GetHand().GetCardsInCardPile();
        for(int i = 0; i < cards.Count && deployed < MAXQUICKDEPLOY; i++)
        {
            if (cards[i].GetComponent<CQBCard>().GetUnitType() == CQBCard.UnitType.FIGHTER)
            {
                cards[i].transform.SetParent(player.GetPlayField().transform);
                deployed++;
            }
        }
    }
    static public void Decoy(GameObject current_card, Player_Behavior player)
    {
        Debug.Log("TODO");
    }
    static public void CAP(GameObject current_card, Player_Behavior player)
    {
        Debug.Log("TODO");
    }

    static public void SPY(GameObject current_card, Player_Behavior player, Player_Behavior opponent)
    {
        player.GetDeck().DealCards(2, player.GetHand().gameObject);
        current_card.transform.SetParent(opponent.GetPlayField().transform);
    }
}

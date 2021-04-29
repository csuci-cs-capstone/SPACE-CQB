using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Management_Scene_Handler : MonoBehaviour
{
    [SerializeField] private AccountCharacteristics Account;
    [SerializeField] private Manage_Card_Collection Collection;
    [SerializeField] private ScrollRect CollectionContent;
    [SerializeField] private ScrollRect DeckContent;
    [SerializeField] private Manage_Card_Collection Manage_Deck1;
    [SerializeField] private Manage_Card_Collection Manage_Deck2;
    [SerializeField] private Manage_Card_Collection Manage_Deck3;
    [SerializeField] private GameCardsBehavior AllCards;
    [SerializeField] private List<string> deck_list;
    [SerializeField] private ManageCardSelector Selector;
    [SerializeField] private StatisticsHandler Stats;
    [SerializeField] private List<CQBCard.FactionType> factions;
    [SerializeField] private int ActiveFactionIndex;
    [SerializeField] private Image FactionSymbol;
    [SerializeField] private List<Sprite> factionsymbols;
    [SerializeField] private List<Manage_Card_Collection> factionCollections;
    [SerializeField] private List<Manage_Card_Collection> decks;


    private void Start()
    {
        //Create all cards within the collection

        Account = GameObject.Find("ActiveAccount").GetComponent<AccountCharacteristics>();
        CQBCard.FactionType kushan = CQBCard.FactionType.KUSHAN;
        CQBCard.FactionType taiidan = CQBCard.FactionType.TAIIDAN;


        //TESTING
        Account.LoadExistingPlayer(Account.GetPlayerName());
        //

        factions.Add(kushan);
        factions.Add(taiidan);

        Debug.Log(Account.playername);
        Account.PrintCollection(0);


        ActiveFactionIndex = 0;
        LoadFaction();
        Collection.TransferCards(factionCollections[ActiveFactionIndex].gameObject, factions[ActiveFactionIndex]);


        ActiveFactionIndex = 1;
        LoadFaction();
        List<GameObject> cards = Collection.TransferCards(factionCollections[ActiveFactionIndex].gameObject, factions[ActiveFactionIndex]);
        foreach (GameObject card in cards)
        {
            card.transform.localScale = new Vector2(.6f, .6f);
        }
        DeactivateCollection();

        ActiveFactionIndex = 0;

        PopulateDecks();

        Manage_Deck2.gameObject.SetActive(false);
        Manage_Deck3.gameObject.SetActive(false);
    }

    public void Change_Deck(int val)
    {
        switch (val)
        {
            case 0:
                if (!Manage_Deck1.gameObject.activeSelf)
                {
                    Manage_Deck1.gameObject.SetActive(true);
                    Selector.SetActiveDeck(Manage_Deck1);
                    Stats.SetActiveDeck(Manage_Deck1);
                    DeckContent.content = Manage_Deck1.gameObject.GetComponent<RectTransform>();
                }
                if (Manage_Deck2.gameObject.activeSelf)
                {
                    Manage_Deck2.gameObject.SetActive(false);
                }
                if (Manage_Deck3.gameObject.activeSelf)
                {
                    Manage_Deck3.gameObject.SetActive(false);
                }
                break;
            case 1:
                if (Manage_Deck1.gameObject.activeSelf)
                {
                    Manage_Deck1.gameObject.SetActive(false);
                }
                if (!Manage_Deck2.gameObject.activeSelf)
                {
                    Selector.SetActiveDeck(Manage_Deck2);
                    Stats.SetActiveDeck(Manage_Deck2);
                    Manage_Deck2.gameObject.SetActive(true);
                    DeckContent.content = Manage_Deck2.gameObject.GetComponent<RectTransform>();
                }
                if (Manage_Deck3.gameObject.activeSelf)
                {
                    Manage_Deck3.gameObject.SetActive(false);
                }
                break;
            case 2:
                if (Manage_Deck1.gameObject.activeSelf)
                {
                    Manage_Deck1.gameObject.SetActive(false);
                }
                if (Manage_Deck2.gameObject.activeSelf)
                {
                    Manage_Deck2.gameObject.SetActive(false);
                }
                if (!Manage_Deck3.gameObject.activeSelf)
                {
                    Selector.SetActiveDeck(Manage_Deck3);
                    Stats.SetActiveDeck(Manage_Deck3);
                    Manage_Deck3.gameObject.SetActive(true);
                    DeckContent.content = Manage_Deck3.gameObject.GetComponent<RectTransform>();
                }
                break;
            default:
                if (!Manage_Deck1.gameObject.activeSelf)
                {
                    Selector.SetActiveDeck(Manage_Deck1);
                    Stats.SetActiveDeck(Manage_Deck1);
                    Manage_Deck1.gameObject.SetActive(true);
                    DeckContent.content = Manage_Deck1.gameObject.GetComponent<RectTransform>();
                }
                if (Manage_Deck2.gameObject.activeSelf)
                {
                    Manage_Deck2.gameObject.SetActive(false);
                }
                if (Manage_Deck3.gameObject.activeSelf)
                {
                    Manage_Deck3.gameObject.SetActive(false);
                }
                break;
        }
    }

    public void Ready()
    {
        Save();
        SceneManager.LoadScene("CQBPrototype2");
    }

    public void Save()
    {
        Account.SetDeck(1, Manage_Deck1.GetList());
        Account.SetDeck(2, Manage_Deck2.GetList());
        Account.SetDeck(3, Manage_Deck3.GetList());
        Account.UpdateSaveFile();
    }

    public void NextFaction(int increment)
    {
        Save();
        Account.UpdateSaveFile();
        DeactivateCollection();
        ActiveFactionIndex += increment;
        if (ActiveFactionIndex > 1)
        {
            ActiveFactionIndex = ActiveFactionIndex % 1;
        }
        else if (ActiveFactionIndex < 0)
        {
            ActiveFactionIndex = 1;
        }
        ActivateCollection();
        FactionSymbol.sprite = factionsymbols[ActiveFactionIndex];
        Selector.SetActiveCollection(factionCollections[ActiveFactionIndex]);
        CollectionContent.content = factionCollections[ActiveFactionIndex].gameObject.GetComponent<RectTransform>();
    }

    private void LoadFaction()
    {
        GameObject card;
        List<string> tempCollection = Account.GetFactionCollection(factions[ActiveFactionIndex]);
        foreach (string cardname in tempCollection)
        {
            card = AllCards.CreateCard(cardname);
            Collection.AddCard(card);
        }
    }

    private void DeactivateCollection()
    {
        if (factionCollections[ActiveFactionIndex].gameObject.activeSelf)
        {
            factionCollections[ActiveFactionIndex].gameObject.SetActive(false);
        }
    }

    private void ActivateCollection()
    {
        factionCollections[ActiveFactionIndex].gameObject.SetActive(true);

    }

    private void PopulateDecks()
    {
        int index;
        for(int i = 0; i < decks.Count; i++)
        {
            deck_list = Account.GetDeck(i);
            index = DetermineFaction(deck_list);
            if(index != -1)
            {
                factionCollections[index].ListTransfer(deck_list, decks[i].gameObject.transform);
            }
        }
    }

    private int DetermineFaction(List<string> deck)
    {
        string name;
        string[] name_parts;

        if (deck.Count > 0)
        {
            name = deck[0];
            name_parts = name.Split('_');
            if (name_parts[0].Equals("Kushan"))
            {
                return 0;
            }

            else if (name_parts[0].Equals("Taiidan"))
            {
                return 1;
            }
            else
            {
                Debug.LogError("Error in loading decks");
                SceneManager.LoadScene("Management");
            }
        }
        return -1;
    }
}

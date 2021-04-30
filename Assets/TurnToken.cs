using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnToken : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] private Color PlayerColor;
    [SerializeField] GameObject Opponent;
    [SerializeField] private Color OpponentColor;
    [SerializeField] private Image Background;
    void Start()
    {
        Player.SetActive(false);
        Opponent.SetActive(false);
    }

    public void ActivatePlayerTurn()
    {
        Player.SetActive(true);
        Background.color = PlayerColor;
        if(Opponent.activeSelf)
        {
            Opponent.SetActive(false);
        }
    }

    public void ActivateOpponentTurn()
    {
        Opponent.SetActive(true);
        Background.color = OpponentColor;
        if(Player.activeSelf)
        {
            Player.SetActive(false);
        }
    }
}

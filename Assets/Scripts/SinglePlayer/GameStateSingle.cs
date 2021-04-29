using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateSingle : MonoBehaviour
{
    public enum BattleState { ENEMYTURN, PLAYERTURN, START, WON, LOST, PLAYERPASSING, EOR }
    public BattleState state;
    public SP_CardPile Hand;
    public PassingButton button;
    public PassingNotice PassingNotice;

    public void SetBattleState(int index)
    {
        this.state = (BattleState)index;
    }

    public bool isPlayersTurn()
    {
        return this.state == BattleState.PLAYERTURN;
    }

    public bool isPlayersPassing()
    {
        return this.state == BattleState.PLAYERPASSING;
    }

    public void SetPassing()
    {
        this.state = BattleState.PLAYERPASSING;
        button.Disable();
        PassingNotice.Enable();
    }

    public void Reset_Button()
    {
        button.Reset();
        PassingNotice.Disable();
    }
}

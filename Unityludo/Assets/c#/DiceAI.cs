using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DiceAI : MonoBehaviour
{
    private int _previousUnsuccessfullAttempts = 0;
    private Stack<int> _privousSixes = new Stack<int>();
    protected int RollDiceAl(int diceValue)
    {
        if (diceValue == 6)
        {
            int changedDiceValue = MonitoreSixes(diceValue);
            return changedDiceValue;
        }

        foreach (var pawn in PlayerInfo.instance.pawnInstances)
        {
            if (pawn.pawnType != PlayerInfo.instance.selectedPawn)
                continue;

            if (pawn.isLeftTheHouse) return 0;

        }

        _previousUnsuccessfullAttempts++;

        bool notLeftHomeInManyTurns = _previousUnsuccessfullAttempts > Random.Range(5, 8);
        if (notLeftHomeInManyTurns)
        {
            _previousUnsuccessfullAttempts = 0;
            return 6;
        }
        return 0;
    }


    private int MonitoreSixes(int sixes)
    {
        if (_privousSixes.Count > 2)
        {
            _privousSixes.Clear();
            return Random.Range(1, 5);
        }
        _privousSixes.Push(sixes);
        return 0;
    }
    public void OnReset()
    {
        _privousSixes.Clear();
        _previousUnsuccessfullAttempts = 0;
    }
}
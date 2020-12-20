using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfleImagePawnTypeMapper : MonoBehaviour
{
    [SerializeField] private Player _thisPlayer,_topLeftCornerPlayer,
        _topRightCornerPlayer,_buttonRightCornerPlayer;

    public void ArangePawnType()
    {
        _thisPlayer.playerPawn = PlayerInfo.instance.selectedPawn;
        _topRightCornerPlayer.playerPawn = (PawnType)GetOpponentPawnColour();
        int tempPawnType =(int) PlayerInfo.instance.selectedPawn;
        _topLeftCornerPlayer.playerPawn = PlayerInfo.instance.selectedPawn == PawnType.blue ? PawnType.yellow : (PawnType)(tempPawnType + 1);
        _buttonRightCornerPlayer.playerPawn = PlayerInfo.instance.selectedPawn == PawnType.yellow ? PawnType.blue : (PawnType)(tempPawnType - 1);
    }

    private int GetOpponentPawnColour()
    {
        int[] pawns = { 1, 2, 3, 4 };
        for (int i = 0; i < pawns.Length; i++)
        {
            bool isSameColour = pawns[i] == (int)PlayerInfo.instance.selectedPawn;
            if (isSameColour)
            {
                int opponentPawnColour = 2 + pawns[i] <= pawns.Length ? pawns[2 + i] : pawns[System.Math.Abs(pawns.Length - (2 + i))];
                return opponentPawnColour;
            }
        }
        return 0;
    }

    public void OnReset()
    {
        _buttonRightCornerPlayer.SetDefaultProfile();
        _topLeftCornerPlayer.SetDefaultProfile();
        _topRightCornerPlayer.SetDefaultProfile();
    }
}

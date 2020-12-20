using System;
using UnityEngine;

class SelectPawnColor : MonoBehaviour
{
    [SerializeField] private GameObject _matchMakingPanel;
    [SerializeField] private LudoBoard _board;
    [SerializeField] private ProfleImagePawnTypeMapper _profleImagePawnType;

    public static SelectPawnColor instance;

    private void Awake()
    {
        instance = this;
    }


    public void SelectColor(PawnType pawnType)
    {
        UiManager.instance.colorSelectionPanel.SetActive(false);
        PlayerInfo.instance.selectedPawn = pawnType;
        ServerRequest.instance.serverConnection = false;
        _matchMakingPanel.SetActive(true);
        ServerRequest.instance.MatchMaking(PlayerInfo.instance.players, pawnType);
        DiceController.instance.currentPawn = pawnType;
        DiceController.instance.UpdateValue();
        _board.Rotate(pawnType);
        _profleImagePawnType.ArangePawnType();
        PawnSpawner.instance.ArrangePawns(pawnType);
    }
}

using TMPro;
using UnityEngine;

class UiManager : MonoBehaviour
{
    public static UiManager instance;

    [SerializeField] private GameObject _ui, _playerSelectionPanel, _matchMakingPanel, _youWinPanel;
    [SerializeField] private GameObject _greenExitPanel, _yellowExitPanel, _blueExitPanel, _redExitPanel;
    [SerializeField] private GameObject _modePanel;
    [SerializeField] private GameObject _splachScreenPanel;
    [SerializeField] private TextMeshProUGUI _userId;

    private DiceAI _diceAI;
    private ProfleImagePawnTypeMapper _profleImagePawnTypeMapper;

    public Sprite defaultProfilePic; 
    public GameObject colorSelectionPanel;
    public TextMeshProUGUI onlinePlayers;

    public GameObject uiPanel { get { return _ui; } }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {

        //onlinePlayers.text = "1";
        _diceAI = GetComponent<DiceAI>();
        _profleImagePawnTypeMapper = GetComponent<ProfleImagePawnTypeMapper>();
        UpdateUi();
        _ui.SetActive(true);
        _playerSelectionPanel.SetActive(true);
        _modePanel.SetActive(true);
        _matchMakingPanel.SetActive(false);
        _splachScreenPanel.SetActive(true);
        _yellowExitPanel.SetActive(false);
        _greenExitPanel.SetActive(false);
        _redExitPanel.SetActive(false);
        _yellowExitPanel.SetActive(false);
        _youWinPanel.SetActive(false);
        _blueExitPanel.SetActive(false);
        _userId.text = LocalPlayer.playerId;

    }

    public void OnClick(string btn)
    {
        switch (btn)
        {
            case "green":
                SelectPawnColor.instance.SelectColor(PawnType.green);
                break;
            case "yellow":
                SelectPawnColor.instance.SelectColor(PawnType.yellow);
                break;
            case "blue":
                SelectPawnColor.instance.SelectColor(PawnType.blue);
                break;
            case "red":
                SelectPawnColor.instance.SelectColor(PawnType.red);
                break;
            case "playerNo2":
                PlayerInfo.instance.players = 2;
                _playerSelectionPanel.SetActive(false);
                colorSelectionPanel.SetActive(true);
                break;
            case "playerNo3":
                PlayerInfo.instance.players = 3;
                _playerSelectionPanel.SetActive(false);
                colorSelectionPanel.SetActive(true);
                break;
            case "playerNo4":
                PlayerInfo.instance.players = 4;
                _playerSelectionPanel.SetActive(false);
                colorSelectionPanel.SetActive(true);
                break;
            case "exitRoom":
                ResetLevel();
                break;
            case "online":
                _modePanel.SetActive(false);
                break;
            case "showdata":
                ServerResponse.socket.Emit("test");
                break;
            default:
                break;
        }

    }

    public void ResetLevel()
    {
        ServerRequest.instance.serverConnection = false;
        ServerRequest.instance.ExitRoom();
        _ui.SetActive(true);
        _modePanel.SetActive(true);
        _matchMakingPanel.SetActive(false);
        _splachScreenPanel.SetActive(false);
        _playerSelectionPanel.SetActive(true);
        _yellowExitPanel.SetActive(false);
        _greenExitPanel.SetActive(false);
        _blueExitPanel.SetActive(false);
        _redExitPanel.SetActive(false);
        _youWinPanel.SetActive(false);
        _diceAI.OnReset();
        _profleImagePawnTypeMapper.OnReset();
        colorSelectionPanel.SetActive(true);
        TempOnlinePlayersData.instance.RemoveAllPlayers();
        PawnTimer.stopTimer = true;
        PlayerInfo.instance.RemoveAllPawns();
    }


    public void ExitPanel(PawnType exitPawnType)
    {
        switch (exitPawnType)
        {
            case PawnType.yellow:
                _yellowExitPanel.SetActive(true);
                break;
            case PawnType.green:
                _greenExitPanel.SetActive(true);
                break;
            case PawnType.red:
                _redExitPanel.SetActive(true);
                break;
            case PawnType.blue:
                _blueExitPanel.SetActive(true);
                break;
            default:
                break;
        }
    }


    public void UpdateUi()
    {
        _userId.text = LocalPlayer.playerId;
    }

}


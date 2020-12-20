using UnityEngine;
class SetWinner : MonoBehaviour
{
    public static SetWinner instance;
    [SerializeField]private GameObject redCrown, yellowCrown, greenCrown, blueCrown;
    private void Awake()
    {
        instance = this;
    }

    public void OnPlayerWin(PawnType winnerPawnType)
    {
        switch (winnerPawnType)
        {
            case PawnType.yellow:
                yellowCrown.SetActive(true);
                break;
            case PawnType.green:
                greenCrown.SetActive(true);
                break;
            case PawnType.red:
                redCrown.SetActive(true);
                break;
            case PawnType.blue:
                blueCrown.SetActive(true);
                break;
            default:
                break;
        }
    }
}

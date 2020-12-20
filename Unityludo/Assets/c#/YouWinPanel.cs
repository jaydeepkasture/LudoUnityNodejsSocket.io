using UnityEngine;
using TMPro;

public class YouWinPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI youWinText;

    public void OnPlayerWin(PawnType lftPawn)
    {
        this.gameObject.SetActive(true);
        youWinText.text = lftPawn.ToString() + " left you win";
    }

    private void OnDisable()
    {
        youWinText.text = "you win";
    }

}

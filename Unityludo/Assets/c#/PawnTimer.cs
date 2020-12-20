using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PawnTimer : MonoBehaviour
{

    [SerializeField] Image[] players;
    private float speed = .1f;
    public static PawnTimer instance;
    public static bool stopTimer;

    private void Awake()
    {
        instance = this;
    }


    public IEnumerator Timer(PawnType pawntype)
    {
        int currentPawn = (int)pawntype;
        int selectedPawn = (int)PlayerInfo.instance.selectedPawn;
        int index = 0;
        if (selectedPawn <= 2)
        {
            index = selectedPawn <= currentPawn ? Mathf.Abs(selectedPawn - currentPawn) :
               Mathf.Abs(4 - currentPawn);
        }
        else
        {
            index = selectedPawn <= currentPawn ? Mathf.Abs(selectedPawn - currentPawn) :
              Mathf.Abs(4-(selectedPawn - currentPawn));
        }

        stopTimer = false;
        float i = 0;
        while (i < 0.99f)
        {

            if (stopTimer)
            {
                players[index].fillAmount = 0;
                yield break;
            }
            i = Mathf.SmoothStep(i, 1, speed);
            players[index].fillAmount = i;
            yield return new WaitForEndOfFrame();
        }
        players[index].fillAmount = 1;
        if (pawntype == PlayerInfo.instance.selectedPawn)
        {
            DiceController.instance.OnClick();
        }
        yield return new WaitForEndOfFrame();
        players[index].fillAmount = 0;
    }

}

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class DiceController : DiceAI
{
    [SerializeField] private Sprite[] _diceValue;
    [SerializeField] private Sprite[] _diceAnimation;
    [SerializeField] private Button _rollDiceButton;
    [SerializeField] private Color[] _diceColour;

    [SerializeField] public PawnType currentPawn=PawnType.yellow;
    [SerializeField] public int currentDiceValue;

     public static DiceController instance;

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] public bool playerMovementIsFinished;
    [SerializeField] public bool playerCanMove;
    private void Start()
    {
        playerMovementIsFinished = true;
        _rollDiceButton.image.color = _diceColour[(int)currentPawn-1];
    }

    public void UpdateValue()
    {
        playerMovementIsFinished = true;
        _rollDiceButton.image.color = _diceColour[(int)currentPawn - 1];
    }

    public void OnClick()
    {

        if (playerMovementIsFinished && currentPawn == PlayerInfo.instance.selectedPawn)
        {
            playerMovementIsFinished = false;
            StartCoroutine(RollDice());
        }
    }


    public bool movingAutomatically = false;
    public IEnumerator RollDice(int diceValueFromServer=0)
    {
        PawnTimer.stopTimer = true;
        int diceVal=0;
        if (diceValueFromServer != 0)
        {
            diceVal = diceValueFromServer-1;
            goto Aimation;
        }
        else
        { 
            diceVal = UnityEngine.Random.Range(0, _diceValue.Length); 
        }

        if(currentPawn==PlayerInfo.instance.selectedPawn)
        {
            int desirableDiceValue = base.RollDiceAl(diceVal+1);
            currentDiceValue = desirableDiceValue == 0 ? diceVal + 1 : desirableDiceValue;
            if (desirableDiceValue != 0) diceVal = desirableDiceValue-1;
        }
        else
        {
            currentDiceValue = diceVal + 1;
        }
        ServerRequest.instance.RollDice(currentDiceValue, currentPawn);

        Aimation:
        for (int i = 0; i < _diceAnimation.Length; i++)
        {
            _rollDiceButton.image.sprite = _diceAnimation[i];
            yield return new WaitForEndOfFrame();
        }
        _rollDiceButton.image.sprite = _diceValue[diceVal];
        yield return new WaitForSeconds(.5f);
        _rollDiceButton.image.color = _diceColour[(int)currentPawn - 1];


        bool diceValueNotFromSever = diceValueFromServer == 0;
        if (diceValueNotFromSever)
            playerCanMove = true;
    }

    
}

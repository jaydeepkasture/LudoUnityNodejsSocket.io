using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PawnMovementController : MonoBehaviour
{

    [SerializeField] public int spotIndexOnLudoBoard;
    [SerializeField] private GameObject[] commonMovingSpot;

    public int pawnNumber = 0;
    public int Lastspot = 0;
    public float speed = 5;
    public Spot currentSpot;
    public GameObject home;
    public PawnType pawnType;
    public bool richedTheDestination;

    private Spot destinationSpot;
    private bool _onSafeSpot;

    private void Start()
    {
        Lastspot = commonMovingSpot.Length;
        spotIndexOnLudoBoard = 0;
        destinationSpot = commonMovingSpot[commonMovingSpot.Length - 1].GetComponent<Spot>();
    }
    public IEnumerator MoveTo(int moveToIndex, bool fromServer = false)
    {

        CheckAndGetOutOfSafeSpot();
        //PawnTimer.stopTimer = true;
        Vector3 lastPostion = Vector3.zero;
        float distance = 0;

        if (!fromServer)
            ServerRequest.instance.MovePlayer(moveToIndex, pawnNumber, (int)pawnType);
        //move one by one
        for (int i = 0; i < moveToIndex; i++)
        {
            distance = (commonMovingSpot[i + spotIndexOnLudoBoard].transform.position - this.transform.position).sqrMagnitude;

            while (distance > .1f)
            {
                distance = (commonMovingSpot[i + spotIndexOnLudoBoard].transform.position - this.transform.position).sqrMagnitude;
                this.transform.position =
                    Vector3.MoveTowards(this.transform.position,
                    commonMovingSpot[i + spotIndexOnLudoBoard].transform.position,
                    speed);

                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(.1f);
            lastPostion = commonMovingSpot[i + spotIndexOnLudoBoard].transform.position;
            currentSpot = commonMovingSpot[i + spotIndexOnLudoBoard].GetComponent<Spot>();
        }
        this.transform.position = lastPostion;
        FinishMovement(moveToIndex);
        CheckForPawnKill(fromServer);
    }

    private void FinishMovement(int lastIndex)
    {
        spotIndexOnLudoBoard += lastIndex;
        DiceController.instance.playerMovementIsFinished = true;

        bool richedAtTheEnd = destinationSpot.spotNo == currentSpot.spotNo;
        if (richedAtTheEnd)
        {
            richedTheDestination = true;
            isLeftTheHouse = false;
            WinnerChecker.instance.CheckForWinner();
        }
    }
    private void CheckForPawnKill(bool fromServer)
    {
        if (CanKillEnemyPawn())
        {
            DiceController.instance.playerMovementIsFinished = true;
        }
        else
        {
            if (!fromServer)
            {
                print("riched the destination :" + richedTheDestination);
                ServerRequest.instance.PlayerFinishedMoving(richedTheDestination);
            }
        }
    }
   
   
    public bool CanMoveAhead(int moveTo)
    {
        if (moveTo != 6)
            return false;
        if (!isLeftTheHouse)
            return false;
        bool canMoveForward = spotIndexOnLudoBoard + moveTo <= Lastspot;

        return (spotIndexOnLudoBoard + moveTo <= Lastspot);
    }

    private bool IsOnSafeSpot()
    {
        if (currentSpot.GetComponent<SafeSpot>() != null)
        {
            currentSpot.gameObject.GetComponent<PawnAjusterOnSafeSpots>().AddPawn(this.gameObject);
            _onSafeSpot = true;
            return true;
        }

        return false;
    }

    private bool CanKillEnemyPawn()
    {

        if (IsOnSafeSpot())
        {
            return false;
        }

        bool canKillOtherPlayerPawn = false;
        GameObject pawnsOnSafeSpot = new GameObject();
        pawnsOnSafeSpot = null;
        foreach (var otherPlayersPawn in PlayerInfo.instance.pawnInstances)
        {
            bool stillAtHome = otherPlayersPawn.spotIndexOnLudoBoard == 0;
            if (stillAtHome)
            {
                continue;
            }

            bool samePawnType = otherPlayersPawn.pawnType == this.pawnType;
            if (samePawnType)
            {
                continue;
            }
            canKillOtherPlayerPawn = otherPlayersPawn.currentSpot.Equals(this.currentSpot);
            if (canKillOtherPlayerPawn)
            {
                StartCoroutine(SendBackToHome(otherPlayersPawn, otherPlayersPawn.home));
                break;
            }
        }


        return canKillOtherPlayerPawn;
    }


    private IEnumerator SendBackToHome(PawnMovementController pawn, GameObject home)
    {
        ResetPawn(pawn);
        float diffrentBetweenHouseAndPawn = (home.transform.position - pawn.gameObject.transform.position).sqrMagnitude;
        while (diffrentBetweenHouseAndPawn > .1f)
        {
            diffrentBetweenHouseAndPawn = (home.transform.position - pawn.gameObject.transform.position).sqrMagnitude;
            pawn.gameObject.transform.position = Vector3.MoveTowards(pawn.gameObject.transform.position,
                home.transform.position,
                speed);
            yield return new WaitForEndOfFrame();
        }
        pawn.gameObject.transform.position = home.transform.position;

    }


    void ResetPawn(PawnMovementController otherPlayer)
    {
        otherPlayer.isLeftTheHouse = false;
        otherPlayer.spotIndexOnLudoBoard = 0;
        otherPlayer.stepOutFromHome = 0;
        otherPlayer.currentSpot = null;
    }


    public bool isLeftTheHouse = false;
    protected int stepOutFromHome = 0;
    protected void CheckHome()
    {

        if (isLeftTheHouse)
        {
            return;
        }
        //very first time leaving the home
        if (!isLeftTheHouse && DiceController.instance.currentDiceValue == 6)
        {
            stepOutFromHome = 1;//value is going to use only once
            isLeftTheHouse = true;
            return;
        }
    }
    protected bool EnoughSpotsLeft(int moveToIndex)
    {
        bool enoughSpotsLeft = Lastspot >= moveToIndex + spotIndexOnLudoBoard;
        return enoughSpotsLeft;
    }
    protected void CheckAndGetOutOfSafeSpot()
    {
        if (!_onSafeSpot)
            return;

        this.transform.SetParent(PawnSpawner.instance.spotPanel.transform);
        RearrangePawns.instance.Rearrange();
        currentSpot.gameObject.GetComponent<PawnAjusterOnSafeSpots>().RemovePawn(this.gameObject);

        _onSafeSpot = false;
    }
}

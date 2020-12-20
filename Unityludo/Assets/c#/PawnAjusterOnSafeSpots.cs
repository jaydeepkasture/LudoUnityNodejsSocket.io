using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
class PawnAjusterOnSafeSpots : MonoBehaviour
{
    public GameObject gridContent;
    private GameObject _topPawn;
    public Button arrowbtn, closeBtn;
    public void Start()
    {
        arrowbtn.gameObject.SetActive(false);
        _topPawn = null;

        transform.GetChild(0).gameObject.SetActive(false);

        arrowbtn.onClick.AddListener(() =>
        {
            OnClick();
        });
        closeBtn.onClick.AddListener(() =>
        {
            CloseGridContentPanel();
        });
    }

    public void AddPawn(GameObject pawn)
    {
        if (_topPawn != null)
        {
            arrowbtn.gameObject.SetActive(true);
            _topPawn.transform.SetParent(gridContent.transform);
        }

        _topPawn = pawn;

    }

    public void RemovePawn(GameObject pawn)
    {
        CloseGridContentPanel();
       
        if (pawn == _topPawn)
        {
            if (gridContent.transform.childCount >= 1)
            {
                GameObject getPawn = gridContent.transform.GetChild(0).gameObject;
                getPawn.transform.SetParent(PawnSpawner.instance.spotPanel.transform);
                getPawn.transform.position = transform.position;
                _topPawn = getPawn;
            }
            else { _topPawn = null; };
        }

        bool isNoPawnLeftInTheGrid = gridContent.transform.childCount == 0;
        if (isNoPawnLeftInTheGrid)
        {
            arrowbtn.gameObject.SetActive(false);
        }
    }

    private void OnClick()
    {
        transform.GetChild(0).transform.gameObject.SetActive(true);

        if (SafeSpotController.instance.currentlyActiveSafeSpotGrid == null)
        {
            SafeSpotController.instance.currentlyActiveSafeSpotGrid = this.gameObject;
            return;
        }
        
        bool isSameGameobject = SafeSpotController.instance.currentlyActiveSafeSpotGrid == this.gameObject;
        if (isSameGameobject)
        {
            return;
        }

        SafeSpotController.instance.currentlyActiveSafeSpotGrid.SetActive(false);
        SafeSpotController.instance.currentlyActiveSafeSpotGrid = this.gameObject;
    }
    private void CloseGridContentPanel()
    {
        transform.GetChild(0).transform.gameObject.SetActive(false);
    }
}


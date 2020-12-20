using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LudoBoard : MonoBehaviour
{
    [SerializeField] private GameObject _ludoboard;

    public void Rotate(PawnType pawnType)
    {
        switch (pawnType)
        {
            case PawnType.yellow:
                _ludoboard.transform.eulerAngles = new Vector3(0, 0, 0);
                break;
            case PawnType.green:
                _ludoboard.transform.eulerAngles = new Vector3(0, 0, 90);
                break;
            case PawnType.red:
                _ludoboard.transform.eulerAngles = new Vector3(0, 0, 180);
                break;
            case PawnType.blue:
                _ludoboard.transform.eulerAngles = new Vector3(0, 0, 270);
                break;
            default:
                break;
        }
    }
}

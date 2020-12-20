using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField]private RectTransform rectComponent;
    private Image imageComp;
    private bool up;

    public float rotateSpeed = 200f;
    public float openSpeed = .005f;
    public float closeSpeed = .01f;
    public bool startMatchMaking = false;

    public static Loading instance;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        startMatchMaking = false;
        imageComp = rectComponent.GetComponent<Image>();
        up = true;
    }

    private void Update()
    {
        if (startMatchMaking)
        {
            rectComponent.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
            changeSize();
        }
    }

    private void changeSize()
    {
        float currentSize = imageComp.fillAmount;

        if (currentSize < .30f && up)
        {
            imageComp.fillAmount += openSpeed;
        }
        else if (currentSize >= .30f && up)
        {
            up = false;
        }
        else if (currentSize >= .02f && !up)
        {
            imageComp.fillAmount -= closeSpeed;
        }
        else if (currentSize < .02f && !up)
        {
            up = true;
        }
    }

}
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadingbar : MonoBehaviour {

    private RectTransform rectComponent;
    private Image imageComp;
   
    public float speed = 0.0f;
    public GameObject _laodingScreenPanel;
    // Use this for initialization
    void Start () {
        _laodingScreenPanel.SetActive(true);
        rectComponent = GetComponent<RectTransform>();
        imageComp = rectComponent.GetComponent<Image>();
        imageComp.fillAmount = 0.0f;
        StartCoroutine(StartAnimation());
    }

    private IEnumerator StartAnimation()
    {
        while (imageComp.fillAmount < .999f)
        {
            imageComp.fillAmount = imageComp.fillAmount + Time.deltaTime * speed;
            yield return new WaitForEndOfFrame();
        }
        imageComp.fillAmount = 1;
      
        yield return new WaitForEndOfFrame();
        _laodingScreenPanel.SetActive(false);
    }
}

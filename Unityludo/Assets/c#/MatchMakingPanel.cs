using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchMakingPanel : MonoBehaviour
{
    private float defaultTime = 60;
    private float timeRemaining ;
    [SerializeField] private TMPro.TextMeshProUGUI coundownText;
    [SerializeField] GameObject modePanel;
    [SerializeField] Loading loading;
    private void Awake()
    {
        timeRemaining = defaultTime;
        coundownText.text = timeRemaining.ToString();
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        StartCoroutine(Coundown());
        loading.startMatchMaking = true;
    }

    public IEnumerator Coundown()
    {
        bool isMatchMakingFailed = timeRemaining < 1;
        if (isMatchMakingFailed)
        {
            timeRemaining = defaultTime;
            gameObject.SetActive(false);
            OnMatchMaingFail();
            yield break;
        }
        yield return new WaitForSecondsRealtime(1f);
        timeRemaining -= 1;
        coundownText.text = timeRemaining.ToString();
        StartCoroutine(Coundown());
    }

    public void OnMatchMaingFail()
    {
        UiManager.instance.ResetLevel();
        ServerRequest.instance.QuitGame();
    }

    private void OnDisable()
    {
        timeRemaining = defaultTime;
        loading.startMatchMaking = false;
    }
}

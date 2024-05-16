using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupText : MonoBehaviour
{
    [SerializeField] private float fadeOutTime = 2f;
    [SerializeField] private float showTime = 5f;
    private TextMeshProUGUI text;
    [SerializeField] private Color visibleColour;
    [SerializeField] private Color invisibleColour;

    private Coroutine playPopupCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPopup()
    {
        text.color = visibleColour;

        if (playPopupCoroutine != null)
        {
            StopCoroutine(playPopupCoroutine);
            playPopupCoroutine = null;
        }
        playPopupCoroutine = StartCoroutine(PlayPopup());
    }

    private IEnumerator PlayPopup()
    {
        yield return new WaitForSeconds(showTime);

        float timer = 0f;

        while (timer < fadeOutTime)
        {
            timer += Time.deltaTime;
            text.color = Color.Lerp(visibleColour, invisibleColour, timer / fadeOutTime);
            yield return null;
        }

        yield return null;
    }
}

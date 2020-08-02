using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Observing class for all messages to the message box
/// </summary>
public class MessageManager : MonoBehaviour, ObserverBase
{
    #region Serialized Variables
    /// <summary>
    /// Amount of text allowed on screen (Note: due to the fadeout time there will be up to 2 more text objects on screen.)
    /// </summary>
    [SerializeField] private int i_maxTextOnScreen = 5;
    /// <summary>
    /// Amount of time for the user to read the text
    /// </summary>
    [SerializeField] private float f_readTime = 2; 
    /// <summary>
    /// Amount of time to fade out the text
    /// </summary>
    [SerializeField] private float f_fadeTime = 2;
    /// <summary>
    /// Amount of time to fade out text once the max amount of text has been reached
    /// </summary>
    [SerializeField] private float f_quickFadeTime = 0.5f;
    /// <summary>
    /// Panel to display the text on. Note: the pannel must also have a GridLayoutGroup attached
    /// </summary>    
    [SerializeField] private float f_fontSize = 30;
    /// <summary>
    /// Font Size of the created text
    /// </summary>
    [SerializeField] private Image im_pannel = null;
    /// <summary>
    /// The grid to set text as a child of.
    /// </summary>
    [SerializeField] private GridLayoutGroup grid = null;

    // Text Settings
    [SerializeField] private Color co_textColor;
    [Range(0.0f, 1.0f), SerializeField] private float f_textStartingAlpha;
    #endregion

    #region Private Variables
    /// <summary>
    /// Entire log of all text sent in game
    /// </summary>
    private List<TextWithImage> L_messageLog = new List<TextWithImage>();
    /// <summary>
    /// Current strings being displayed
    /// </summary>
    private List<string> L_currentMessages = new List<string>();
    /// <summary>
    /// Current text mesh pro objects on screen
    /// </summary>
    private List<TextMeshProUGUI> L_textOnScreen = new List<TextMeshProUGUI>();
    /// <summary>
    /// Text that is being faded out
    /// </summary>
    private List<TextMeshProUGUI> L_deadText = new List<TextMeshProUGUI>();
    #endregion

    #region Monobehaviour Functions
    // Start is called before the first frame update
    void Start()
    {
        // Search for the message manager and add it to the list of objects observing this
        //foreach (CollectableResource _collectableResource in FindObjectsOfType<CollectableResource>())
            //_collectableResource.AddObserver(this);
    }
    // Update is called once per frame
    void Update()
    {
        // Check to see if dead text is fully faded before deleting it
        for(int i = 0; i < L_deadText.Count; i++)
            if(L_deadText[i].alpha == 0)
            {
                Destroy(L_deadText[i].gameObject);
                L_deadText.Remove(L_deadText[i]);
            }
    }
    #endregion
    
    #region Observer Functions
    /// <summary>
    /// Once notified, store the text in the message log, and add text to screen
    /// </summary>
    /// <param name="newText">text to add to screen</param>
    public void OnNotify(TextWithImage newText)
    {
        // Store the text and image in a less temporary log
        L_messageLog.Add(newText);
        AddText(newText);
    }
    #endregion

    #region private functions
    /// <summary>
    /// Add text to the panel using the TextWithImage struct
    /// </summary>
    /// <param name="newText">text to be added to screen</param>
    void AddText(TextWithImage newText)
    {
        // Check if you should fade out the first message or add the image directly
        if(L_currentMessages.Count < i_maxTextOnScreen)
        {
            if(newText.text != null)
                L_currentMessages.Add(newText.text);
        }
        else
        {
            if(newText.text != null)
                L_currentMessages.Add(newText.text);
            FadeMessage(L_textOnScreen[0], f_quickFadeTime);
        }

        // Add a new text mesh pro component to a new game object and parent it to the panel
        GameObject newGo = new GameObject();
        newGo.transform.SetParent(grid.transform);
        TextMeshProUGUI newTmp = newGo.AddComponent<TextMeshProUGUI>();
        newGo.AddComponent<Outline>();
        // Set the text parameters
        if(newText.text != null)
        {
            newTmp.alignment = TextAlignmentOptions.Center;
            newTmp.fontSize = f_fontSize;
            newTmp.color = co_textColor;
            newTmp.alpha = f_textStartingAlpha;
            newTmp.text = newText.text;            
        }
        // Store the text as "on screen", set its transform to 0 on the z axis and start the reading timer
        L_textOnScreen.Add(newTmp);
        newGo.GetComponent<RectTransform>().localPosition = new Vector3(newGo.GetComponent<RectTransform>().localPosition.x, newGo.GetComponent<RectTransform>().localPosition.y, 0.0f);//Vector3.Scale(newGo.GetComponent<RectTransform>().localPosition, (Vector3.one - Vector3.right));
        StartCoroutine(ReadTimer(newTmp));
    }

    /// <summary>
    /// Removes the text from "active" lists and sets it as "dead." Begins the fadeout animation.
    /// </summary>
    /// <param name="tmp_textbox">Text to be faded out and deleted</param>
    /// <param name="timeToFade">Amount of time for the fadeout to run</param>
    void FadeMessage(TextMeshProUGUI tmp_textbox, float timeToFade)
    {
        // Set the text as dead, remove it from the current messages list
        L_deadText.Add(tmp_textbox);
        L_currentMessages.Remove(tmp_textbox.text);
        // Remove the object as "on screen" and begin to fade it out
        L_textOnScreen.Remove(tmp_textbox);
        StartCoroutine(Fade(tmp_textbox, timeToFade));
    }

    /// <summary>
    /// Gives the user some time to read the text before beginning the fadeout animation
    /// </summary>
    /// <param name="onScreenText">Text to fade</param>
    /// <returns></returns>
    IEnumerator ReadTimer(TextMeshProUGUI onScreenText)
    {
        // Wait until the read time has passed
        yield return new WaitForSeconds(f_readTime);
        // A fadeout might occur early on the object so check if it's already being faded.
        if(onScreenText.alpha >= f_textStartingAlpha)
            FadeMessage(onScreenText, f_fadeTime);
    }

    /// <summary>
    /// Fade animation for text
    /// </summary>
    /// <param name="text">Text mesh pro object to fade out</param>
    /// <param name="f_timeToFade">Amount of time for the animation </param>
    /// <returns></returns>
    IEnumerator Fade(TextMeshProUGUI text, float f_timeToFade)
    {
        // While the alpha is not 0, lerp the alpha down and increase the amount of time elapsed each frame
        float timeElapsed = 0;
        while (text.alpha > 0.0f)
        {
            timeElapsed += Time.deltaTime;
            if(text != null)
                text.alpha = Mathf.Lerp(1.0f, 0.0f, timeElapsed / f_timeToFade);
            yield return new WaitForEndOfFrame();
        }
        text.alpha = 0.0f;
    }
    #endregion
}

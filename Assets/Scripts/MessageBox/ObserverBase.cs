using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Text and image pair. Note: not currently used
/// </summary>
public struct TextWithImage
{
    public string text;
    public Image image;
}

/// <summary>
/// Interface for observing objects
/// </summary>
public interface ObserverBase
{
    /// <summary>
    /// Once notified with text, inhereted objects will implement functionality
    /// </summary>
    /// <param name="newData">Text and/or image sent</param>
    void OnNotify(TextWithImage newData);
}

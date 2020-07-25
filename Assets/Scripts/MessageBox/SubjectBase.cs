using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for subjects of observers.
/// </summary>
public class SubjectBase : MonoBehaviour
{
    /// <summary>
    /// List of all objects observing this subject
    /// </summary>
    protected List<ObserverBase> observers = new List<ObserverBase>();

    /// <summary>
    /// Add a new object to observe this subject
    /// </summary>
    /// <param name="newObserver">This object is now observing this subject</param>
    public void AddObserver(ObserverBase newObserver)
    {
        observers.Add(newObserver);
    }

    /// <summary>
    /// Sends data to all observers
    /// </summary>
    protected virtual void Notify(TextWithImage twi)
    {
        // For every object observing, try casting the observers to a message manager
        foreach (ObserverBase ob in observers)
            try
            {
                MessageManager mm = ob as MessageManager;
                // Create a new "TextWithImage" object, set the text, and send it to the message manager
                mm.OnNotify(twi);
            }
            catch (System.InvalidCastException e)
            {
                // If it's not a message manager debug the error.
                Debug.Log("Couldn't send message: " + e.ToString());
            }
    }

}


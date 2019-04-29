using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Task that completes when an object known as ball is dropped.
/// Uses the OnObjectReset event
/// </summary>
/// 

// When we declare a class and we use the ':' character followed by another class name
// this means that we are inheriting from that class. This means that any and all variables,
// methods etc are automatically given to our class of BallResetCheckListItem.

public class GunResetCheckListItem : CheckListItem
{
    // If you look at the CheckListItem script, you will see most of the functions and variables
    // are 'virtual', when we use the virtual keyword it means that we can override what that function, or
    // variable does, we do this with the 'override' keyword. 

    // A bool to check if we have already dropped the ball over the edge.
    public bool hasDroppedGun = false;
    /// <summary>
    ///Check if gun has been dropped
    /// </summary>
    // This is an example of a get and set function, we can use get and sets to do specific code, when we want to get or set a variable.
    // Get is when we want to get the value of IsComplete i.e. mybool = IsComplete; 
    // Set is when we want to change the value of IsComplete i.e.IsComplete = myBool;
    // An example of this would be lets say I want to set a score int and then update the ui.
    // Instead of having multiple functions I could do the following:
    // public int score = 0;
    // public int MyScore{ get {return score;} set {score = value; UIManager.UpdateScore(score);}
    // I would the call MyScore = some new value (i.e. 1); when this happens the score gets set to 1, and the ui automatically updates as it calls the UIManager.UpdateScore.
    // In this example below we are overriding the function IsComplete that is in the CheckListItem class to do something else in this case,
    // we are returning the bool hasDroppedBall.
    public override bool IsComplete { get { return hasDroppedGun; } }
    /// <summary>
    /// set completion to true when hasdroppedgun returns as true
    /// </summary>
    /// <returns></returns>
    // This function again is returning a float of our progress of this task,
    // in this case when hasDroppedBall is true we return 1, and if it isn't we return 0;
    public override float GetProgress()
    {
        // This is an example of a short hand if statement.
        // here we are checking if our bool of hasDroppedball is true, we do this by using the '?' character as the start of our if.
        // if it is then we are returning 1, ':' in this scenario is the start of our else statement.
        // instead of writing the following we can do it in 1 line, this one line is the same as:
        // if(hasDroppedBall == true)
        // {
        //      return 1;
        // }
        // else
        // {
        //      return 0;
        // }
        ///sets progress to only be 1 or 0, true or false
        return hasDroppedGun ? 1 : 0;
    }

    // This function we are overriding the functionality of the GetStatusReadOut() in the CheckListItem Class.
    // We are returning a string of the value of the bool hasDroppedBall so it will either return "True" or "False";
    public override string GetStatusReadout()
    {
        ///set the HasDroppedGun variable to a string for the GUI
        return hasDroppedGun.ToString();
    }

    // This function we are overriding the functionality of the GetTaskReadout function in the CheckListItemClass.
    // When this function is called in CheckListItemClass we are instead returning the string of "Got away from you huh?".
    public override string GetTaskReadout()
    {
        ///set that as the task text
        return "Thats not how you handle a gun!";
    }

    // This is the function where all the fun stuff happens, it is also the function we need to call when the game event happens
    // i.e. when the ball falls over the edge.
    // This function takes in a type of GameEvents.ObjectResetData which is a class in GameEvents.cs, if you highlight ObjectResetData and press F12 
    // it will take you to it's definition.
    private void GameEvents_OnObjectReset(GameEvents.ObjectResetData data)
    {
        // Here we are checking first if we have not dropped the ball, and that the object that what has entered reset world trigger
        // in this case our ball has a rigidbody and has the take of "Ball".
        if (!hasDroppedGun
            && data.offendingCollider.attachedRigidbody != null &&
            data.offendingCollider.attachedRigidbody.CompareTag("NerfGun"))
        {
            ///check to see if gun has been dropped
            // We can use the keyword var to hold any sort of data, i.e. a var ourData we could put anythin into it, a bool, a string, an int etc.
            // The downside is because it can be everything Unity has to assign the most amount of memory it can to accomodate it, so use var's wisely especially.
            // in big projects. If I didn't want to use a var I could write it as GameEvents.CheckListItemChangedData ourData = new GameEvents.CheckListItemChangedData(); .
            // For this case we are creating a new instance of the class CheckListItemChangedData and storing it in our var  
            var ourData = new GameEvents.CheckListItemChangedData();
            ///make a new game event check list changed data
            // I then take ourData and get the item variable and assign the instance of this class to it, using the 'this' keyword.
            ourData.item = this;
            ///make that our new data
            // From there I can set the previousItemProgress Variable of our data to our current progress in this case it return 1 as we have dropped the ball over the edge.
            // i.e. from 0 being false, to 1 being true, so the previous item progress was it hadn't been dropped over the edge to now it has been dropped over the edge.
            ourData.previousItemProgress = GetProgress();
            ///call the get progress module
            // Since the ball has now gone over the edge we want to se the hasDroppedBall bool to true, so we can't do this function again as the task is complete.
            hasDroppedGun = true;
            ///set hasdroppedgun to true

            // We then tell the game events to invoke the CheckListItemChanged event and pass in our data.
            // this will take our data and up the tasks completed in the UI.
            GameEvents.InvokeCheckListItemChanged(ourData);
            ///set the new data
        }
    }

    // OnEnable is called whenever this script is turned on/off.
    // It is the first function to be called when Unity starts and the script is active.
    // In this case, we want to subscribe our GameEvents_OnObjectReset to the GameEvents.OnObjectReset event.
    // We do this by using the += syntax.
    // This is so when the GameEvents.OnObjectReset event is triggered our function will also get called.
    private void OnEnable()
    {
        GameEvents.OnObjectReset += GameEvents_OnObjectReset;
    }

    // Conversley OnDisable is the last function to be called on a script.
    // In this function we can dictate what happens when we disable this script.
    // In our case we want to unsubscribe from the GameEvents.OnObjectReset Event.
    // If we don't do this, Unity will flip out as it is expecting there to be a function it can call but it can't as
    // we have disabled the script. To unsubscribe from the event we use the -= syntax.
    private void OnDisable()
    {
        GameEvents.OnObjectReset -= GameEvents_OnObjectReset;
    }
}
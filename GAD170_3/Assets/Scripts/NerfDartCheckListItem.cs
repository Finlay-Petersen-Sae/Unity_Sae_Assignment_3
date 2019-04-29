using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NerfDartCheckListItem : CheckListItem
{
    public List<GameObject> Dragons = new List<GameObject>();
    //make a list for dragons
    public int numberOfDragonsHit;
    //interger to find the amount of dragons hit

    public override bool IsComplete { get { return Dragons.Count == 0; } }
    //set the dragons list to 0

    public override float GetProgress()
    {
        return (float)numberOfDragonsHit / (float)Dragons.Count;
        //set the progress by checking the numberOfDragonsHit to the Dragons list
    }

    public override string GetStatusReadout()
    {
        return (numberOfDragonsHit - Dragons.Count) + " / " + numberOfDragonsHit.ToString() ;
        //set the progress to a GUI and update it constantly
    }

    public override string GetTaskReadout()
    {
        return "Shoot some Dragons";
        //Set the task name on the Gui
    }

    public void Start()
    {
        numberOfDragonsHit = Dragons.Count;
        // on start set number of dragons hit to dragons.count
    }

    public void OnDragonHit()
    {
        //on dragon hit run this code
        //Check to see if number of dragons is less then number of dragons in the list
        if (numberOfDragonsHit < Dragons.Count)
        {
            //check the checklistitem changed data
            var ourData = new GameEvents.CheckListItemChangedData();
            //change it to this
            ourData.item = this;
            ourData.previousItemProgress = GetProgress();
            //add 1 to the numberOfDragonsHit
            numberOfDragonsHit++;
            // add the change to the data
            GameEvents.InvokeCheckListItemChanged(ourData);
        }
    }
    private void GameEvents_OnObjectReset(GameEvents.PhysicsInteractionData data)//change on object reset
    {
        Debug.Log("Get To hear?");
        // Here we are checking first if we have not dropped the ball, and that the object that what has entered reset world trigger
        // in this case our ball has a rigidbody and has the take of "Ball".
        if (!IsComplete && data != null && data.other.tag == "NerfDart")// or data.instigator
        {
            Debug.Log("Get To hear");
            // We can use the keyword var to hold any sort of data, i.e. a var ourData we could put anythin into it, a bool, a string, an int etc.
            // The downside is because it can be everything Unity has to assign the most amount of memory it can to accomodate it, so use var's wisely especially.
            // in big projects. If I didn't want to use a var I could write it as GameEvents.CheckListItemChangedData ourData = new GameEvents.CheckListItemChangedData(); .
            // For this case we are creating a new instance of the class CheckListItemChangedData and storing it in our var  
            var ourData = new GameEvents.CheckListItemChangedData();
            // I then take ourData and get the item variable and assign the instance of this class to it, using the 'this' keyword.
            ourData.item = this;
            // From there I can set the previousItemProgress Variable of our data to our current progress in this case it return 1 as we have dropped the ball over the edge.
            // i.e. from 0 being false, to 1 being true, so the previous item progress was it hadn't been dropped over the edge to now it has been dropped over the edge.
            Dragons.Remove(data.instigator);
            ourData.previousItemProgress = GetProgress();
            // Since the ball has now gone over the edge we want to se the hasDroppedBall bool to true, so we can't do this function again as the task is complete.
           
            // We then tell the game events to invoke the CheckListItemChanged event and pass in our data.
            // this will take our data and up the tasks completed in the UI.
            GameEvents.InvokeCheckListItemChanged(ourData);

        }
    }

    // OnEnable is called whenever this script is turned on/off.
    // It is the first function to be called when Unity starts and the script is active.
    // In this case, we want to subscribe our GameEvents_OnObjectReset to the GameEvents.OnObjectReset event.
    // We do this by using the += syntax.
    // This is so when the GameEvents.OnObjectReset event is triggered our function will also get called.
    private void OnEnable()
    {
        GameEvents.OnImportantPhysicsCollision += GameEvents_OnObjectReset;
    }

    // Conversley OnDisable is the last function to be called on a script.
    // In this function we can dictate what happens when we disable this script.
    // In our case we want to unsubscribe from the GameEvents.OnObjectReset Event.
    // If we don't do this, Unity will flip out as it is expecting there to be a function it can call but it can't as
    // we have disabled the script. To unsubscribe from the event we use the -= syntax.
    private void OnDisable()
    {
        GameEvents.OnImportantPhysicsCollision -= GameEvents_OnObjectReset;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks number of shots up to a target amount. Is told directly by Hoop via Unity Event. 
/// </summary>
public class GoalPointCheckListItem : CheckListItem
{
    public int numberOfRequiredCheckpoints;
    //make a int for numberofrequiredcheckpoints
    public int numberOfCheckpointsScored;
    //make an int for numberOfCheckpointsScored

    public override bool IsComplete { get { return numberOfCheckpointsScored == numberOfRequiredCheckpoints; } }
    //this task is complete when the numberOfCheckpointsScored is equal to numberOfRequiredCheckpoints

    public override float GetProgress()
    {
        return (float)numberOfCheckpointsScored / (float)numberOfRequiredCheckpoints;
        // get the progress of the numberOfCheckpointScored off the NumberOfRequiredCheckpoints
    }

    public override string GetStatusReadout()
    {
        return numberOfCheckpointsScored.ToString() + " / " + numberOfRequiredCheckpoints.ToString();
        //set the two ints to string so that it can be used on the GUI
    }

    public override string GetTaskReadout()
    {
        return "Find the Checkpoints";
        //set the Events name on the Gui
    }

    public void OnCheckPointPassed()
    {
        if (numberOfCheckpointsScored < numberOfRequiredCheckpoints)
        {
            //if when a checkpoint is passed a number of checkpoints are scored is less then number of required checkpoints
            var ourData = new GameEvents.CheckListItemChangedData();
            //create a new gameevents checklistitemchangeddata
            ourData.item = this;
            // make the new data this
            ourData.previousItemProgress = GetProgress();
            // get new data progress and set it to ourdata

            numberOfCheckpointsScored++;
            // add one to the numberOfCheckpointsScored to ourData

            GameEvents.InvokeCheckListItemChanged(ourData);
            //Update data to ourData
        }
    }
}

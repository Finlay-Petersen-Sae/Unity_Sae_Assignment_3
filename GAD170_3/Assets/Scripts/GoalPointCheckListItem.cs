using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tracks number of shots up to a target amount. Is told directly by Hoop via Unity Event. 
/// </summary>
public class GoalPointCheckListItem : CheckListItem
{
    public int numberOfRequiredCheckpoints;
    public int numberOfCheckpointsScored;

    public override bool IsComplete { get { return numberOfCheckpointsScored == numberOfRequiredCheckpoints; } }

    public override float GetProgress()
    {
        return (float)numberOfCheckpointsScored / (float)numberOfRequiredCheckpoints;
    }

    public override string GetStatusReadout()
    {
        return numberOfCheckpointsScored.ToString() + " / " + numberOfRequiredCheckpoints.ToString();
    }

    public override string GetTaskReadout()
    {
        return "Find the Checkpoints";
    }

    public void OnCheckPointPassed()
    {
        if (numberOfCheckpointsScored < numberOfRequiredCheckpoints)
        {
            var ourData = new GameEvents.CheckListItemChangedData();
            ourData.item = this;
            ourData.previousItemProgress = GetProgress();

            numberOfCheckpointsScored++;

            GameEvents.InvokeCheckListItemChanged(ourData);
        }
    }
}

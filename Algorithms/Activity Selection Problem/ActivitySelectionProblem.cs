using System;
using System.Collections.Generic;
using System.Linq;

public struct Activity
{
    public int StartTime { get; set; }
    public int FinishTime { get; set; }

    public Activity(int startTime, int finishTime)
    {
        StartTime = startTime;
        FinishTime = finishTime;
    }
}

public static class ActivityScheduler
{
    public static IList<Activity> SelectActivities(IEnumerable<Activity> activities)
    {
        // Sort activities by their finish times in ascending order
        var sortedActivities = activities.OrderBy(a => a.FinishTime).ToList();
        var selectedActivities = new List<Activity>();

        // The end time of the last selected activity
        int lastFinishTime = 0;

        foreach (var activity in sortedActivities)
        {
            // If the start time of the current activity is greater than or equal to the last finish time
            if (activity.StartTime >= lastFinishTime)
            {
                // Select this activity
                selectedActivities.Add(activity);
                lastFinishTime = activity.FinishTime; // Update the last finish time
            }
        }

        return selectedActivities;
    }
}
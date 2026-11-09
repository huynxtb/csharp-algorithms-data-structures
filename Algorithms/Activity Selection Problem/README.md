# Activity Selection Problem

## Introduction
The Activity Selection Problem is a classic greedy algorithm problem. Given a set of activities, each defined by a start and finish time, the goal is to select the maximum number of non-overlapping activities that can be performed by a single resource. This problem is commonly encountered in scheduling applications.

## Usage
To use the Activity Selection Problem implementation, create a list of `Activity` objects and call the `SelectActivities` method from the `ActivityScheduler` class. Here is an example:

```csharp
var activities = new List<Activity>
{
    new Activity(1, 3),
    new Activity(2, 5),
    new Activity(4, 6),
    new Activity(6, 7),
    new Activity(5, 9),
    new Activity(8, 9)
};

var selectedActivities = ActivityScheduler.SelectActivities(activities);
```

## Detailed Explanation
1. **Sorting**: The input activities are sorted by their finish times in ascending order. This is crucial for the greedy approach, as it allows us to always consider the next activity that finishes the earliest.
2. **Selection**: We initialize an empty list to hold the selected activities and a variable to track the finish time of the last selected activity. We iterate through the sorted activities and select an activity if its start time is greater than or equal to the finish time of the last selected activity.
3. **Return**: The method returns a list of selected activities that can be performed without overlapping.

## Complexity Analysis
- **Time Complexity**: The sorting step takes O(N log N) time, where N is the number of activities. The selection step takes O(N) time. Therefore, the overall time complexity is O(N log N).
- **Space Complexity**: The space complexity is O(N) due to the storage of the selected activities list.
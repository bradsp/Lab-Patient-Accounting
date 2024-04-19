/*
 USAGE
  
 * You now can filter any event log with a few simple lines of code. 
 * The code below retrieves all the entries in the "System" event log and then applies two filters to the results. 
 * Finally, the filtered entries are displayed on the console.

EventLog Log = new EventLog("System");
EventLogEntry[] Entries =
   SearchEventLog.FilterEventLog(EventLogFilterType.TimeGenerated,
      Log.Entries, DateTime.Parse("1/1/2009"),
      DateTime.Parse("1/31/2009"));
   Entries = SearchEventLog.FilterEventLog(EventLogFilterType.EntryType,
   Entries, EventLogEntryType.Error);

foreach (EventLogEntry Entry in Entries)
{
    Console.WriteLine(" Message: "   + Entry.Message);
    Console.WriteLine(" Category: "  + Entry.Category);
    Console.WriteLine(" EntryType: " + Entry.EntryType.ToString());
    Console.WriteLine(" Source: "    + Entry.Source);
}

 
//Here is a sample of the output from the above example when run on my computer:

Message: The time provider NtpClient is configured to acquire time
         from one or more time sources, however none of the
         sources are currently accessible. No attempt to contact
         a source will be made for 14 minutes.
         NtpClient has no source of accurate time.
Category: (0)
EntryType: Error
Source: W32Time

Message: DCOM was unable to communicate with the computer
         TRSBETASQL using any of the configured protocols.
Category: (0)
EntryType: Error
Source: DCOM

 */
using System;
// programmer added
using System.Collections;
using System.Diagnostics;
/// <summary>
/// List of possible fields that can be used to filter the event log entries
/// wdk 20090211
/// </summary>
public enum EventLogFilterType
{
    /// <summary>
    /// Datetime
    /// </summary>
    TimeGenerated,
    /// <summary>
    /// user
    /// </summary>
    UserName,
    /// <summary>
    /// machine
    /// </summary>
    MachineName,
    /// <summary>
    /// category
    /// </summary>
    Category,
    /// <summary>
    /// source
    /// </summary>
    Source,
    /// <summary>
    /// entry type
    /// </summary>
    EntryType,
    /// <summary>
    /// message
    /// </summary>
    Message,
    /// <summary>
    /// event id
    /// </summary>
    EventID
}

namespace Utilities;

/// <summary>
/// Class allows searching of the event entry logs. It has two static methods that can be called.
/// 1. FilterEventLog (EventLogFilterType FilterType,IEnumerable Entries, object Criteria1, object Criteria2)
///     which uses the TimeGenerated enum to cast the Criteria1, and Criteria2 into Datetimes for searching by datetime range
/// 2.  FilterEventLog(EventLogFilterType FilterType, IEnumerable Entries, object Criteria)
///     which uses the enum to filter the log by the type passed in.
/// </summary>
public sealed class SearchEventLog
{
    // Prevent this class from being instantiated.
    private SearchEventLog() { }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="FilterType"></param>
    /// <param name="Entries"></param>
    /// <param name="Criteria1"></param>
    /// <param name="Criteria2"></param>
    /// <returns></returns>
    public static EventLogEntry[]

       FilterEventLog(EventLogFilterType FilterType,
                      IEnumerable Entries, object Criteria1,
                      object Criteria2)
    {
        ArrayList FilteredEntries = new ArrayList();
        foreach (EventLogEntry Entry in Entries)
        {
            switch (FilterType)
            {
                case EventLogFilterType.TimeGenerated:
                    if (Entry.TimeGenerated >= (DateTime)Criteria1 &&
                        Entry.TimeGenerated <= (DateTime)Criteria2)
                        FilteredEntries.Add(Entry);
                    break;
            }
        }
        EventLogEntry[] EntriesArray =
           new EventLogEntry[FilteredEntries.Count];
        FilteredEntries.CopyTo(EntriesArray);
        return (EntriesArray);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="FilterType"></param>
    /// <param name="Entries"></param>
    /// <param name="Criteria"></param>
    /// <returns></returns>
    public static EventLogEntry[]
       FilterEventLog(EventLogFilterType FilterType,
                      IEnumerable Entries, object Criteria)
    {
        ArrayList FilteredEntries = new ArrayList();
        foreach (EventLogEntry Entry in Entries)
        {
            switch (FilterType)
            {
                case EventLogFilterType.Category:
                    if (Entry.Category == (string)Criteria)
                        FilteredEntries.Add(Entry);
                    break;
                case EventLogFilterType.EntryType:
                    if (Entry.EntryType == (EventLogEntryType)Criteria)
                        FilteredEntries.Add(Entry);
                    break;
                case EventLogFilterType.EventID:
                    if (Entry.InstanceId == (int)Criteria)
                        FilteredEntries.Add(Entry);
                    break;
                case EventLogFilterType.MachineName:
                    if (Entry.MachineName == (string)Criteria)
                        FilteredEntries.Add(Entry);
                    break;
                case EventLogFilterType.Message:
                    if (Entry.Message == (string)Criteria)
                        FilteredEntries.Add(Entry);
                    break;
                case EventLogFilterType.Source:
                    if (Entry.Source == (string)Criteria)
                        FilteredEntries.Add(Entry);
                    break;
                case EventLogFilterType.UserName:
                    if (Entry.UserName == (string)Criteria)
                        FilteredEntries.Add(Entry);
                    break;
            }
        }
        EventLogEntry[] EntriesArray =
           new EventLogEntry[FilteredEntries.Count];
        FilteredEntries.CopyTo(EntriesArray);
        return (EntriesArray);
    }
}

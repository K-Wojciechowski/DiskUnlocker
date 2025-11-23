using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DiskUnlocker;

public static class EventLogReader {
  public static IEnumerable<EjectionEvent> GetEjectionEvents() {
    EventLog log = GetSystemLog();

    for (int i = log.Entries.Count - 1; i >= 0; i--) {
      var entry = log.Entries[i];
      if (entry.Source == "Microsoft-Windows-Kernel-PnP" && entry.InstanceId == 225 && entry.CategoryNumber == 223 && entry.IsRecent()) {
        yield return new EjectionEvent(entry);
      }
    }
  }

  public static async Task<IEnumerable<EjectionEvent>> GetEjectionEventsAsync() => await Task.Run(GetEjectionEvents).ConfigureAwait(false);

  private static EventLog GetSystemLog() {
    EventLog[] logs = EventLog.GetEventLogs();
    return logs.Where(l => l.Log == "System").First();
  }

  private static bool IsRecent(this EventLogEntry e) {
    var cutoff = DateTime.Now.AddMinutes(-10);
    var diff = DateTime.Compare(e.TimeGenerated, cutoff);
    return diff >= 0;
  }
}

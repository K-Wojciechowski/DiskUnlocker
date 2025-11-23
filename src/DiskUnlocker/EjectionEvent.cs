using System;
using System.Diagnostics;

namespace DiskUnlocker;

public class EjectionEvent {
  public int Pid { get; }
  public string ProcessName { get; }
  public string Device { get; }
  public string Message { get; }
  public DateTime TimeGenerated { get; }

  public bool CanBeKilled => !ProcessName.Equals("System", StringComparison.OrdinalIgnoreCase);

  public EjectionEvent(EventLogEntry e) {
    Pid = int.Parse(e.ReplacementStrings[0]);
    ProcessName = e.ReplacementStrings[2];
    Device = e.ReplacementStrings[4];
    Message = e.Message;
    TimeGenerated = e.TimeGenerated;
  }
}

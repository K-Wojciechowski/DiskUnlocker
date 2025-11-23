using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiskUnlocker;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {
  private readonly ObservableCollection<EjectionEvent> events = new ObservableCollection<EjectionEvent>();
  public MainWindow() {
    InitializeComponent();
    EventDataGrid.ItemsSource = events;
  }

  private async void WindowLoaded(object sender, RoutedEventArgs e) {
    await Task.Yield();
    var foundEvents = (await EventLogReader.GetEjectionEventsAsync().ConfigureAwait(true))
      .OrderByDescending(x => x.TimeGenerated)
      .ToList();

    if (foundEvents.Any()) {
      Header.Content = foundEvents.Count == 1 ? "Found 1 event" : $"Found {foundEvents.Count} events";
      foundEvents.ForEach(events.Add);
    } else {
      Header.Content = "No events found.";
      await Task.Delay(2000).ConfigureAwait(true);
      Application.Current.Shutdown();
    }
  }

  private void KillProcess(object sender, RoutedEventArgs e) {
    for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual) {
      if (vis is DataGridRow row && row.Item is EjectionEvent ee) {
        if (!ee.CanBeKilled) {
          MessageBox.Show("This process cannot be killed.", "Cannot kill SYSTEM", MessageBoxButton.OK, MessageBoxImage.Warning);
          return;
        }

        try {
          Process.GetProcessById(ee.Pid).Kill();
        } catch (Exception ex) {
          MessageBox.Show(ex.ToString(), $"Killing process {ee.Pid} failed", MessageBoxButton.OK, MessageBoxImage.Error);
          return;
        }
        events.Remove(ee);
      }
    }
  }
}

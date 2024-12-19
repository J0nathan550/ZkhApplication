using System.Windows.Controls;

namespace ZkhApplication.Views;

public partial class NavigationPageView : UserControl
{
    public NavigationPageView()
    {
        InitializeComponent();
    }

    private void AddSubmissionNavigation_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        MainWindowView.Instance.NavigationPageViewControl.Visibility = System.Windows.Visibility.Collapsed;
        MainWindowView.Instance.CreateSubmissionViewControl.Visibility = System.Windows.Visibility.Visible;
        MainWindowView.Instance.ViewSubmissionViewControl.Visibility = System.Windows.Visibility.Collapsed;
    }

    private void ViewSubmissionNavigation_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        MainWindowView.Instance.NavigationPageViewControl.Visibility = System.Windows.Visibility.Collapsed;
        MainWindowView.Instance.CreateSubmissionViewControl.Visibility = System.Windows.Visibility.Collapsed;
        MainWindowView.Instance.ViewSubmissionViewControl.Visibility = System.Windows.Visibility.Visible;
    }
}
using System.Windows.Controls;
using ZkhApplication.ViewModels;

namespace ZkhApplication.Views;

public partial class CreateSubmissionView : UserControl
{
    public CreateSubmissionView()
    {
        DataContext = new CreateSubmissionViewModel();
        InitializeComponent();
    }

    private void GoBackToMain_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        MainWindowView.Instance.NavigationPageViewControl.Visibility = System.Windows.Visibility.Visible;
        MainWindowView.Instance.CreateSubmissionViewControl.Visibility = System.Windows.Visibility.Collapsed;
        MainWindowView.Instance.ViewSubmissionViewControl.Visibility = System.Windows.Visibility.Collapsed;
    }
}
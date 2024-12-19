using CommunityToolkit.Mvvm.Input;
using ModernWpf.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ZkhApplication.Models;

public partial class Submission
{
    public long SubmissionId { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string ThemeOfReport { get; set; }
    public string ImageLink { get; set; }
    public string SmallDescription { get; set; }
    public string StatusType { get; set; }

    [RelayCommand]
    private void MakeFullScreenImage()
    {
        ContentDialog contentDialog = new()
        {
            Background = new SolidColorBrush(Colors.Transparent)
        };

        Grid dialogGrid = new();
        dialogGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        dialogGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        Image image = new()
        {
            Source = new BitmapImage(new Uri(ImageLink)),
            Stretch = Stretch.Uniform,
        };
        RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.Fant);
        Grid.SetRow(image, 0);
        dialogGrid.Children.Add(image);

        Button actionButton = new()
        {
            Content = "Зачинити",
            Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x14, 0x4C, 0x6F)),
            Margin = new Thickness(0,5,0,5),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Cursor = System.Windows.Input.Cursors.Hand
        };
        actionButton.Click += (sender, e) => contentDialog.Hide();
        Grid.SetRow(actionButton, 1);
        dialogGrid.Children.Add(actionButton);
        contentDialog.Content = dialogGrid;
        contentDialog.BorderThickness = new Thickness(0);
        contentDialog.IsShadowEnabled = false;
        contentDialog.ShowAsync();
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using ZkhApplication.Database;
using ZkhApplication.Models;
using ZkhApplication.Views;

namespace ZkhApplication.ViewModels;

public partial class CreateSubmissionViewModel : ObservableObject
{
    [ObservableProperty]
    private string _addressText;
    [ObservableProperty]
    private string _phoneNumberText;
    [ObservableProperty]
    private string _statusOfReportText;
    [ObservableProperty]
    private string _smallDescriptionText;
    [ObservableProperty]
    private string _imagePath;
    [ObservableProperty]
    private Visibility _addressHeaderVisibility = Visibility.Collapsed;
    [ObservableProperty]
    private Visibility _phoneNumberHeaderNullVisibility = Visibility.Collapsed;
    [ObservableProperty]
    private Visibility _phoneNumberHeaderRegexVisibility = Visibility.Collapsed;
    [ObservableProperty]
    private Visibility _statusOfReportHeaderVisibility = Visibility.Collapsed;
    [ObservableProperty]
    private Visibility _smallDescriptionHeaderVisibility = Visibility.Collapsed;
    [ObservableProperty]
    private Visibility _textImageVisibility = Visibility.Visible;

    [RelayCommand]
    private void SelectImage()
    {
        OpenFileDialog openFileDialog = new()
        {
            Title = "Оберіть зображення",
            Filter = "Image Files (*.png;*.jpg;*.jpeg;*.bmp)|*.png;*.jpg;*.jpeg;*.bmp",
            Multiselect = false
        };

        if (openFileDialog.ShowDialog() == true)
        {
            string selectedFilePath = openFileDialog.FileName;
            ImagePath = selectedFilePath;
            if (File.Exists(ImagePath))
            {
                TextImageVisibility = Visibility.Hidden;
            }
            else
            {
                ImagePath = null;
                TextImageVisibility = Visibility.Visible;
            }
        }
    }

    [RelayCommand]
    private void SubmitReport()
    {
        bool hasError = false;

        if (string.IsNullOrWhiteSpace(AddressText))
        {
            AddressHeaderVisibility = Visibility.Visible;
            hasError = true;
        }
        else
        {
            AddressHeaderVisibility = Visibility.Collapsed;
        }

        if (string.IsNullOrWhiteSpace(PhoneNumberText) || !IsValidPhoneNumber(PhoneNumberText))
        {
            PhoneNumberHeaderRegexVisibility = Visibility.Visible;
            hasError = true;
        }
        else
        {
            PhoneNumberHeaderRegexVisibility = Visibility.Collapsed;
        }

        if (string.IsNullOrWhiteSpace(StatusOfReportText))
        {
            StatusOfReportHeaderVisibility = Visibility.Visible;
            hasError = true;
        }
        else
        {
            StatusOfReportHeaderVisibility = Visibility.Collapsed;
        }

        if (string.IsNullOrWhiteSpace(SmallDescriptionText))
        {
            SmallDescriptionHeaderVisibility = Visibility.Visible;
            hasError = true;
        }
        else
        {
            SmallDescriptionHeaderVisibility = Visibility.Collapsed;
        }

        if (string.IsNullOrWhiteSpace(ImagePath))
        {
            MessageBox.Show("Будь ласка, виберіть зображення.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            hasError = true;
        }

        if (hasError)
        {
            return;
        }

        Submission submission = new()
        {
            SubmissionId = SaveSystem.SubmissionData.Submissions.Count + 1,
            Address = AddressText,
            PhoneNumber = PhoneNumberText,
            ThemeOfReport = StatusOfReportText,
            SmallDescription = SmallDescriptionText,
            ImageLink = ImagePath,
            StatusType = "В обробці"
        };
        SaveSystem.SubmissionData.Submissions.Add(submission);
        SaveSystem.Save();

        MessageBox.Show($"Ваша заявка №{submission.SubmissionId} успішно створена!", "Успіх!", MessageBoxButton.OK, MessageBoxImage.Information);
        
        AddressText = string.Empty;
        PhoneNumberText = string.Empty;
        StatusOfReportText = string.Empty;
        SmallDescriptionText = string.Empty;
        ImagePath = null;
        AddressHeaderVisibility = Visibility.Collapsed;
        PhoneNumberHeaderNullVisibility = Visibility.Collapsed;
        PhoneNumberHeaderRegexVisibility = Visibility.Collapsed;
        StatusOfReportHeaderVisibility = Visibility.Collapsed;
        SmallDescriptionHeaderVisibility = Visibility.Collapsed;
        TextImageVisibility = Visibility.Visible;


        MainWindowView.Instance.NavigationPageViewControl.Visibility = Visibility.Visible;
        MainWindowView.Instance.CreateSubmissionViewControl.Visibility = Visibility.Collapsed;
        MainWindowView.Instance.ViewSubmissionViewControl.Visibility = Visibility.Collapsed;

    }

    private readonly Regex phoneRegex = new(@"^\+38[\d]{7,10}$", RegexOptions.Compiled);
    private bool IsValidPhoneNumber(string phoneNumber)
    {
        return phoneRegex.IsMatch(phoneNumber);
    }
}
using ModernWpf.Controls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ZkhApplication.Models;
using ZkhApplication.ViewModels;

namespace ZkhApplication.Views
{
    public partial class ViewSubmissionView : UserControl
    {
        private readonly ViewSubmissionViewModel viewSubmissionViewModel = new();

        public ViewSubmissionView()
        {
            DataContext = viewSubmissionViewModel;
            InitializeComponent();
            WarningNoSubmissions.Visibility = Visibility.Collapsed;
        }

        private void GoBackToMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindowView.Instance.NavigationPageViewControl.Visibility = Visibility.Visible;
            MainWindowView.Instance.CreateSubmissionViewControl.Visibility = Visibility.Collapsed;
            MainWindowView.Instance.ViewSubmissionViewControl.Visibility = Visibility.Collapsed;
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            viewSubmissionViewModel.LoadSubmissions();
            UpdateWarningVisibility();
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                string userInput = sender.Text.ToLower();

                List<string> filteredSuggestions = viewSubmissionViewModel.FullSubmissions
                    .Where(sub =>
                        sub.Address.ToLower().Contains(userInput) ||
                        sub.SubmissionId.ToString().Contains(userInput))
                    .Select(sub => $"Заявка: {sub.SubmissionId}, Адреса: {sub.Address}")
                    .ToList();

                if (filteredSuggestions.Count == 0)
                {
                    filteredSuggestions.Add("Знайдено жодної заявки або адреси за вашим запитом.");
                }

                sender.ItemsSource = filteredSuggestions;
            }
            else if (string.IsNullOrEmpty(sender.Text))
            {
                viewSubmissionViewModel.ReconstructSubmissions();
                UpdateWarningVisibility();
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (args.SelectedItem != null)
            {
                string selectedSuggestion = args.SelectedItem.ToString();
                long? submissionId = null;

                if (selectedSuggestion.StartsWith("Заявка:"))
                {
                    string[] parts = selectedSuggestion.Split(',');
                    if (parts.Length > 0)
                    {
                        string idPart = parts[0].Replace("Заявка:", "").Trim();
                        if (long.TryParse(idPart, out long parsedId))
                        {
                            submissionId = parsedId;
                        }
                    }
                }

                if (submissionId.HasValue)
                {
                    viewSubmissionViewModel.FilterSubmissionsById(submissionId.Value);
                }
                UpdateWarningVisibility();
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (string.IsNullOrWhiteSpace(args.QueryText))
            {
                viewSubmissionViewModel.ReconstructSubmissions();
                UpdateWarningVisibility();
                return;
            }
            long submissionId = -1;
            if (args.QueryText.StartsWith("Заявка: "))
            {
                int pos = args.QueryText.IndexOf(',');
                string subId = args.QueryText.Substring(8, pos - 8);
                if (!long.TryParse(subId, out submissionId))
                {
                    submissionId = -1;
                }
            }
            else
            {
                if (!long.TryParse(args.QueryText, out submissionId))
                {
                    submissionId = -1;
                }
            }

            IEnumerable<Submission> filteredSubmissions = viewSubmissionViewModel.FullSubmissions.Where(sub => sub.Address.Contains(args.QueryText, StringComparison.OrdinalIgnoreCase) || sub.SubmissionId == submissionId);
            viewSubmissionViewModel.Submissions = new ObservableCollection<Submission>(filteredSubmissions);
            if (filteredSubmissions == null)
            {
                viewSubmissionViewModel.ClearSubmissions();
            }
            UpdateWarningVisibility();
        }

        private void UpdateWarningVisibility()
        {
            WarningNoSubmissions.Visibility = viewSubmissionViewModel.Submissions.Count == 0
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}
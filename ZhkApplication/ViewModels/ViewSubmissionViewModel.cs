using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using ZkhApplication.Database;
using ZkhApplication.Models;

namespace ZkhApplication.ViewModels;

public partial class ViewSubmissionViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Submission> submissions = [];
    [ObservableProperty]
    private ObservableCollection<Submission> fullSubmissions = [];

    public void LoadSubmissions()
    {
        SaveSystem.Load();
        FullSubmissions = new ObservableCollection<Submission>(SaveSystem.SubmissionData.Submissions);
        ReconstructSubmissions();
    }

    public void ReconstructSubmissions()
    {
        Submissions.Clear();

        foreach (Submission submission in FullSubmissions.OrderByDescending(sub => sub.SubmissionId))
        {
            Submissions.Add(submission);
        }
    }

    public void FilterSubmissionsById(long submissionId)
    {
        Submission matchingSubmission = FullSubmissions.FirstOrDefault(sub => sub.SubmissionId == submissionId);
        Submissions.Clear();
        if (matchingSubmission != null)
        {
            Submissions.Add(matchingSubmission);
        }
    }

    public void ClearSubmissions()
    {
        Submissions.Clear();
    }
}
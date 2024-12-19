using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using ZkhApplication.Models;

namespace ZkhApplication.Database;

public static class SaveSystem
{
    private static SubmissionDatabase submissionDatabase = new();
    public static SubmissionDatabase SubmissionData { get => submissionDatabase; set => submissionDatabase = value; }

    public static void Load()
    {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        path = Path.Combine(path, "ZkhApplication", "ZkhApplication.data");
        if (File.Exists(path))
        {
            string data = File.ReadAllText(path);
            SubmissionDatabase ud = JsonSerializer.Deserialize<SubmissionDatabase>(data);
            if (ud != null) { submissionDatabase = ud; }
        }
    }

    public static void Save()
    {
        string info = JsonSerializer.Serialize(submissionDatabase);
        string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        path = Path.Combine(path, "ZkhApplication");
        Directory.CreateDirectory(path);
        path = Path.Combine(path, "ZkhApplication.data");
        File.WriteAllText(path, info);
    }
}

public class SubmissionDatabase
{
    public ObservableCollection<Submission> Submissions { get; set; } = [];
}
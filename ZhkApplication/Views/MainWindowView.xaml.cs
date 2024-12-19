using System.Windows;
using ZkhApplication.Database;

namespace ZkhApplication.Views;

public partial class MainWindowView : Window
{
    public static MainWindowView Instance { get; private set; }

    public MainWindowView()
    {
        InitializeComponent();
        SaveSystem.Load();
        Instance = this;
    }
}
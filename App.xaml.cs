using System.Configuration;
using System.Data;
using System.Windows;

namespace MunicipalServicesApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Global flag to track admin login status
        public static bool IsAdminLoggedIn = false;

        // Custom linked list shared across all windows
        public IssueLinkedList IssueList { get; } = new IssueLinkedList();
    }
}

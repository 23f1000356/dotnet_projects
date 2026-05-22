namespace PracticeFA.App.Models;

/// <summary>
/// Lightweight host context so EmployeeSearchBox can bind on different parents (P34).
/// </summary>
public sealed class SearchHostContext
{
    public SearchHostContext(string searchBadge, string hostName)
    {
        SearchBadge = searchBadge;
        HostName = hostName;
    }

    public string SearchBadge { get; set; }

    public string HostName { get; set; }
}

using AcciCom.Models;

namespace AcciCom.Services
{
    // Services/IEventHubService.cs
    public interface IEventHubService
    {
        Task SendAccidentReportAsync(AccidentReport report);
    }
}

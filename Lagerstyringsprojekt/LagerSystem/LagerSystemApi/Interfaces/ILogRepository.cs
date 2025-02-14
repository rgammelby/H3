namespace LagerSystemApi.Interfaces
{
    public interface ILogRepository
    {
        Task<List<Log>> GetAllLogs();
    }
}

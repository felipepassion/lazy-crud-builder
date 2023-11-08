
namespace ToolSvcData.Access
{
    public interface ISqlDataAccess
    {
        Task<int> InsertData<T>(string storedProcedure, T parameters, string connectionStringName);
        Task<List<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionStringName);
        Task SaveData<T>(string storedProcedure, T parameters, string connectionStringName);
    }
}
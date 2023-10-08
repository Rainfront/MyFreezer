using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MyFreezer.API.Repositories;

public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        var temp = _configuration.GetConnectionString("DefaultConnection");
        _connectionString = temp;
    }
    public IDbConnection CreateConnection()
        => new SqlConnection(_connectionString);
}
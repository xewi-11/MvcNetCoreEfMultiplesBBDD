using MvcNetCoreEfMultiplesBBDD.Models;

namespace MvcNetCoreEfMultiplesBBDD.Repositories
{
    public interface IRepositoryEmpleados
    {
        Task<List<V_Empleado>> GetEmpleadosAsync();
        Task<V_Empleado> GetDetailsEmpleadoByIdAsync(int emp_no);
        Task<List<string>> GetOficiosAsync();
        Task<List<V_Empleado>> GetEmpleadosPorOficioAsync(string oficio);
        Task<int> InsertEmpleadoAsync(string apellido, string oficio, int director, int salario, int comision, string departamento);
    }
}

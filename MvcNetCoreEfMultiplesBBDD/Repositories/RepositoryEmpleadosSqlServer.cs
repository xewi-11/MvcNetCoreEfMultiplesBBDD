using Microsoft.EntityFrameworkCore;
using MvcNetCoreEfMultiplesBBDD.Data;
using MvcNetCoreEfMultiplesBBDD.Models;

namespace MvcNetCoreEfMultiplesBBDD.Repositories
{
    #region  STORED PROCEDUREs Y VISTAS





    // sql

    //    create view V_EMPLEADOS
    //as
    //   select Cast(isnull(ROW_NUMBER() over (order by EMP.APELLIDO),0)as int) as ID ,
    //   EMP.EMP_NO,EMP.APELLIDO,EMP.OFICIO,EMP.SALARIO,
    //   DEPT.DEPT_NO as IDDEPARTAMENTO,
    //   DEPT.DNOMBRE as DEPARTAMENTO, DEPT.LOC as LOCALIDAD
    //   from EMP
    //   inner join DEPT on EMP.DEPT_NO=DEPT.DEPT_NO
    //go

    //    create procedure SP_ALL_VEMPLEADOS
    //as
    //   select* from V_EMPLEADOS
    //go

    #endregion
    public class RepositoryEmpleadosSqlServer : IRepositoryEmpleados
    {
        private HospitalContext context;
        public RepositoryEmpleadosSqlServer(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<V_Empleado>> GetEmpleadosAsync()
        {
            string sql = "SP_ALL_VEMPLEADOS";
            var consulta = this.context.VistaEmpleados.FromSqlRaw(sql);
            List<V_Empleado> empleados = await consulta.ToListAsync();
            return empleados;
        }

        public async Task<List<string>> GetOficiosAsync()
        {
            var consulta = (from datos in this.context.VistaEmpleados
                            select datos.Oficio).Distinct();

            return await consulta.ToListAsync();
        }
        public async Task<List<V_Empleado>> GetEmpleadosPorOficioAsync(string oficio)
        {
            var consulta = from datos in this.context.VistaEmpleados
                           where datos.Oficio == oficio
                           select datos;
            return await consulta.ToListAsync();
        }
        public async Task<V_Empleado> GetDetailsEmpleadoByIdAsync(int emp_no)
        {
            var consulta = from datos in this.context.VistaEmpleados
                           where datos.Emp_No == emp_no
                           select datos;
            return await consulta.FirstAsync();
        }



    }
}
using Microsoft.EntityFrameworkCore;
using MvcNetCoreEfMultiplesBBDD.Data;
using MvcNetCoreEfMultiplesBBDD.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace MvcNetCoreEfMultiplesBBDD.Repositories
{
    #region STORED PROCEDURES Y VISTAS

    //oracle 

    //    CREATE OR REPLACE VIEW V_EMPLEADOS
    //AS
    //  select
    //   EMP.EMP_NO, EMP.APELLIDO, EMP.OFICIO, EMP.SALARIO,
    //   DEPT.DEPT_NO as IDDEPARTAMENTO,
    //   DEPT.DNOMBRE as DEPARTAMENTO, DEPT.LOC as LOCALIDAD
    //   from EMP
    //   inner join DEPT on EMP.DEPT_NO= DEPT.DEPT_NO;



    //    create or replace procedure  SP_ALL_VEMPLEADOS
    //(p_cursor_empleados out SYS_REFCURSOR)
    //as 
    //BEGIN
    //  open p_cursor_empleados for select* from V_EMPLEADOS;
    //end;

    #endregion
    public class RepositoryEmpleadosOracle : IRepositoryEmpleados
    {
        private HospitalContext context;
        public RepositoryEmpleadosOracle(HospitalContext context)
        {
            this.context = context;
        }



        public async Task<List<V_Empleado>> GetEmpleadosAsync()
        {
            string sql = "BEGIN ";
            sql += " SP_ALL_VEMPLEADOS (:p_cursor_empleados); ";
            sql += " END;";
            OracleParameter pamCursor = new OracleParameter();
            pamCursor.ParameterName = "p_cursor_empleados";
            pamCursor.Value = null;
            pamCursor.Direction = ParameterDirection.Output;
            //INDICAMOS EL TIPO DE ORACLE
            pamCursor.OracleDbType = OracleDbType.RefCursor;

            var consulta = this.context.VistaEmpleados.FromSqlRaw(sql, pamCursor);
            return await consulta.ToListAsync();
        }
        public async Task<V_Empleado> GetDetailsEmpleadoByIdAsync(int emp_no)
        {
            var consulta = from datos in this.context.VistaEmpleados
                           where datos.Emp_No == emp_no
                           select datos;
            return await consulta.FirstAsync();
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
    }
}

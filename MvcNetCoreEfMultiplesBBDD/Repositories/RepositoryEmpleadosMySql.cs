using Microsoft.EntityFrameworkCore;
using MvcNetCoreEfMultiplesBBDD.Data;
using MvcNetCoreEfMultiplesBBDD.Models;
using System.Data;

namespace MvcNetCoreEfMultiplesBBDD.Repositories
{
    public class RepositoryEmpleadosMySql : IRepositoryEmpleados
    {
        #region VISTAS

        //        create or replace view V_EMPLEADOS
        //as
        //    select
        //      EMP.EMP_NO, EMP.APELLIDO, EMP.OFICIO, EMP.SALARIO,
        //       DEPT.DEPT_NO as IDDEPARTAMENTO,
        //       DEPT.DNOMBRE as DEPARTAMENTO, DEPT.LOC as LOCALIDAD
        //       from EMP
        //       inner join DEPT on EMP.DEPT_NO= DEPT.DEPT_NO;

        //        DELIMITER $$

        //CREATE PROCEDURE SP_ALL_V_EMPLEADOS()
        //BEGIN
        //    SELECT* FROM V_EMPLEADOS;
        //END $$

        //DELIMITER ;


        #endregion
        private HospitalContext context;
        public RepositoryEmpleadosMySql(HospitalContext context)
        {
            this.context = context;
        }



        public async Task<List<V_Empleado>> GetEmpleadosAsync()
        {
            //EF
            string sql = "CALL SP_ALL_V_EMPLEADOS";

            var consulta = this.context.VistaEmpleados.FromSqlRaw(sql);

            //Linq
            //var consulta2 = from datos in this.context.VistaEmpleados select datos;
            //return await consulta.ToListAsync();

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

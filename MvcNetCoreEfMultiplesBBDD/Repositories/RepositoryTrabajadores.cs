using Microsoft.EntityFrameworkCore;
using MvcNetCoreEfMultiplesBBDD.Data;
using MvcNetCoreEfMultiplesBBDD.Models;

namespace MvcNetCoreEfMultiplesBBDD.Repositories
{
    #region  STORED PROCEDUREs


    //oracle 

    //    CREATE OR REPLACE VIEW V_EMPLEADOS
    //AS
    //  select
    //   EMP.EMP_NO, EMP.APELLIDO, EMP.OFICIO, EMP.SALARIO,
    //   DEPT.DEPT_NO as IDDEPARTAMENTO,
    //   DEPT.DNOMBRE as DEPARTAMENTO, DEPT.LOC as LOCALIDAD
    //   from EMP
    //   inner join DEPT on EMP.DEPT_NO= DEPT.DEPT_NO;

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

    #endregion
    public class RepositoryTrabajadores
    {
        private HospitalContext context;
        public RepositoryTrabajadores(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<V_Empleado>> GetEmpleadosAsync()
        {
            var consulta = from datos in this.context.VistaEmpleados
                           select datos;
            return await consulta.ToListAsync();
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
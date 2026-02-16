using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreEfMultiplesBBDD.Data;
using MvcNetCoreEfMultiplesBBDD.Models;
using System.Data;

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

    //    create procedure SP_INSERT_EMP
    //(@apellido nvarchar(50),@oficio nvarchar(50),@dir int,@salario int, @comision int, @departamento nvarchar(50),@emp_no int out)
    //as
    //  declare @idUsuario int
    //  declare @dept_no int
    //  declare @fecha_alt dateTime

    //  select @idUsuario = (select CAST(MAX(EMP_NO) as INT) from EMP) +1;
    //  select @dept_no = (select DEPT_NO from DEPT where DNOMBRE=@departamento);
    //  select @fecha_alt = GETDATE();

    //    insert into EMP values(@idUsuario, @apellido, @oficio, @dir, @fecha_alt, @salario, @comision, @dept_no);

    //    SET @emp_no = @idUsuario;
    //    go

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
            return await consulta.FirstOrDefaultAsync();
        }
        public async Task<int> InsertEmpleadoAsync(string apellido, string oficio, int director, int salario, int comision, string departamento)
        {
            string sql = "SP_INSERT_EMP @apellido, @oficio, @dir, @salario, @comision, @departamento, @emp_no OUTPUT";
            SqlParameter pamApellido = new SqlParameter("@apellido", apellido);
            SqlParameter pamOficio = new SqlParameter("@oficio", oficio);
            SqlParameter pamDir = new SqlParameter("@dir", director);
            SqlParameter pamSalario = new SqlParameter("@salario", salario);
            SqlParameter pamComision = new SqlParameter("@comision", comision);
            SqlParameter pamDepart = new SqlParameter("@departamento", departamento);

            // 🔹 Parámetro de salida
            SqlParameter pamEmpNo = new SqlParameter("@emp_no", SqlDbType.Int);
            pamEmpNo.Direction = ParameterDirection.Output;

            await this.context.Database.ExecuteSqlRawAsync(
                sql,
                pamApellido,
                pamOficio,
                pamDir,
                pamSalario,
                pamComision,
                pamDepart,
                pamEmpNo
                );

            // 🔹 Recuperar el valor
            return (int)pamEmpNo.Value;

        }


    }
}
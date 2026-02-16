using Microsoft.EntityFrameworkCore;
using MvcNetCoreEfMultiplesBBDD.Data;
using MvcNetCoreEfMultiplesBBDD.Models;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
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

    //    CREATE OR REPLACE PROCEDURE SP_INSERT_EMP
    //(
    //  p_apellido IN  EMP.APELLIDO%TYPE,
    //  p_oficio IN  EMP.OFICIO%TYPE,
    //  p_dir IN  EMP.DIR%TYPE,
    //  p_salario IN  EMP.SALARIO%TYPE,
    //  p_comision IN  EMP.COMISION%TYPE,
    //  p_departamento IN  DEPT.DNOMBRE%TYPE,
    //  p_emp_no OUT EMP.EMP_NO%TYPE
    //)
    //AS
    //  v_dept_no   DEPT.DEPT_NO%TYPE;
    //  v_fecha_alt EMP.FECHA_ALT%TYPE;
    //    BEGIN

    //  -- Obtener departamento
    //  SELECT DEPT_NO
    //  INTO v_dept_no
    //  FROM DEPT
    //  WHERE DNOMBRE = p_departamento;

    //  -- Generar nuevo ID(mejor usar SEQUENCE en producción)
    //  SELECT NVL(MAX(EMP_NO),0) + 1
    //  INTO p_emp_no
    //  FROM EMP;

    //    v_fecha_alt := SYSDATE;

    //  -- Insertar empleado
    //  INSERT INTO EMP
    //  VALUES(p_emp_no,
    //          p_apellido,
    //          p_oficio,
    //          p_dir,
    //          v_fecha_alt,
    //          p_salario,
    //          p_comision,
    //          v_dept_no);

    //    END;

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

        public async Task<int> InsertEmpleadoAsync(string apellido, string oficio, int director, int salario, int comision, string departamento)
        {
            string sql = "BEGIN SP_INSERT_EMP( :apellido, :oficio, :dir, :salario, :comision, :departamento, :emp_no); END;";
            OracleParameter pamApellido = new OracleParameter(":apellido", apellido);
            OracleParameter pamOficio = new OracleParameter(":oficio", oficio);
            OracleParameter pamDir = new OracleParameter(":dir", director);
            OracleParameter pamSalario = new OracleParameter(":salario", salario);
            OracleParameter pamComision = new OracleParameter(":comision", comision);
            OracleParameter pamDepart = new OracleParameter(":departamento", departamento);

            // 🔹 Parámetro de salida
            OracleParameter pamEmpNo = new OracleParameter(":emp_no", OracleDbType.Int32);
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
            OracleDecimal oracleNumber = (OracleDecimal)pamEmpNo.Value;
            return oracleNumber.ToInt32();

        }
    }
}

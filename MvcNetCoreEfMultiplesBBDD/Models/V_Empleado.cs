using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcNetCoreEfMultiplesBBDD.Models
{
    [Table("V_EMPLEADOS")]
    public class V_Empleado
    {


        [Key]
        [Column("EMP_NO")]
        public int Emp_No { get; set; }

        [Column("APELLIDO")]
        public string Apellido { get; set; }
        [Column("OFICIO")]
        public string Oficio { get; set; }
        [Column("SALARIO")]
        public int Salario { get; set; }
        [Column("IDDEPARTAMENTO")]


        public int Dept_No { get; set; }
        [Column("DEPARTAMENTO")]
        public string Dnombre { get; set; }
        [Column("LOCALIDAD")]
        public string LOCALIDAD { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcNetCoreEfMultiplesBBDD.Models
{
    [Table("EMP")]
    public class Empleado
    {
        [Key]
        [Column("EMP_NO")]
        public int Emp_No { get; set; }
        [Column("APELLIDO")]
        public string apellido { get; set; }
        [Column("OFICIO")]
        public string oficio { get; set; }
        [Column("DIR")]
        public int dir { get; set; }
        [Column("FECHA_ALT")]
        public DateTime fecha_alt { get; set; }
        [Column("SALARIO")]
        public int salario { get; set; }
        [Column("COMISION")]
        public int comision { get; set; }
        [Column("DEPT_NO")]
        public string departamento { get; set; }

    }
}

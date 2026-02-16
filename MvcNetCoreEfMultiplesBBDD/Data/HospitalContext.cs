using Microsoft.EntityFrameworkCore;
using MvcNetCoreEfMultiplesBBDD.Models;

namespace MvcNetCoreEfMultiplesBBDD.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options) : base(options)
        {
        }
        public DbSet<V_Empleado> VistaEmpleados { get; set; }

        public DbSet<Empleado> tablaEmp { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using MvcNetCoreEfMultiplesBBDD.Models;
using MvcNetCoreEfMultiplesBBDD.Repositories;

namespace MvcNetCoreEfMultiplesBBDD.Controllers
{
    public class TrabajadoresController : Controller
    {
        public IRepositoryEmpleados repo;
        public TrabajadoresController(IRepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            List<V_Empleado> empleados = await this.repo.GetEmpleadosAsync();
            List<string> oficios = await this.repo.GetOficiosAsync();
            ViewData["OFICIOS"] = oficios;
            return View(empleados);
        }
        [HttpPost]
        public async Task<IActionResult> Index(string oficio)
        {
            List<V_Empleado> empleados = await this.repo.GetEmpleadosPorOficioAsync(oficio);
            List<string> oficios = await this.repo.GetOficiosAsync();
            ViewData["OFICIOS"] = oficios;
            return View(empleados);
        }
        public async Task<IActionResult> Details(int emp_no)
        {
            V_Empleado model = await this.repo.GetDetailsEmpleadoByIdAsync(emp_no);
            return View(model);
        }
    }
}

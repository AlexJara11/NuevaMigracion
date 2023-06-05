using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FrontSeccion;
using FrontSeccion.Models;
using FrontSeccion.Services.Alumnos;
using BackSeccion1.Services.Alumnos;

namespace FrontSeccion1.Controllers
{
    public class AlumnoesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AlumnoServiceCrear _alumnoService;
        private readonly AlumnoServiceUpdate _serviceUpdate;
        private readonly AlumnoServiceDelete _serviceDelete;

        public AlumnoesController(ApplicationDbContext context, AlumnoServiceCrear alumnoService, AlumnoServiceUpdate serviceUpdate, AlumnoServiceDelete serviceDelete)
        {
            _context = context;
            _alumnoService = alumnoService;
            _serviceUpdate = serviceUpdate;
            _serviceDelete = serviceDelete; 
        }
        // GET: Alumnoes
        public async Task<IActionResult> Index()
        {
            try
            {
                var alumnos = await _context.Alumno
                    .Where(a => !a.isDeleted)  // Filtrar por isDeleted en false
                    .ToListAsync();

                return View(alumnos);
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                Console.WriteLine("Se ha producido una excepción: " + ex.Message);

                // Redirigir a una página de error o hacer algo más
                return RedirectToAction("Error", "Home");
            }

        }



        // GET: BUSQUEDA
        public IActionResult Busqueda(string searchString)
        {
            var alumnos = from a in _context.Alumno
                          select a;

            if (!string.IsNullOrEmpty(searchString))
            {
                alumnos = alumnos.Where(a => a.dni_alu.Contains(searchString));
            }

            return View("Index", alumnos.ToList());

        }

        

    

    // GET: Alumnoes/Details/5
    public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Alumno == null)
            {
                return NotFound();
            }

            var alumno = await _context.Alumno
                .FirstOrDefaultAsync(m => m.id_alu == id);
            if (alumno == null)
            {
                return NotFound();
            }

            return View(alumno);
        }

        // GET: Alumnoes/Create
        public IActionResult Create()
        {
            try
            {

                return View();
            }
            catch (Exception ex)
            {          
                Console.WriteLine("Se ha producido una excepción: " + ex.Message);            
                return RedirectToAction("Error", "Home");
            }
        }

        // POST: Alumnoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_alu,dni_alu,nombre_alu,apellidos_alu")] Alumno alumno)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    alumno.isDeleted = false;
                    bool created = await _alumnoService.CreateAlumno(alumno);
                    if (created)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        // Handle failure to create alumno
                        Console.WriteLine("Se ha producido una excepción: ");
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                
                Console.WriteLine("Se ha producido una excepción: " + ex.Message);

                // Redirigir a una página de error o hacer algo más
                return RedirectToAction("Error", "Home");
            }

            return View(alumno);
        }


        // GET: Alumnoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Alumno == null)
            {
                return NotFound();
            }

            var alumno = await _context.Alumno.FindAsync(id);
            if (alumno == null)
            {
                return NotFound();
            }
            return View(alumno);
        }

        // POST: Alumnoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_alu,dni_alu,nombre_alu,apellidos_alu")] Alumno alumno)
        {
            try
            {
                if (id != alumno.id_alu)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    bool updated = await _serviceUpdate.UpdateAlumno(alumno);
                    if (updated)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        // Handle failure to update alumno
                        Console.WriteLine("No se pudo actualizar el alumno o el alumno ha sido eliminado lógicamente.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                Console.WriteLine("Se ha producido una excepción: " + ex.Message);

                // Redirigir a una página de error o hacer algo más
                return RedirectToAction("Error", "Home");
            }

            return View(alumno);
        }

        // GET: Alumnoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Alumno == null)
            {
                return NotFound();
            }

            var alumno = await _context.Alumno
                .FirstOrDefaultAsync(m => m.id_alu == id);
            if (alumno == null)
            {
                return NotFound();
            }

            return View(alumno);
        }

        // POST: Alumnoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            try
            {
                bool deleted = await _serviceDelete.DeleteAlumno(id);
                if (deleted)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Manejar fallo en la eliminación del alumno
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                Console.WriteLine("Se ha producido una excepción: " + ex.Message);

                // Redirigir a una página de error o realizar otra acción
                return RedirectToAction("Error", "Home");
            }

        }

        private bool AlumnoExists(int id)
        {
          return (_context.Alumno?.Any(e => e.id_alu == id)).GetValueOrDefault();
        }
    }
}

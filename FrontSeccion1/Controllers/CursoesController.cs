using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FrontSeccion;
using FrontSeccion.Models;
using BackSeccion1.Services.Cursos;

namespace FrontSeccion1.Controllers
{
    public class CursoesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CursoServiceCrear _serviceCrear;
        private readonly CursoServiceUpdate _serviceUpdate;
        private readonly CursoServiceDelete _serviceDelete;

        public CursoesController(ApplicationDbContext context, CursoServiceCrear serviceCrear, CursoServiceUpdate serviceUpdate, CursoServiceDelete serviceDelete)
        {
            _context = context;
            _serviceCrear = serviceCrear;
            _serviceUpdate = serviceUpdate;
            _serviceDelete = serviceDelete;
        }

        // GET: Cursoes
        public async Task<IActionResult> Index()
        {
            try
            {
                if (_context.Curso != null)
                {
                    var cursos = await _context.Curso.Where(c => !c.isDeleted).ToListAsync();
                    return View(cursos);
                }
                else
                {
                    return Problem("Entity set 'ApplicationDbContext.Curso' is null.");
                }
            }
            catch (Exception ex)
            {
                // Handle exception if needed
                return Problem("An error occurred while processing the request.", statusCode: 500);
            }

            
        }

        // GET: Cursoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Curso == null)
            {
                return NotFound();
            }

            var curso = await _context.Curso
                .FirstOrDefaultAsync(m => m.id_cur == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // GET: Cursoes/Create
        public IActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                // Handle exception if needed
                return Problem("An error occurred while processing the request.", statusCode: 500);
            }
        }

        // POST: Cursoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_cur,descripcion_cur,estado_cur")] Curso curso)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool created = await _serviceCrear.CreateCurso(curso);
                    if (created)
                        return RedirectToAction(nameof(Index));
                    else
                        return Problem("Failed to create the curso.");
                }

                return View(curso);
            }
            catch (Exception ex)
            {
                // Handle exception if needed
                return Problem("An error occurred while processing the request.", statusCode: 500);
            }
        }

    // GET: Cursoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Curso == null)
            {
                return NotFound();
            }

            var curso = await _context.Curso.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }
            return View(curso);
        }

        // POST: Cursoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_cur,descripcion_cur,estado_cur")] Curso curso)
        {
            try
            {
                if (id != curso.id_cur)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    bool updated = await _serviceUpdate.UpdateCurso(curso);
                    if (updated)
                        return RedirectToAction(nameof(Index));
                    else
                        return Problem("Failed to update the curso.");
                }

                return View(curso);
            }
            catch (Exception ex)
            {
                // Handle exception if needed
                return Problem("An error occurred while processing the request.", statusCode: 500);
            }
        }


        // GET: Cursoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Curso == null)
            {
                return NotFound();
            }

            var curso = await _context.Curso
                .FirstOrDefaultAsync(m => m.id_cur == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // POST: Cursoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                bool deleted = await _serviceDelete.DeleteCurso(id);
                if (deleted)
                    return RedirectToAction(nameof(Index));
                else
                    return Problem("Failed to delete the curso.");
            }
            catch (Exception ex)
            {
                // Handle exception if needed
                return Problem("An error occurred while processing the request.", statusCode: 500);
            }
        }

        private bool CursoExists(int id)
        {
          return (_context.Curso?.Any(e => e.id_cur == id)).GetValueOrDefault();
        }
    }
}

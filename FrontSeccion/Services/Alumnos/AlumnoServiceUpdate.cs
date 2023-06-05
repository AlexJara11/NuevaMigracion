using FrontSeccion;
using FrontSeccion.Models;
using Microsoft.EntityFrameworkCore;

namespace BackSeccion1.Services.Alumnos
{
    public class AlumnoServiceUpdate
    {
        private readonly ApplicationDbContext _context;

        public AlumnoServiceUpdate(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> UpdateAlumno(Alumno alumno)
        {
            if (alumno == null)
                throw new ArgumentNullException(nameof(alumno));

            if (!_context.Alumno.Any(a => a.id_alu == alumno.id_alu))
                return false;

            try
            {
                var existingAlumno = await _context.Alumno.FindAsync(alumno.id_alu);

                if (existingAlumno.isDeleted)
                    return false;  // No permitir actualizar un alumno eliminado lógicamente

                _context.Entry(existingAlumno).CurrentValues.SetValues(alumno);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency exception (e.g., if the alumno was deleted by another process)
                return false;
            }
        }
    }
}

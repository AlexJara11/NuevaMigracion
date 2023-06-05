using FrontSeccion;

namespace BackSeccion1.Services.Alumnos
{
    public class AlumnoServiceDelete
    {
        private readonly ApplicationDbContext _context;

        public AlumnoServiceDelete(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteAlumno(int id)
        {
            var alumno = await _context.Alumno.FindAsync(id);
            if (alumno == null)
                return false;

            try
            {
                alumno.isDeleted = true;  // Establecer eliminación lógica
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                // Manejar la excepción si es necesario
                return false;
            }
        }
    }
}



using FrontSeccion.Models;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FrontSeccion.Services.Alumnos
{
    public class AlumnoServiceCrear
    {
        private readonly ApplicationDbContext _context;

        public AlumnoServiceCrear(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateAlumno(Alumno alumno)
        {

            var validationContext = new ValidationContext(alumno, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(alumno, validationContext, validationResults, validateAllProperties: true);

            if (isValid)
            {
                _context.Add(alumno);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}

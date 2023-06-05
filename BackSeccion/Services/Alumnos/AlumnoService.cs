using FrontSeccion.Models;
using FrontSeccion;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BackSeccion.Services.Alumnos
{
    public class AlumnoService
    {
        private readonly ApplicationDbContext _context;


        public AlumnoService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateAlumno(Alumno alumno)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(alumno);
            //    await _context.SaveChangesAsync();
            //    return true;
            //}

            //return false;
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

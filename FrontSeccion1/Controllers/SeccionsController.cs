using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FrontSeccion;
using FrontSeccion.Models;
using System.Drawing;
using System.Xml.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ClosedXML.Excel;
using BackSeccion1.Services.Secciones;
using FrontSeccion.Services.Alumnos;

namespace FrontSeccion1.Controllers
{
    public class SeccionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SeccionServicesCrear _serviceCrear;
        private readonly SeccionServicesUpdate _servicesUpdate;
        private readonly SeccionServicesDelete _servicesDelete;
        private Seccion sec = new Seccion();

        public SeccionsController(ApplicationDbContext context, SeccionServicesCrear servicesCrear, 
            SeccionServicesUpdate servicesUpdate, SeccionServicesDelete servicesDelete)
        {
            _context = context;
            _serviceCrear = servicesCrear;
            _servicesUpdate = servicesUpdate;
            _servicesDelete = servicesDelete;
        }

        // GET: Seccions
        public async Task<IActionResult> Index()
        {

            try
            {
                var secciones = await _context.Seccion.Where(s => !s.isDeleted)
                    .Include(s => s.Curso)
                    .ToListAsync();
                return View(secciones);
            }
            catch (Exception ex)
            {
                // Handle exception if needed
                Console.WriteLine("Se ha producido una excepción: " + ex.Message);
                return Problem("An error occurred while processing the request.", statusCode: 500);
            }
            //try
            //{
            //    var applicationDbContext = await _context.Seccion.Include(s => s.Curso).ToListAsync();
            //    return View(applicationDbContext);
            //}
            //catch (Exception ex)
            //{
            //    // Manejo de la excepción
            //    // Puedes imprimir el mensaje de error o realizar alguna otra acción
            //    Console.WriteLine("Se ha producido una excepción: " + ex.Message);

            //    // Redirigir a una página de error o hacer algo más
            //    return RedirectToAction("Error", "Home");
            //}
        }
        

        // GET: Seccions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Seccion == null)
            {
                return NotFound();
            }

            var seccion = await _context.Seccion
                .Include(s => s.Curso)
                .FirstOrDefaultAsync(m => m.id_sec == id);
            if (seccion == null)
            {
                return NotFound();
            }

            return View(seccion);
        }
        
        //ExportarPDF
        public FileResult ExportarAlumnosAPdf(int idSec)
        {
            // Obtener los datos de los alumnos de la sección específica
            var alumnos = _context.Alumno
                .Include(a => a.Seccion)
                .Where(a => a.Seccion.Any(s => s.id_sec == idSec))
                .ToList();

            // Obtener la descripción del curso de la sección específica
            var descripcionCurso = _context.Curso
                .Include(s => s.Seccion)
                .Where(a => a.Seccion.Any(s => s.id_sec == idSec))
                .Select(s => s.descripcion_cur)
                .FirstOrDefault();

            // Crear el documento PDF
            Document document = new Document();
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

            // Abrir el documento
            document.Open();
            // Agregar el encabezado con formato de título centrado
            Paragraph header = new Paragraph("Listado de Alumnos - Curso: " + descripcionCurso);
            header.Alignment = Element.ALIGN_CENTER; // Alinear el texto al centro
            header.Font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16); // Establecer la fuente y tamaño del texto
            header.SpacingAfter = 10; // Espacio después del encabezado
            document.Add(header);


            // Crear una tabla para mostrar los datos de los alumnos
            PdfPTable table = new PdfPTable(3); // 3 columnas para DNI, Nombre y Apellidos
            table.WidthPercentage = 100;

            // Agregar encabezados de columna
            table.AddCell("DNI");
            table.AddCell("NOMBRES");
            table.AddCell("APELLIDOS");

            // Agregar filas con los datos de los alumnos
            foreach (var alumno in alumnos)
            {
                table.AddCell(alumno.dni_alu);
                table.AddCell(alumno.nombre_alu);
                table.AddCell(alumno.apellidos_alu);
            }

            // Agregar la tabla al documento
            document.Add(table);

            // Cerrar el documento
            document.Close();

            // Descargar el archivo PDF
            byte[] fileBytes = memoryStream.ToArray();
            memoryStream.Close();

            return File(fileBytes, "application/pdf", "alumnos_seccion.pdf");
        }

        //ExportarExcel
        public FileResult ExportarAlumnosAExcel(int idSec)
        {
            // Obtener los datos de los alumnos de la sección específica
            var alumnos = _context.Alumno
                .Include(a => a.Seccion)
                .Where(a => a.Seccion.Any(s => s.id_sec == idSec))
                .ToList();

            // Obtener la descripción del curso de la sección específica
            var descripcionCurso = _context.Curso
                .Include(s => s.Seccion)
                .Where(a => a.Seccion.Any(s => s.id_sec == idSec))
                .Select(s => s.descripcion_cur)
                .FirstOrDefault();

            // Crear un nuevo libro de Excel
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Alumnos");

            // Agregar el encabezado con formato de título
            worksheet.Cell("A1").Value = "Listado de Alumnos - " + descripcionCurso;
            worksheet.Range("A1:C1").Merge().Style.Font.SetBold().Font.SetFontSize(16);
            worksheet.Row(1).Height = 25;

            // Agregar encabezados de columna
            worksheet.Cell(2, 1).Value = "DNI";
            worksheet.Cell(2, 2).Value = "NOMBRES";
            worksheet.Cell(2, 3).Value = "APELLIDOS";

            // Agregar filas con los datos de los alumnos
            for (int i = 0; i < alumnos.Count; i++)
            {
                var alumno = alumnos[i];
                worksheet.Cell(i + 2, 1).Value = alumno.dni_alu;
                worksheet.Cell(i + 2, 2).Value = alumno.nombre_alu;
                worksheet.Cell(i + 2, 3).Value = alumno.apellidos_alu;
            }

            // Guardar el libro de Excel en un MemoryStream
            var memoryStream = new MemoryStream();
            workbook.SaveAs(memoryStream);
            memoryStream.Position = 0;

            // Descargar el archivo Excel
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "alumnos_seccion.xlsx");
        }




       

        // GET: Seccions/Create
        public IActionResult Create()
        {
            ViewData["CursoId"] = new SelectList(_context.Curso, "id_cur", "descripcion_cur");
            return View();
        }

        // POST: Seccions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id_sec,aula_sec,CursoId,estado_sec,fecha_registro_sec")] Seccion seccion)
        {
            try
            {
                bool created = await _serviceCrear.CreateSeccion(seccion);
                if (created)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Handle failure to create seccion
                    // ...
                }
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                Console.WriteLine("Se ha producido una excepción: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }

            ViewData["CursoId"] = new SelectList(_context.Curso, "id_cur", "descripcion_cur", seccion.CursoId);
            return View(seccion);
        }


        // GET: Seccions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Seccion == null)
            {
                return NotFound();
            }

            var seccion = await _context.Seccion.FindAsync(id);
            if (seccion == null)
            {
                return NotFound();
            }
            ViewData["CursoId"] = new SelectList(_context.Curso, "id_cur", "descripcion_cur", seccion.CursoId);
            return View(seccion);
        }

        // POST: Seccions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id_sec,aula_sec,CursoId,estado_sec,fecha_registro_sec")] Seccion seccion)
        {
            try
            {
                if (id != seccion.id_sec)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    bool edited = await _servicesUpdate.EditSeccion(seccion);
                    if (edited)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        // Handle failure to edit seccion
                        // ...
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                Console.WriteLine("Se ha producido una excepción: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }

            ViewData["CursoId"] = new SelectList(_context.Curso, "id_cur", "descripcion_cur", seccion.CursoId);
            return View(seccion);
        }

        // GET: Seccions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Seccion == null)
            {
                return NotFound();
            }

            var seccion = await _context.Seccion
                .Include(s => s.Curso)
                .FirstOrDefaultAsync(m => m.id_sec == id);
            if (seccion == null)
            {
                return NotFound();
            }

            return View(seccion);
        }

        // POST: Seccions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                bool deleted = await _servicesDelete.DeleteSeccion(id);
                if (deleted)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    // Handle failure to delete seccion
                    // ...
                }
            }
            catch (Exception ex)
            {
                // Manejo de la excepción
                Console.WriteLine("Se ha producido una excepción: " + ex.Message);
                return RedirectToAction("Error", "Home");
            }
            return Problem("Entity set 'ApplicationDbContext.Seccion' is null.");
            //try
            //{
            //    bool deleted = await _servicesDelete.DeleteSeccion(id);
            //    if (deleted)
            //    {
            //        return RedirectToAction(nameof(Index));
            //    }
            //    else
            //    {
            //        // Handle failure to delete seccion
            //        // ...
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // Manejo de la excepción
            //    Console.WriteLine("Se ha producido una excepción: " + ex.Message);
            //    return RedirectToAction("Error", "Home");
            //}
            //return Problem("Entity set 'ApplicationDbContext.Seccion' is null.");
        }
    }
}

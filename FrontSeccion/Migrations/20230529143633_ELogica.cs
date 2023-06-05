using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackSeccion1.Migrations
{
    /// <inheritdoc />
    public partial class ELogica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Alumno",
                columns: table => new
                {
                    id_alu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dni_alu = table.Column<string>(type: "char(8)", unicode: false, fixedLength: true, maxLength: 8, nullable: false),
                    nombre_alu = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    apellidos_alu = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumno", x => x.id_alu);
                });

            migrationBuilder.CreateTable(
                name: "Curso",
                columns: table => new
                {
                    id_cur = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    descripcion_cur = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: false),
                    estado_cur = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curso", x => x.id_cur);
                });

            migrationBuilder.CreateTable(
                name: "Seccion",
                columns: table => new
                {
                    id_sec = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    aula_sec = table.Column<int>(type: "int", nullable: false),
                    CursoId = table.Column<int>(type: "int", nullable: false),
                    estado_sec = table.Column<bool>(type: "bit", nullable: false),
                    fecha_registro_sec = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seccion", x => x.id_sec);
                    table.ForeignKey(
                        name: "FK_Seccion_Curso_CursoId",
                        column: x => x.CursoId,
                        principalTable: "Curso",
                        principalColumn: "id_cur",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Detalle_asig_alumno_seccion",
                columns: table => new
                {
                    Alumnoid_alu = table.Column<int>(type: "int", nullable: false),
                    Seccionid_sec = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Detalle_asig_alumno_seccion", x => new { x.Alumnoid_alu, x.Seccionid_sec });
                    table.ForeignKey(
                        name: "FK_Detalle_asig_alumno_seccion_Alumno_Alumnoid_alu",
                        column: x => x.Alumnoid_alu,
                        principalTable: "Alumno",
                        principalColumn: "id_alu",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Detalle_asig_alumno_seccion_Seccion_Seccionid_sec",
                        column: x => x.Seccionid_sec,
                        principalTable: "Seccion",
                        principalColumn: "id_sec",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_asig_alumno_seccion_Seccionid_sec",
                table: "Detalle_asig_alumno_seccion",
                column: "Seccionid_sec");

            migrationBuilder.CreateIndex(
                name: "IX_Seccion_CursoId",
                table: "Seccion",
                column: "CursoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Detalle_asig_alumno_seccion");

            migrationBuilder.DropTable(
                name: "Alumno");

            migrationBuilder.DropTable(
                name: "Seccion");

            migrationBuilder.DropTable(
                name: "Curso");
        }
    }
}

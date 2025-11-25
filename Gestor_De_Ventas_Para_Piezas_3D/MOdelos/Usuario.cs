using SQLite;
using System;

namespace Gestor_De_Ventas_Para_Piezas_3D.Modelos
{
    [Table("Usuarios")]
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        // --- Datos Personales ---
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }

        // --- Datos de Usuario ---
        public string Rol { get; set; }

        [Unique] // El usuario debe ser único
        public string NombreUsuario { get; set; }

        public string Contrasena { get; set; }
    }
}
using SQLite;
using System;
using System.Globalization;

namespace Gestor_De_Ventas_Para_Piezas_3D.Modelos
{
    [Table("Ventas")]
    public class Venta
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string NombreEmpleado { get; set; }
        public string Cliente { get; set; }
        public string Telefono { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public string FechaSolicitud { get; set; }
        public string FechaEntrega { get; set; }
        public string Duracion { get; set; }
        public string Observaciones { get; set; }
        public decimal Costo { get; set; }

        // Estado: "Venta", "En producción", "Completado", "Entregado"
        public string Estado { get; set; } = "Venta";

        [Ignore]
        public string CostoFormateado => Costo.ToString("C", new CultureInfo("es-MX"));

        // --- PROPIEDADES VISUALES PARA LA TABLA DE ESTADOS ---
        // Estas propiedades ponen una "X" solo si el estado coincide
        [Ignore]
        public string MarcaVenta => (Estado == "Venta" || Estado == "Pendiente") ? "X" : "";

        [Ignore]
        public string MarcaProduccion => Estado == "En producción" ? "X" : "";

        [Ignore]
        public string MarcaCompletado => Estado == "Completado" ? "X" : "";

        [Ignore]
        public string MarcaEntregado => Estado == "Entregado" ? "X" : "";
    }
}
using System;

namespace Gestor_De_Ventas_Para_Piezas_3D.Modelos
{
    // Esta clase representa una fila en la tabla de Registro de Ventas
    public class Venta
    {
        public int Id { get; set; }
        public string NombreEmpleado { get; set; }
        public string Cliente { get; set; } // Lo conservamos en el modelo por si se necesita en otra parte.
        public string Telefono { get; set; }
        public string Producto { get; set; }
        public int Cantidad { get; set; }
        public string FechaSolicitud { get; set; }
        public string FechaEntrega { get; set; }
        public string Duracion { get; set; }
        public string Observaciones { get; set; }
        public decimal Costo { get; set; } // Usamos decimal para manejar dinero con precisión

        // Propiedad calculada para formato de moneda (Pesos Mexicanos)
        public string CostoFormateado => Costo.ToString("C", new System.Globalization.CultureInfo("es-MX"));
    }
}
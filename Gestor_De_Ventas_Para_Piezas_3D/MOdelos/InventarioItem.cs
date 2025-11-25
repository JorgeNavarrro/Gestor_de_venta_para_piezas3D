using SQLite;
using System.Globalization;

namespace Gestor_De_Ventas_Para_Piezas_3D.Modelos
{
    [Table("Inventario")]
    public class InventarioItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string NombreArticulo { get; set; }
        public string Categoria { get; set; }
        public string Detalles { get; set; } // Material/Color
        public int StockActual { get; set; }
        public string Unidad { get; set; } // Rollos, Litros, Piezas
        public decimal PrecioCosto { get; set; }
        public string TipoPrecio { get; set; } // "Costo" o "Venta"

        // --- PROPIEDADES VISUALES (Calculadas) ---

        [Ignore]
        public string PrecioCostoFormateado => PrecioCosto.ToString("C", new CultureInfo("es-MX"));

        // Regla de Negocio: Menor a 50 es Bajo Stock
        [Ignore]
        public string Estado => StockActual < 50 ? "Bajo Stock" : "En Stock";

        // Color: Rojo si es bajo stock, Negro si está bien
        [Ignore]
        public string ColorEstado => StockActual < 50 ? "#E74C3C" : "Black";

        [Ignore]
        public bool EsBajoStock => StockActual < 50;
    }
}
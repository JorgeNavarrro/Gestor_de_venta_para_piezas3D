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
        public string Detalles { get; set; }
        public int StockActual { get; set; }
        public string Unidad { get; set; }
        public decimal PrecioCosto { get; set; }
        public string TipoPrecio { get; set; }

        [Ignore]
        public string PrecioCostoFormateado => PrecioCosto.ToString("C", new CultureInfo("es-MX"));

        // --- LÓGICA VISUAL ---

        [Ignore]
        public string Estado
        {
            get
            {
                if (StockActual < 50) return "⚠️ Alerta: Bajo Stock";
                if (StockActual > 80) return "⚠️ Exceso de Merma";
                return "En Stock";
            }
        }

        [Ignore]
        public string ColorEstado
        {
            get
            {
                if (StockActual < 50) return "#E74C3C"; // Rojo
                if (StockActual > 80) return "#F39C12"; // Naranja
                return "#27AE60"; // Verde
            }
        }

        // ✅ ESTA ES LA PROPIEDAD QUE TE FALTABA
        [Ignore]
        public bool EsBajoStock => StockActual < 50;
    }
}
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

        // --- LÓGICA VISUAL ACTUALIZADA ---

        [Ignore]
        public string Estado
        {
            get
            {
                // Definimos límites según la unidad de medida
                if (Unidad == "Mililitros" || Unidad == "Gramos")
                {
                    // Para líquidos o polvo (Ej. Resina): Menos de 1000 es bajo, Más de 10000 (10 L/Kg) es exceso
                    if (StockActual < 1000) return "⚠️ Alerta: Bajo Stock";
                    if (StockActual > 10000) return "⚠️ Exceso de Merma";
                }
                else if (Unidad == "Pliegos" || Unidad == "Hojas")
                {
                    // Para papel/vinil: Menos de 20 es bajo, Más de 500 es exceso
                    if (StockActual < 20) return "⚠️ Alerta: Bajo Stock";
                    if (StockActual > 500) return "⚠️ Exceso de Merma";
                }
                else
                {
                    // Para piezas, bolsas u otros por defecto (ej. Llaveros, argollas, etc.)
                    if (StockActual < 50) return "⚠️ Alerta: Bajo Stock";
                    if (StockActual > 200) return "⚠️ Exceso de Merma";
                }

                return "En Stock";
            }
        }

        [Ignore]
        public string ColorEstado
        {
            get
            {
                // El color se asigna automáticamente dependiendo de lo que diga el 'Estado'
                if (Estado == "⚠️ Alerta: Bajo Stock") return "#E74C3C"; // Rojo
                if (Estado == "⚠️ Exceso de Merma") return "#F39C12"; // Naranja

                return "#27AE60"; // Verde
            }
        }

        [Ignore]
        public bool EsBajoStock => Estado == "⚠️ Alerta: Bajo Stock";
    }
}
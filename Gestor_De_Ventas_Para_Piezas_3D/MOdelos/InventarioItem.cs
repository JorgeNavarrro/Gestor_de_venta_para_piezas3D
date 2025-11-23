using System;

namespace Gestor_De_Ventas_Para_Piezas_3D.Modelos
{
    public class InventarioItem
    {
        public int Id { get; set; }
        public string NombreArticulo { get; set; }
        public string Categoria { get; set; }
        public string Detalles { get; set; }
        public int StockActual { get; set; }
        public string Unidad { get; set; }
        public string Estado { get; set; }
        public decimal PrecioCosto { get; set; }
        public string TipoPrecio { get; set; }

        public string PrecioCostoFormateado => PrecioCosto.ToString("C", new System.Globalization.CultureInfo("es-MX"));

        public string ColorEstado
        {
            get
            {
                if (Estado == "Bajo Stock") return "OrangeRed";
                if (Estado == "Agotado") return "Red";
                return "Black";
            }
        }
    }
}
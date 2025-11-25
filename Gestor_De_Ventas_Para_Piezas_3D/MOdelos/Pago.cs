using SQLite;
using System;
using System.Globalization;

namespace Gestor_De_Ventas_Para_Piezas_3D.Modelos
{
    [Table("Pagos")]
    public class Pago
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int IdPedido { get; set; } // Puede ser manual o vinculado
        public string NombreCliente { get; set; }
        public string TelefonoCliente { get; set; }
        public string Productos { get; set; }
        public string MedioPago { get; set; } // Tarjeta, Efectivo, etc.
        public string EstadoPago { get; set; } = "Pagado"; // Por defecto
        public decimal CostoTotal { get; set; }
        public string FechaEmision { get; set; }
        public string Observaciones { get; set; }

        // Solo visual
        [Ignore]
        public string CostoTotalFormateado => CostoTotal.ToString("C", new CultureInfo("es-MX"));

        [Ignore]
        public string NumeroNota => $"NOTA-{Id:D6}";
    }
}
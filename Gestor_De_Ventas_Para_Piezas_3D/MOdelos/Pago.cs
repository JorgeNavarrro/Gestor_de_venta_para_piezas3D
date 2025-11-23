using System;
using System.Globalization;

namespace Gestor_De_Ventas_Para_Piezas_3D.Modelos
{
    public class Pago
    {
        public int Id { get; set; }
        public int IdPedido { get; set; }
        public string NombreCliente { get; set; }
        public string TelefonoCliente { get; set; }
        public string Productos { get; set; }
        public string MedioPago { get; set; } // Tarjeta, Efectivo, Transferencia
        public string EstadoPago { get; set; } // Pendiente, Pagado
        public decimal CostoTotal { get; set; }
        public string FechaEmision { get; set; }
        public string Observaciones { get; set; }

        // Formato de moneda MXN
        public string CostoTotalFormateado => CostoTotal.ToString("C", new CultureInfo("es-MX"));

        // Generamos un número de nota ficticio basado en el ID
        public string NumeroNota => $"NOTA-{Id:D6}";
    }
}
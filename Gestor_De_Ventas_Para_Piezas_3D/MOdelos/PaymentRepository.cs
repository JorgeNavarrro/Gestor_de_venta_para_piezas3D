using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Gestor_De_Ventas_Para_Piezas_3D.Modelos
{
    // Esta clase estática simula una base de datos en memoria
    public static class PaymentRepository
    {
        public static ObservableCollection<Pago> Pagos { get; set; } = new ObservableCollection<Pago>();

        static PaymentRepository()
        {
            // Cargar datos de prueba iniciales
            Pagos.Add(new Pago { Id = 1, IdPedido = 101, NombreCliente = "Ana Martinez", TelefonoCliente = "982131888", Productos = "Maceta", MedioPago = "Tarjeta", EstadoPago = "Pagado", CostoTotal = 5000.00m, FechaEmision = "18/11/2025", Observaciones = "Pago aprobado" });
            Pagos.Add(new Pago { Id = 2, IdPedido = 102, NombreCliente = "Luis Torres", TelefonoCliente = "982131777", Productos = "Llavero", MedioPago = "Efectivo", EstadoPago = "Pendiente", CostoTotal = 1200.00m, FechaEmision = "18/11/2025", Observaciones = "Pago contra entrega" });
            Pagos.Add(new Pago { Id = 3, IdPedido = 103, NombreCliente = "Jorge Navarro", TelefonoCliente = "982131666", Productos = "Figura", MedioPago = "Transferencia", EstadoPago = "Pagado", CostoTotal = 1750.00m, FechaEmision = "18/11/2025", Observaciones = "Comprobante enviado" });
        }

        public static void AgregarPago(Pago nuevoPago)
        {
            Pagos.Add(nuevoPago);
        }
    }
}
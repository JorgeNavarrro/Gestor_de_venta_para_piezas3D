using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Gestor_De_Ventas_Para_Piezas_3D.Modelos
{
    // Implementamos INotifyPropertyChanged para que la "X" se mueva automáticamente al cambiar el estado
    public class Pedido : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Cliente { get; set; }
        public string Producto { get; set; }
        public string FechaEntrega { get; set; }
        public string FechaInicio { get; set; }
        public string Duracion { get; set; }

        private string _estado;
        public string Estado
        {
            get => _estado;
            set
            {
                if (_estado != value)
                {
                    _estado = value;
                    OnPropertyChanged();
                    // Notificar a las propiedades visuales que cambiaron
                    OnPropertyChanged(nameof(IsVenta));
                    OnPropertyChanged(nameof(IsProduccion));
                    OnPropertyChanged(nameof(IsCompletado));
                    OnPropertyChanged(nameof(IsEntregado));
                }
            }
        }

        public string ColorEstado { get; set; } = "Black";

        // Propiedades visuales para la tabla de Producción (Devuelven "X" si coincide el estado)
        public string IsVenta => Estado == "Venta" ? "X" : "";
        public string IsProduccion => Estado == "En producción" ? "X" : "";
        public string IsCompletado => Estado == "Completado" ? "X" : "";
        public string IsEntregado => Estado == "Entregado" ? "X" : "";

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
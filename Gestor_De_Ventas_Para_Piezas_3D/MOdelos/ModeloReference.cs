using System;

namespace Gestor_De_Ventas_Para_Piezas_3D.Modelos
{
    public class ModeloReference
    {
        public string Nombre { get; set; }
        public string Dimensiones { get; set; }
        public string Material { get; set; }
        public string Categoria { get; set; }
        public string Descripcion { get; set; }

        // En una app real, aquí iría la ruta de la imagen
        // Por ahora usaremos un color o un placeholder
        public string ImagenPlaceholder => "Cube";
    }
}
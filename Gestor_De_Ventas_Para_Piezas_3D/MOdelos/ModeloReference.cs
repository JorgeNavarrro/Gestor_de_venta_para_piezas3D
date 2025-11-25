using SQLite; // Usamos SQLite en lugar de MySQL

namespace Gestor_De_Ventas_Para_Piezas_3D.Modelos
{
    // Esto crea una tabla llamada "Productos" en tu archivo local
    [Table("Productos")]
    public class ModeloReference
    {
        // ID único y autoincrementable (1, 2, 3...)
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public string Material { get; set; }
        public string Descripcion { get; set; }
        public string Imagen { get; set; }
    }
}
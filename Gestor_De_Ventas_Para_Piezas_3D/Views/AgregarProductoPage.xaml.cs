using System;
using Microsoft.Maui.Controls;
using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using Gestor_De_Ventas_Para_Piezas_3D.Services;

// IMPORTANTE: El namespace debe coincidir con lo que pusiste en x:Class del XAML
namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas
{
    public partial class AgregarProductoPage : ContentPage
    {
        public AgregarProductoPage()
        {
            InitializeComponent();
        }

        // Este método se ejecuta al presionar el botón "Guardar"
        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            // 1. Validamos que los campos obligatorios no estén vacíos
            // 'txtNombre' y 'pkrCategoria' son los nombres que pusimos en el XAML
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || pkrCategoria.SelectedIndex == -1)
            {
                await DisplayAlert("Faltan Datos", "Por favor escribe un nombre y selecciona una categoría.", "OK");
                return;
            }

            // 2. Creamos un nuevo objeto Producto con los datos del formulario
            var nuevoProducto = new ModeloReference
            {
                Nombre = txtNombre.Text,
                Categoria = pkrCategoria.SelectedItem.ToString(),
                Material = txtMaterial.Text,
                Descripcion = txtDescripcion.Text,
                // Si el usuario no escribe una imagen, usamos el robot por defecto
                Imagen = string.IsNullOrWhiteSpace(txtImagen.Text) ? "dotnet_bot.png" : txtImagen.Text
            };

            // 3. Guardamos el producto en la base de datos SQLite
            var dbService = new DatabaseService();
            await dbService.GuardarProductoAsync(nuevoProducto);

            // 4. Mostramos mensaje de éxito
            await DisplayAlert("Éxito", "Producto guardado correctamente.", "OK");

            // 5. Regresamos automáticamente a la pantalla del Catálogo
            await Navigation.PopAsync();
        }

        // Este método se ejecuta al presionar el botón "Cancelar"
        private async void OnCancelarClicked(object sender, EventArgs e)
        {
            // Simplemente regresamos a la pantalla anterior sin guardar nada
            await Navigation.PopAsync();
        }
    }
}
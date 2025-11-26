using System;
using Microsoft.Maui.Controls;
using Gestor_De_Ventas_Para_Piezas_3D.Modelos;
using Gestor_De_Ventas_Para_Piezas_3D.Services;
using System.IO; // Necesario para manejar archivos

namespace Gestor_De_Ventas_Para_Piezas_3D.Vistas
{
    public partial class AgregarProductoPage : ContentPage
    {
        private string _rutaImagenGuardada; // Aquí guardaremos la ruta de la foto elegida

        public AgregarProductoPage()
        {
            InitializeComponent();
            // Imagen por defecto si no eligen nada
            _rutaImagenGuardada = "dotnet_bot.png";
        }

        // Lógica para abrir la galería
        private async void OnSeleccionarImagenClicked(object sender, EventArgs e)
        {
            try
            {
                // Abrimos el selector de archivos (solo imágenes)
                var resultado = await FilePicker.Default.PickAsync(new PickOptions
                {
                    PickerTitle = "Selecciona una imagen",
                    FileTypes = FilePickerFileType.Images
                });

                if (resultado != null)
                {
                    // Copiamos la imagen a la carpeta de datos de la app para que no se pierda
                    var nuevoNombre = $"{Guid.NewGuid()}{Path.GetExtension(resultado.FileName)}";
                    var rutaDestino = Path.Combine(FileSystem.AppDataDirectory, nuevoNombre);

                    using (var streamOrigen = await resultado.OpenReadAsync())
                    using (var streamDestino = File.Create(rutaDestino))
                    {
                        await streamOrigen.CopyToAsync(streamDestino);
                    }

                    // Guardamos la ruta para la base de datos
                    _rutaImagenGuardada = rutaDestino;

                    // Mostramos la previsualización
                    imgPreview.Source = ImageSource.FromFile(rutaDestino);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo cargar la imagen: {ex.Message}", "OK");
            }
        }

        private async void OnGuardarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || pkrCategoria.SelectedIndex == -1)
            {
                await DisplayAlert("Faltan Datos", "Por favor escribe un nombre y selecciona una categoría.", "OK");
                return;
            }

            var nuevoProducto = new ModeloReference
            {
                Nombre = txtNombre.Text,
                Categoria = pkrCategoria.SelectedItem.ToString(),
                Material = txtMaterial.Text,
                Descripcion = txtDescripcion.Text,

                // AQUÍ USAMOS LA RUTA DE LA IMAGEN QUE SELECCIONARON
                Imagen = _rutaImagenGuardada
            };

            var dbService = new DatabaseService();
            await dbService.GuardarProductoAsync(nuevoProducto);

            await DisplayAlert("Éxito", "Producto guardado correctamente.", "OK");
            await Navigation.PopAsync();
        }

        private async void OnCancelarClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
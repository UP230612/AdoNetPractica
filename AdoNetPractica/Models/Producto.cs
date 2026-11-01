// Esta clase representa un producto en mi base de datos
// Cada propiedad aqui es como una columna de la tabla Productos en SQL Server

namespace AdoNetPractica.Models
{
    public class Producto
    {
        // El Id es el numero unico que identifica a cada producto
        // La base de datos lo genera sola, no lo pongo yo a mano
        public int Id { get; set; }

        // El nombre del producto, por ejemplo "Laptop HP" o "Mouse Logitech"
        public string Nombre { get; set; } = string.Empty;

        // Una descripcion corta de para que sirve el producto
        public string Descripcion { get; set; } = string.Empty;

        // El precio del producto en pesos
        public decimal Precio { get; set; }

        // Cuantos productos hay disponibles en almacen
        public int Existencia { get; set; }
    }
}

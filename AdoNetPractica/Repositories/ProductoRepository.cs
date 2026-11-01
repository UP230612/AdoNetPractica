// Aqui es donde escribo todo el codigo para hablar con la base de datos
// Uso ADO.NET que es la forma "directa" de conectarse a SQL Server desde .NET
// ADO.NET significa que yo escribo el SQL a mano, sin que ningun programa lo haga por mi

using Microsoft.Data.SqlClient;
using AdoNetPractica.Models;

namespace AdoNetPractica.Repositories
{
    // Esta clase implementa la interfaz, o sea, tiene que tener todos los metodos que pide
    public class ProductoRepository : IProductoRepository
    {
        // Guardo aqui la cadena de conexion para no escribirla en cada metodo
        private readonly string _connectionString;

        // El constructor recibe la configuracion de la app para leer el appsettings.json
        public ProductoRepository(IConfiguration configuration)
        {
            // Le digo que busque la conexion que se llama "DefaultConnection"
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        // ============================================================
        // OBTENER TODOS - Consulta
        // Trae todos los productos que estan guardados en la tabla
        // ============================================================
        public List<Producto> ObtenerTodos()
        {
            // Creo una lista donde voy a ir guardando los productos que lea de la base de datos
            var lista = new List<Producto>();

            // Abro la conexion a SQL Server usando la cadena de conexion que guarde arriba
            using var conexion = new SqlConnection(_connectionString);
            conexion.Open();

            // Escribo la consulta SQL que quiero ejecutar
            var comando = new SqlCommand(
                "SELECT Id, Nombre, Descripcion, Precio, Existencia FROM Productos",
                conexion
            );

            // Ejecuto la consulta y con el lector voy leyendo fila por fila
            using var lector = comando.ExecuteReader();
            while (lector.Read())
            {
                // Por cada fila que encuentra, creo un objeto Producto y lo agrego a la lista
                lista.Add(new Producto
                {
                    Id          = lector.GetInt32(0),   // columna 0 = Id
                    Nombre      = lector.GetString(1),   // columna 1 = Nombre
                    Descripcion = lector.GetString(2),   // columna 2 = Descripcion
                    Precio      = lector.GetDecimal(3),  // columna 3 = Precio
                    Existencia  = lector.GetInt32(4)     // columna 4 = Existencia
                });
            }

            return lista;
        }

        // ============================================================
        // OBTENER POR ID - Buscar
        // Busca un producto especifico usando su numero de Id
        // ============================================================
        public Producto? ObtenerPorId(int id)
        {
            using var conexion = new SqlConnection(_connectionString);
            conexion.Open();

            // Uso @Id en lugar de poner el numero directo, eso es mas seguro y evita inyecciones SQL
            var comando = new SqlCommand(
                "SELECT Id, Nombre, Descripcion, Precio, Existencia FROM Productos WHERE Id = @Id",
                conexion
            );

            // Aqui le digo cuanto vale @Id
            comando.Parameters.AddWithValue("@Id", id);

            using var lector = comando.ExecuteReader();

            // Si encontro una fila, regreso el producto. Si no, regreso null (significa que no existe)
            if (lector.Read())
            {
                return new Producto
                {
                    Id          = lector.GetInt32(0),
                    Nombre      = lector.GetString(1),
                    Descripcion = lector.GetString(2),
                    Precio      = lector.GetDecimal(3),
                    Existencia  = lector.GetInt32(4)
                };
            }

            return null;
        }

        // ============================================================
        // INSERTAR - Insert
        // Guarda un producto nuevo en la base de datos
        // Regresa el Id que la base de datos le asigno al nuevo producto
        // ============================================================
        public int Insertar(Producto producto)
        {
            using var conexion = new SqlConnection(_connectionString);
            conexion.Open();

            // OUTPUT INSERTED.Id es un truco de SQL Server para que me diga el Id que genero
            var sql = @"
                INSERT INTO Productos (Nombre, Descripcion, Precio, Existencia)
                OUTPUT INSERTED.Id
                VALUES (@Nombre, @Descripcion, @Precio, @Existencia)
            ";

            var comando = new SqlCommand(sql, conexion);

            // Paso cada campo del producto como parametro
            comando.Parameters.AddWithValue("@Nombre",      producto.Nombre);
            comando.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
            comando.Parameters.AddWithValue("@Precio",      producto.Precio);
            comando.Parameters.AddWithValue("@Existencia",  producto.Existencia);

            // ExecuteScalar ejecuta la consulta y regresa el primer valor, que es el nuevo Id
            var nuevoId = comando.ExecuteScalar();
            return Convert.ToInt32(nuevoId);
        }

        // ============================================================
        // ACTUALIZAR - Actualizar
        // Cambia los datos de un producto que ya existe
        // Regresa true si se actualizo bien, false si no encontro el producto
        // ============================================================
        public bool Actualizar(Producto producto)
        {
            using var conexion = new SqlConnection(_connectionString);
            conexion.Open();

            var sql = @"
                UPDATE Productos
                SET Nombre      = @Nombre,
                    Descripcion = @Descripcion,
                    Precio      = @Precio,
                    Existencia  = @Existencia
                WHERE Id = @Id
            ";

            var comando = new SqlCommand(sql, conexion);

            comando.Parameters.AddWithValue("@Id",          producto.Id);
            comando.Parameters.AddWithValue("@Nombre",      producto.Nombre);
            comando.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
            comando.Parameters.AddWithValue("@Precio",      producto.Precio);
            comando.Parameters.AddWithValue("@Existencia",  producto.Existencia);

            // ExecuteNonQuery dice cuantas filas cambio. Si es mayor a cero funciono bien
            int filasAfectadas = comando.ExecuteNonQuery();
            return filasAfectadas > 0;
        }

        // ============================================================
        // ELIMINAR - Eliminar
        // Borra un producto de la base de datos usando su Id
        // Regresa true si lo borro, false si no encontro nada con ese Id
        // ============================================================
        public bool Eliminar(int id)
        {
            using var conexion = new SqlConnection(_connectionString);
            conexion.Open();

            var comando = new SqlCommand("DELETE FROM Productos WHERE Id = @Id", conexion);
            comando.Parameters.AddWithValue("@Id", id);

            int filasAfectadas = comando.ExecuteNonQuery();
            return filasAfectadas > 0;
        }
    }
}

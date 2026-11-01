// Esta es la interfaz que el maestro pide implementar
// Una interfaz es como un contrato: cualquier clase que la use TIENE que tener estos metodos
// Es util porque si algun dia cambio la base de datos, no tengo que cambiar todo el proyecto

using AdoNetPractica.Models;

namespace AdoNetPractica.Repositories
{
    public interface IProductoRepository
    {
        // Trae la lista completa de todos los productos que estan en la base de datos
        List<Producto> ObtenerTodos();

        // Busca y regresa un producto por su Id. Si no existe regresa null (nada)
        Producto? ObtenerPorId(int id);

        // Guarda un producto nuevo en la base de datos y regresa el Id que le asigno
        int Insertar(Producto producto);

        // Cambia la informacion de un producto que ya existe. Regresa true si funciono, false si no
        bool Actualizar(Producto producto);

        // Borra un producto por su Id. Regresa true si se borro, false si no encontro nada
        bool Eliminar(int id);
    }
}

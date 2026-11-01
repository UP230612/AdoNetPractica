// Este es el archivo principal de mi proyecto, el punto de arranque
// Aqui configuro todo: los servicios, el swagger y los endpoints (rutas de la API)

using AdoNetPractica.Models;
using AdoNetPractica.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Esto agrega el swagger, que es una pagina web para probar mi API desde el navegador
// Uso Swashbuckle que es la libreria mas comun para swagger en .NET
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registro el repositorio para que la aplicacion sepa que clase usar cuando se lo pida
// Esto se llama inyeccion de dependencias: le digo que cuando alguien pida IProductoRepository,
// le de un ProductoRepository que ya sabe conectarse a la base de datos
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

var app = builder.Build();

// Activo el swagger para que aparezca en el navegador al correr la aplicacion
app.UseSwagger();
app.UseSwaggerUI();

// ============================================================
// ENDPOINTS - Son las rutas que puede usar alguien para hablar con mi API
// ============================================================

// GET /productos
// Ruta para traer TODOS los productos de la base de datos
app.MapGet("/productos", (IProductoRepository repo) =>
{
    var productos = repo.ObtenerTodos();
    return Results.Ok(productos);
})
.WithName("ObtenerTodos");

// GET /productos/{id}
// Ruta para buscar un producto especifico por su Id
app.MapGet("/productos/{id}", (int id, IProductoRepository repo) =>
{
    var producto = repo.ObtenerPorId(id);

    // Si no encontro el producto, mando un error 404 con un mensaje
    if (producto is null)
        return Results.NotFound(new { mensaje = "No se encontro un producto con ese Id" });

    return Results.Ok(producto);
})
.WithName("ObtenerPorId");

// POST /productos
// Ruta para agregar un producto nuevo
app.MapPost("/productos", (Producto producto, IProductoRepository repo) =>
{
    var nuevoId = repo.Insertar(producto);

    // Regresa un 201 (creado) y la ruta donde se puede ver el nuevo producto
    return Results.Created($"/productos/{nuevoId}", new { id = nuevoId, mensaje = "Producto guardado correctamente" });
})
.WithName("Insertar");

// PUT /productos/{id}
// Ruta para actualizar un producto que ya existe
app.MapPut("/productos/{id}", (int id, Producto producto, IProductoRepository repo) =>
{
    // Me aseguro de usar el Id que viene en la URL, no el que venga en el cuerpo del JSON
    producto.Id = id;

    var seActualizo = repo.Actualizar(producto);

    if (!seActualizo)
        return Results.NotFound(new { mensaje = "No se encontro el producto para actualizar" });

    return Results.Ok(new { mensaje = "Producto actualizado correctamente" });
})
.WithName("Actualizar");

// DELETE /productos/{id}
// Ruta para eliminar un producto por su Id
app.MapDelete("/productos/{id}", (int id, IProductoRepository repo) =>
{
    var seElimino = repo.Eliminar(id);

    if (!seElimino)
        return Results.NotFound(new { mensaje = "No se encontro el producto para eliminar" });

    return Results.Ok(new { mensaje = "Producto eliminado correctamente" });
})
.WithName("Eliminar");

app.Run();

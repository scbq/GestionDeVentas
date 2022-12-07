using Microsoft.AspNetCore.Mvc;
using GestionDeVentas.Repository;
using GestionDeVentas.Models;



namespace GestionDeVentas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : Controller
    {
        private ProductoRepository repository = new ProductoRepository();

        [HttpGet]
        public ActionResult<List<Producto>> Get()
        {
            try
            {
                List<Producto> lista = repository.listarProducto();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Producto> Get(int id)
        {
            try
            {
                Producto? producto = repository.obtenerProducto(id);
                if (producto != null)
                {
                    return Ok(producto);
                }
                else
                {
                    return NotFound("El producto no fue encontrado");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] Producto producto)
        {
            try
            {
                Producto productoCreado = repository.crearProducto(producto);
                return StatusCode(StatusCodes.Status201Created, productoCreado);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete]
        public ActionResult Delete([FromBody] int id)
        {
            try
            {
                bool seElimino = repository.eliminarProducto(id);
                if (seElimino)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("{id}")]

        public ActionResult<Producto> Put(long id, [FromBody]Producto productoAActualizar)
        {
            try
            {
                Producto? productoActualizado = repository.ActualizarProducto(id, productoAActualizar);
                if (productoActualizado != null)
                {
                    return Ok(productoActualizado);
                }
                else
                {
                    return NotFound("El producto no fue encontrado");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using GestionDeVentas.Repository;
using GestionDeVentas.Models;

namespace GestionDeVentas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosVendidosController : Controller
    {
        private ProductoVendidoRepository repository = new ProductoVendidoRepository();

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<ProductoVendido> lista = repository.listarProductoVendido();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ProductoVendido> Get(int id)
        {
            try
            {
                ProductoVendido? productoVendido = repository.obtenerProductoVendido(id);
                if (productoVendido != null)
                {
                    return Ok(productoVendido);
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
        public ActionResult Post([FromBody] ProductoVendido productoVendido)
        {
            try
            {
                ProductoVendido productoVendidoCreado = repository.crearProductoVendido(productoVendido);
                return StatusCode(StatusCodes.Status201Created, productoVendidoCreado);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete]
        public ActionResult Delete([FromBody] long id)
        {
            try
            {
                bool seElimino = repository.eliminarProductoVendido(id);
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

        public ActionResult<ProductoVendido> Put(long id, [FromBody] ProductoVendido productoVendidoAActualizar)
        {
            try
            {
                ProductoVendido? productoVendidoActualizado = repository.ActualizarProductoVendido(id, productoVendidoAActualizar);
                if (productoVendidoActualizado != null)
                {
                    return Ok(productoVendidoActualizado);
                }
                else
                {
                    return NotFound("El producto vendido no fue encontrado");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}

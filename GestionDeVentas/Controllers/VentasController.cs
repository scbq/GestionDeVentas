using Microsoft.AspNetCore.Mvc;
using GestionDeVentas.Repository;
using GestionDeVentas.Models;

namespace GestionDeVentas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentasController : Controller
    {
        private VentaRepository repository = new VentaRepository();

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<Venta> lista = repository.listarVenta();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Venta> Get(int id)
        {
            try
            {
                Venta? venta = repository.obtenerVenta(id);
                if (venta != null)
                {
                    return Ok(venta);
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
        public ActionResult Post([FromBody] Venta venta)
        {
            try
            {
                Venta ventaCreada = repository.crearVenta(venta);
                return StatusCode(StatusCodes.Status201Created, ventaCreada);
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
                bool seElimino = repository.eliminarVenta(id);
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

        public ActionResult<Venta> Put(long id, [FromBody] Venta ventaAActualizar)
        {
            try
            {
                Venta? ventaActualizada = repository.ActualizarVenta(id, ventaAActualizar);
                if (ventaActualizada != null)
                {
                    return Ok(ventaActualizada);
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

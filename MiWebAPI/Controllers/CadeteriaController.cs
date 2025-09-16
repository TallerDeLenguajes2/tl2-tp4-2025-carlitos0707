using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Lector;
using Cadeterias;
namespace MiWebAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class CadeteriaController : ControllerBase
{

    [HttpGet("Pedidos")]
    public IActionResult GetPedidos()
    {

        return Ok(AccesoADatosJson.AbrirArchivoTexto("Datos/pedidos.json"));
    }


    [HttpGet("Cadetes")]
    public IActionResult GetCadetes()
    {
        return Ok(AccesoADatosJson.AbrirArchivoTexto("Datos/cadetes.json"));
    }

    [HttpGet("Informe")]
    public IActionResult GetInforme()
    {
        return Ok(AccesoADatosJson.AbrirArchivoTexto("Datos/informe.json"));
    }

    [HttpPost("AgregarPedido")]
    public IActionResult AgregarPedido(Pedido? pedido)
    {
        if (pedido == null)
        {
            return BadRequest("Pedido Invalido");
        }
        else
        {
            string stringPedidos = AccesoADatosJson.AbrirArchivoTexto("Datos/pedidos.json");
            AccesoADatosJson lector = new AccesoADatosJson();
            List<Pedido> pedidos = new List<Pedido>();
            pedidos.AddRange(lector.AbrirPedidos(stringPedidos));
            pedidos.Add(pedido);
            lector.GuardarArchivoTexto("Datos/pedidos.json", pedidos);
            return Ok("Pedido Agregado");
        }
    }
}
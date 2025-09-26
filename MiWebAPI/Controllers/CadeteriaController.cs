using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Lector;
using Cadeterias;
namespace MiWebAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class CadeteriaController : ControllerBase
{
    static AccesoADatosJson lector = new AccesoADatosJson();

    [HttpGet("Pedidos")]
    public IActionResult GetPedidos()
    {
        List<Pedido> pedidos = lector.AbrirPedidos("Datos/pedidos.json");
        return Ok(pedidos);
    }


    [HttpGet("Cadetes")]
    public IActionResult GetCadetes()
    {
        List<Cadete> cadetes = lector.AbrirCadetes("Datos/cadetes.json");
        return Ok(cadetes);
    }

    [HttpGet("Informe")]
    public IActionResult GetInforme()
    {
        Informe informe = lector.AbrirInformes("Datos/informe.json");
        return Ok(informe);
    }

    [HttpPost("AgregarPedido")]
    public IActionResult AgregarPedido(Pedido pedido)
    {
        if (pedido == null)
        {
            return BadRequest("Pedido Invalido");
        }
        List<Pedido> pedidos = lector.AbrirPedidos("Datos/pedidos.json");
        pedidos.Add(pedido);
        lector.GuardarArchivoTexto("Datos/pedidos.json", pedidos);
        return Ok(pedidos);
    }

    [HttpPut("AsignarPedido")]
    public IActionResult AsignarPedido(int idPedido, int idCadete)
    {
        List<Pedido> pedidos = lector.AbrirPedidos("Datos/pedidos.json");
        List<Cadete> cadetes = lector.AbrirCadetes("Datos/cadetes.json");
        Pedido pedido = pedidos.Find(x => x.Nro == idPedido);
        Cadete cadete = cadetes.Find(x => x.Id == idCadete);
        if (pedido == null)
        {
            return BadRequest("No se encontro el pedido");
        }
        if (cadete == null)
        {
            return BadRequest("No se encontro el cadete");
        }
        pedido.Cadete = cadete;
        lector.GuardarArchivoTexto("Datos/pedidos.json", pedidos);
        return Ok("Pedido asignado");
    }

    [HttpPut("CambiarEstadoPedido")]
    public IActionResult CambiarEstadoPedido(int idPedido, bool estado)
    {
        List<Pedido> pedidos = lector.AbrirPedidos("Datos/pedidos.json");
        if (pedidos == null)
        {
            return StatusCode(500, "Error del servidor");
        }
        Pedido pedido = pedidos.Find(x => x.Nro == idPedido);
        if (pedido == null)
        {
            return BadRequest("No se encontro el pedido");            
        }
        pedido.Entregado = estado;
        lector.GuardarArchivoTexto("Datos/pedidos.json",pedidos);
        return Ok("Se cambio el estado del pedido");
    }

    [HttpPut("CambiarCadetePedido")]
    public IActionResult CambiarCadetePedido(int idPedido, int idNuevoCadete)
    {
        IActionResult res = AsignarPedido(idPedido, idNuevoCadete);
        return res;
    }
}
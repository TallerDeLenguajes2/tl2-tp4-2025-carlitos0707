using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Lector;
using Cadeterias;
namespace MiWebAPI.Controllers;


[ApiController]
[Route("[controller]")]
public class CadeteriaController : ControllerBase
{
    private accesoDatosCadeteria ADCadeteria;
    private accesoDatosCadetes ADCadetes;
    private accesoDatosPedidos ADPedidos;
    private accsesoDatosInforme ADInforme;

    public CadeteriaController()
    {
        ADCadeteria = new accesoDatosCadeteria();
        ADCadetes = new accesoDatosCadetes();
        ADPedidos = new accesoDatosPedidos();
        ADInforme = new accsesoDatosInforme();
    }

    [HttpGet("Pedidos")]
    public IActionResult GetPedidos()
    {
        List<Pedido> pedidos = ADPedidos.Obtener();
        return Ok(pedidos);
    }


    [HttpGet("Cadetes")]
    public IActionResult GetCadetes()
    {
        List<Cadete> cadetes = ADCadetes.Obtener();
        return Ok(cadetes);
    }

    [HttpGet("Informe")]
    public IActionResult GetInforme()
    {
        Informe informe = ADInforme.Obtener();
        return Ok(informe);
    }

    [HttpPost("AgregarPedido")]
    public IActionResult AgregarPedido(Pedido pedido)
    {
        if (pedido == null)
        {
            return BadRequest("Pedido Invalido");
        }
        List<Pedido> pedidos = ADPedidos.Obtener();
        pedidos.Add(pedido);
        ADPedidos.GuardarPedidos(pedidos);
        return Created("Pedido creado",pedidos);
    }

    [HttpPut("AsignarPedido")]
    public IActionResult AsignarPedido(int idPedido, int idCadete)
    {
        List<Pedido> pedidos = ADPedidos.Obtener();
        List<Cadete> cadetes = ADCadetes.Obtener();
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
        ADPedidos.GuardarPedidos(pedidos);
        return Ok("Pedido asignado");
    }

    [HttpPut("CambiarEstadoPedido")]
    public IActionResult CambiarEstadoPedido(int idPedido, bool estado)
    {
        List<Pedido> pedidos = ADPedidos.Obtener();
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
        ADPedidos.GuardarPedidos(pedidos);
        return Ok("Se cambio el estado del pedido");
    }

    [HttpPut("CambiarCadetePedido")]
    public IActionResult CambiarCadetePedido(int idPedido, int idNuevoCadete)
    {
        IActionResult res = AsignarPedido(idPedido, idNuevoCadete);
        return res;
    }
}
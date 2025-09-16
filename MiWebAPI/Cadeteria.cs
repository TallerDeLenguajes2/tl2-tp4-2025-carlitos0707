namespace Cadeterias;
using System.Linq;

public class Pedido
{
    int nro;
    string obs;
    Cliente cliente;
    Cadete cadete;
    bool entregado;
    public int Nro { get => nro; }
    public bool Entregado { get => entregado; set => entregado = value; }
    public string Obs { get => obs; set => obs = value; }
    public Cliente Cliente { get => cliente; set => cliente = value; }
    public Cadete Cadete { get => cadete; set => cadete = value; }

    public Pedido(int nro, string obs, string nombreCliente, string direccion, int telefono, string datosReferenciaDireccion, bool entregado)
    {
        this.cliente = new Cliente(nombreCliente, direccion, telefono, datosReferenciaDireccion);
        this.entregado = entregado;
        this.nro = nro;
        this.obs = obs;
    }
    public Pedido()
    {
        
    }
}


public class Cliente
{
    string nombre;
    string direccion;
    int telefono;
    string datosReferenciaDireccion;

    public string Nombre { get => nombre; set => nombre = value; }
    public string Direccion { get => direccion; set => direccion = value; }
    public int Telefono { get => telefono; set => telefono = value; }
    public string DatosReferenciaDireccion { get => datosReferenciaDireccion; set => datosReferenciaDireccion = value; }

    public Cliente(string nombre, string direccion, int telefono, string datosReferenciaDireccion)
    {
        this.DatosReferenciaDireccion = datosReferenciaDireccion;
        this.nombre = nombre;
        this.direccion = direccion;
        this.telefono = telefono;
    }

    public void VerDireccionCliente()
    {
        Console.WriteLine($"Direccion: {this.direccion}");
        Console.WriteLine($"Datos de referencia Direccion: {this.datosReferenciaDireccion}");
    }
    public void VerDatosCliente()
    {
        Console.WriteLine($"Nombre: {this.nombre}");
        Console.WriteLine($"Telefono: {this.telefono}");
    }
}


public class Cadete
{
    int id;
    string nombre;
    string direccion;
    int telefono;
    public int Id { get => id; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string Direccion { get => direccion; set => direccion = value; }
    public int Telefono { get => telefono; set => telefono = value; }

    public Cadete(int id, string nombre, string direccion, int telefono)
    {
        this.id = id;
        this.nombre = nombre;
        this.direccion = direccion;
        this.telefono = telefono;
    }

}


public class Cadeteria
{
    string nombre;
    int telefono;
    List<Cadete> listadoCadetes;
    List<Pedido> listadoPedidos;
    public List<Cadete> ListadoCadetes { get => listadoCadetes; set => listadoCadetes = value; }
    public string Nombre { get => nombre; }
    public int Telefono { get => telefono; }
    public List<Pedido> ListadoPedidos { get => listadoPedidos; set => listadoPedidos = value; }

    public Cadeteria(string nombre, int telefono)
    {
        this.nombre = nombre;
        this.telefono = telefono;
        this.listadoPedidos = new List<Pedido>();
        this.listadoCadetes = new List<Cadete>();
    }
    public void AsignarPedido(int nro, int id)
    {
        Pedido pedido = listadoPedidos.Single(pedido => pedido.Nro == nro);
        Cadete cadete = listadoCadetes.Single(cadete => cadete.Id == id);
        pedido.Cadete = cadete;
    }

    public void MarcarEntregado(int nro)
    {
        Pedido pedido = listadoPedidos.Single(pedido => pedido.Nro == nro);
        pedido.Entregado = true;
    }
    public double JornalACobrar(int id)
    {
        double jornal = 0;
        foreach (Pedido pedido in ListadoPedidos)
        {
            if (pedido.Entregado && pedido.Cadete.Id == id)
            {
                jornal += 500;
            }
        }
        return jornal;
    }

    public void AgregarPedido(int nro, string obs, string nombreCliente, string direccion, int telefono, string datosReferenciaDireccion, bool entregado)
    {
        Pedido pedido = new Pedido(nro, obs, nombreCliente, direccion, telefono, datosReferenciaDireccion, entregado);
        this.listadoPedidos.Add(pedido);
    }
    public void AgregarPedido(Pedido pedido)
    {
        this.listadoPedidos.Add(pedido);
    }
    public void AgregarCadete(int id, string nombre, string direccion, int telefono)
    {
        Cadete cadete = new Cadete(id,nombre,direccion,telefono);
        this.listadoCadetes.Add(cadete);
    }

    public void AgregarCadete(Cadete cadete)
    {
        this.listadoCadetes.Add(cadete);
    }

    public double CalcularSueldo(Cadete cadete, List<Pedido> pedidos)
    {
        double total = 0;

        foreach (Pedido pedido in pedidos)
        {
            if (pedido.Cadete.Id == cadete.Id && pedido.Entregado)
            {
                total += 500;
            }
        }
        return total;
    }
    public string InformeJornada()
    {
        string informe = "";
        Console.Clear();
        informe = "========= INFORME DE JORNADA =========\n\n";

        double pagoPorPedido = 500; // monto por pedido entregado
        int totalPedidos = 0;
        double totalRecaudado = 0;

        foreach (var cadete in listadoCadetes)
        {
            // Contamos los pedidos entregados de este cadete
            int entregados = listadoPedidos
                .Count(p => p.Entregado && p.Cadete != null && p.Cadete.Id == cadete.Id);

            double monto = entregados * pagoPorPedido;

            informe += $"Cadete: {cadete.Nombre} (ID {cadete.Id})\n";
            informe += "\t- Pedidos entregados: {entregados}\n";
            informe += "\t- Monto ganado: ${monto}\n";
            informe += "----------------------------------\n";

            totalPedidos += entregados;
            totalRecaudado += monto;
        }

        double promedio = listadoCadetes.Count > 0
            ? (double)totalPedidos / listadoCadetes.Count
            : 0;

        informe += $"TOTAL de pedidos entregados: {totalPedidos}\n";
        informe += $"TOTAL recaudado: ${totalRecaudado}\n";
        informe += $"Promedio de envíos por cadete: {promedio:F2}\n";
        informe += "===================================\n";
        return informe;
    }

}


public class Informe
{
    Cadeteria cadeteria;
    List<Cadete> cadetes;
    List<Pedido> pedidosCompletados;
    Dictionary<string, double> sueldos;
    public Cadeteria Cadeteria { get => cadeteria; set => cadeteria = value; }
    public List<Cadete> Cadetes { get => cadetes; }
    public List<Pedido> PedidosCompletados { get => pedidosCompletados; }
    public Dictionary<string, double> Sueldos { get => sueldos; }

    public Informe(Cadeteria cadeteria)
    {
        this.cadeteria = cadeteria;
        this.cadetes = cadeteria.ListadoCadetes;
        this.pedidosCompletados = cadeteria.ListadoPedidos.FindAll(pedido => pedido.Entregado == true);
        foreach (Cadete cadete in cadetes)
        {
            this.sueldos[cadete.Nombre] = cadeteria.CalcularSueldo(cadete, this.pedidosCompletados);
        }
        
    }
}

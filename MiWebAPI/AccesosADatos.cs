using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks.Dataflow;
using Cadeterias;
using Microsoft.AspNetCore.Identity.Data;
namespace Lector
{
    public class AccesoADatosJson
    {

        public static string AbrirArchivoTexto(string nombreArchivo)
        {
            string documento;
            using (var archivoOpen = new FileStream(nombreArchivo, FileMode.Open))
            {
                using (var strReader = new StreamReader(archivoOpen))
                {
                    documento = strReader.ReadToEnd();
                    archivoOpen.Close();
                }
            }
            return documento;
        }

        public static void GuardarArchivo(string nombreArchivo, string datos)
        {
            using (var archivo = new FileStream(nombreArchivo, FileMode.Create))
            {
                using (var strWriter = new StreamWriter(archivo))
                {
                    strWriter.WriteLine("{0}", datos);
                    strWriter.Close();
                }
            }
        }
    }


    class accesoDatosCadeteria : AccesoADatosJson
    {
        static string nombreArchivo = "Datos/cadeteria.json";
        public Cadeteria Obtener()
        {
            string jsonString = AbrirArchivoTexto(nombreArchivo);
            Cadeteria cadeteria = JsonSerializer.Deserialize<Cadeteria>(jsonString);
            return cadeteria;
        }
        public void GuardarCadeteria(Cadeteria cadeteria)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(cadeteria,options);
            GuardarArchivo(nombreArchivo, jsonString);

        }
    }
    class accesoDatosCadetes : AccesoADatosJson
    {
        static string nombreArchivo = "Datos/cadetes.json";
        public List<Cadete> Obtener()
        {
            string jsonString = AbrirArchivoTexto(nombreArchivo);
            List<Cadete> cadetes = JsonSerializer.Deserialize<List<Cadete>>(jsonString);
            return cadetes;
        }
        public void GuardarCadetes(List<Cadete> cadetes)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(cadetes,options);
            GuardarArchivo(nombreArchivo, jsonString);
        }

    }
    class accesoDatosPedidos : AccesoADatosJson
    {
        static string nombreArchivo = "Datos/pedidos.json";
        public List<Pedido> Obtener()
        {
            string jsonString = AbrirArchivoTexto(nombreArchivo);
            List<Pedido> pedidos = JsonSerializer.Deserialize<List<Pedido>>(jsonString);
            return pedidos;
        }
        public void GuardarPedidos(List<Pedido> pedidos)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(pedidos,options);
            GuardarArchivo(nombreArchivo, jsonString);
        }
    }
    class accsesoDatosInforme : AccesoADatosJson
    {
        static string nombreArchivo = "Datos/informe.json";
        public Informe Obtener()
        {
            string jsonString = AbrirArchivoTexto(nombreArchivo);
            Informe informe = JsonSerializer.Deserialize<Informe>(jsonString);
            return informe;
        }
        public void GuardarArchivoTexto(List<Informe> informes)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(informes,options);
            GuardarArchivo(nombreArchivo, jsonString);
        }
    }
}
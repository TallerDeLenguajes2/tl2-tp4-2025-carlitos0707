using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Cadeterias;
using Microsoft.AspNetCore.Identity.Data;
namespace Lector
{

    public interface IAccesoADatos
    {
        List<Cadeteria> AbrirCadeteria(string nombreArchivo);

        List<Cadete> AbrirCadetes(string nombreArchivo);

        List<Pedido> AbrirPedidos(string nombreArchivo);
        void GuardarArchivoTexto(string nombreArchivo,List<Cadeteria> cadeterias);
        void GuardarArchivoTexto(string nombreArchivo, List<Cadete> cadetes);
        void GuardarArchivoTexto(string nombreArchivo, List<Pedido> pedidos);
        void GuardarArchivoTexto(string nombreArchivo, List<Informe> informes);
        void migrarDatos(string csv, string json,string tipo);
    }
    public class AccesoADatosCSV : IAccesoADatos
    {
        List<string[]> abrirCsv(string nombreDeArchivo)
        {
            FileStream MiArchivo = new FileStream(nombreDeArchivo, FileMode.Open);
            StreamReader StrReader = new StreamReader(MiArchivo);
            string Linea = "";
            List<string[]> LecturaDelArchivo = new List<string[]>();

            while ((Linea = StrReader.ReadLine()) != null)
            {
                string[] Fila = Linea.Split(',');
                LecturaDelArchivo.Add(Fila);
            }

            return LecturaDelArchivo;
        }
        public List<Cadeteria> AbrirCadeteria(string nombreDeArchivo)
        {
            List<string[]> LecturaDelArchivo = abrirCsv(nombreDeArchivo);
            List<Cadeteria> cadeterias = Cadeterias(LecturaDelArchivo);
            return cadeterias;
        }


        public List<Cadete> AbrirCadetes(string nombreArchivo)
        {
            List<string[]> stringCadetes = abrirCsv(nombreArchivo);

            List<Cadete> cadetes = Cadetes(stringCadetes);

            return cadetes;
        }

        List<Cadeteria> Cadeterias(List<string[]> stringCadeterias)
        {
            List<Cadeteria> cadeterias = new List<Cadeteria>();
            foreach (string[] stringCadeteria in stringCadeterias)
            {
                Cadeteria cadeteria = new Cadeteria(stringCadeteria[0], int.Parse(stringCadeteria[1]));
                cadeterias.Add(cadeteria);
            }


            return cadeterias;
        }

        List<Cadete> Cadetes(List<string[]> stringCadetes)
        {
            List<Cadete> cadetes = new List<Cadete>();
            foreach (string[] stringCadete in stringCadetes)
            {
                Cadete cadete = new Cadete(int.Parse(stringCadete[0]), stringCadete[1], stringCadete[2], int.Parse(stringCadete[3]));
                cadetes.Add(cadete);
            }
            return cadetes;
        }

        public List<Pedido> AbrirPedidos(string nombreArchivo)
        {
            List<Pedido> pedidos = new List<Pedido>();

            return pedidos;
        }

        public void migrarDatos(string csv, string json, string tipo)
        {
            if (tipo.ToLower() == "cadetes")
            {
                var cadetes = AbrirCadetes(csv);
                string jsonString = JsonSerializer.Serialize(cadetes, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(json, jsonString);
            }
            else if (tipo.ToLower() == "cadeterias")
            {
                var cadeterias = AbrirCadeteria(csv);
                string jsonString = JsonSerializer.Serialize(cadeterias, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(json, jsonString);
            }
            else
            {
                throw new ArgumentException("Tipo de entidad no válido. Use 'cadetes' o 'cadeterias'.");
            }
        }



        public void GuardarArchivoTexto(string nombreArchivo, List<Cadeteria> cadeterias)
        {
            throw new NotImplementedException();
        }

        public void GuardarArchivoTexto(string nombreArchivo, List<Cadete> cadetes)
        {
            throw new NotImplementedException();
        }
        public void GuardarArchivoTexto(string nombreArchivo, List<Pedido> pedidos)
        {
            throw new NotImplementedException();
        }
        public void GuardarArchivoTexto(string nombreArchivo, List<Informe> informes)
        {
            throw new NotImplementedException();
        }
    }



    public class AccesoADatosJson : IAccesoADatos
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



        public List<Cadeteria> AbrirCadeteria(string nombreArchivo)
        {
            string jsonString = AbrirArchivoTexto(nombreArchivo);
            List<Cadeteria> cadeterias = JsonSerializer.Deserialize<List<Cadeteria>>(jsonString);
            return cadeterias;
        }

        public List<Cadete> AbrirCadetes(string nombreArchivo)
        {
            string jsonString = AbrirArchivoTexto(nombreArchivo);
            List<Cadete> cadetes = JsonSerializer.Deserialize<List<Cadete>>(jsonString);
            return cadetes;
        }

        public List<Pedido> AbrirPedidos(string nombreArchivo)
        {
            List<Pedido> pedidos = new List<Pedido>();

            return pedidos;
        }
        public void GuardarArchivoTexto(string nombreArchivo, List<Cadeteria> cadeterias)
        {
            string jsonString = JsonSerializer.Serialize(cadeterias);
            GuardarArchivo(nombreArchivo, jsonString);

        }
        public void GuardarArchivoTexto(string nombreArchivo, List<Cadete> cadetes)
        {
            string jsonString = JsonSerializer.Serialize(cadetes);
            GuardarArchivo(nombreArchivo, jsonString);
        }
        public void GuardarArchivoTexto(string nombreArchivo, List<Pedido> pedidos)
        {
            string jsonString = JsonSerializer.Serialize(pedidos);
            GuardarArchivo(nombreArchivo, jsonString);
        }
        public void migrarDatos(string json, string csv, string tipo)
        {
            if (tipo.ToLower() == "cadetes")
            {
                var cadetes = AbrirCadetes(json);
                using (var writer = new StreamWriter(csv))
                {
                    foreach (var cadete in cadetes)
                    {
                        string fila = $"{cadete.Id},{cadete.Nombre},{cadete.Direccion},{cadete.Telefono}";
                        writer.WriteLine(fila);
                    }
                }
            }
            else if (tipo.ToLower() == "cadeterias")
            {
                var cadeterias = AbrirCadeteria(json);
                using (var writer = new StreamWriter(csv))
                {
                    foreach (var cadeteria in cadeterias)
                    {
                        // Exportamos: nombre, telefono, cantidadPedidos
                        string fila = $"{cadeteria.Nombre},{cadeteria.Telefono},{cadeteria.ListadoPedidos.Count}";
                        writer.WriteLine(fila);
                    }
                }
            }
            else
            {
                throw new ArgumentException("Tipo de entidad no válido. Use 'cadetes' o 'cadeterias'.");
            }
        }
        public void GuardarArchivoTexto(string nombreArchivo, List<Informe> informes)
        {
            string jsonString = JsonSerializer.Serialize(informes);
            GuardarArchivo(nombreArchivo, jsonString);  
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO.Compression;
using System.IO;
using Newtonsoft.Json;
using System.Runtime.Serialization.Json;
using ClimaMethod3;
using System.Globalization;

namespace Clima
{
    class Program
    {
        static void Main(string[] args)
        {
            GetRequest("https://smn.cna.gob.mx/webservices/index.php?method=3");
            Console.ReadLine();
            Console.ReadKey();
        }

        async static void GetRequest(String url)
        {
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url)) //obtener una variable con la info del url
            using (var content = await response.Content.ReadAsStreamAsync()) //obtener la info del archivo
            using (var descomprimido = new GZipStream(content, CompressionMode.Decompress)) //descomprimir el archivo
            {
                if (response.IsSuccessStatusCode)
                {
                    StreamReader reader = new StreamReader(descomprimido);
                    String data = reader.ReadLine();
                    var listInfo = JsonConvert.DeserializeObject<List<Ciudad>>(data);
                    foreach (var info in listInfo)
                    {
                        if (info.CityId.Equals("MXTS2043"))
                        {
                            Console.WriteLine("ID: " + info.CityId + "\nNombre: " + info.Name + "\nAbreviatura: " + info.StateAbbr + "\nNumero de hora: " + info.HourNumber
                                + "\nUltimo reporte: " + DateTime.ParseExact(info.LastReportTime.Substring(0, 8), "yyyyMMdd",CultureInfo.InvariantCulture).ToLongDateString() + "\nDía de la semana: " + info.DayOfWeek + "\nFechaUTC: " + DateTime.ParseExact(info.ValidDateUtc.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture).ToLongDateString()
                                + "\nFecha Local: " + DateTime.ParseExact(info.LocalValidDate.Substring(0, 8), "yyyyMMdd", CultureInfo.InvariantCulture).ToLongDateString() + "\nTemperatura: " + info.TempC + "\nDescripción del cielo: " + info.SkyDescriptionLong
                                + "\nProbabilidad de precipitación: " + info.ProbabilityOfPrecip + "\nHumedad Relativa: " + info.RelativeHumidity
                                + "\nVelocidad del viento en mph: " + info.WindSpeedMph + "\nVelocidad del viento en Km: " + info.WindSpeedKm
                                + "\nDirección del viento cardenal: " + info.WindDirectionCardinal + "\nCódigo del icono: " + info.IconCode + "\nLatitud: " + info.Latitude + "\nLongitud: " + info.Longitude +"\n");
                        }
                    }

                }


            }


            /*{
                String data = await content.ReadAsStringAsync();
                if(data != null)
                {
                    Console.WriteLine(data);
                }
            }*/
        }
    }
}

using Newtonsoft.Json;
using PruebaAtlantidaFE.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace PruebaAtlantidaFE.Controllers
{
    public class TarjetaCreditoController : Controller
    {
        private readonly HttpClient _httpClient;

        public TarjetaCreditoController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44352/"); 
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }    

        // GET: TarjetaCredito
        public async Task<ActionResult> Index()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/TarjetaCredito");

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var objetoRespuesta = JsonConvert.DeserializeObject<List<TarjetaCreditoCLS>>(jsonResponse);

                    return View(objetoRespuesta);
                }
                else
                {
                    return Content("Error: No se pudo realizar la conexión con el servidor");
                }
            }catch(Exception ex)
            {
                return Content($"No se pudo realizar la conexión con el servidor: {ex.Message}");
            }
        }

        public async Task<ActionResult> VerEstadoCuenta(string Numero)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/TarjetaCredito/{Numero}");

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var tarjetaCredito = JsonConvert.DeserializeObject<TarjetaCreditoCLS>(jsonResponse);

                    ViewBag.InteresBonificable = tarjetaCredito.SaldoActual * tarjetaCredito.PorcentajeInteresConfigurable;
                    ViewBag.CuotaMinima = tarjetaCredito.SaldoActual * tarjetaCredito.PorcentajeConfigurableSaldoMinimo;
                    ViewBag.MontoTotalConIntereses = tarjetaCredito.SaldoActual + ViewBag.InteresBonificable;

                    return View(tarjetaCredito);
                }
                else
                {
                    return Content("Error: No se pudo realizar la conexión con el servidor");
                }
            }
            catch (Exception ex)
            {
                return Content($"No se pudo realizar la conexión con el servidor: {ex.Message}");
            }
        }


    }
}
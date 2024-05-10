using PruebaAtlantidaFE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using FluentValidation.Results;

namespace PruebaAtlantidaFE.Controllers
{
    public class MovimientoController : Controller
    {
        private readonly HttpClient _httpClient;

        public MovimientoController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:44352/");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public ActionResult RegistrarCompra(int IdtarjetaCredito)
        {
            ViewBag.IdtarjetaCredito = IdtarjetaCredito;
            ViewBag.FechaHoy = DateTime.Now;
            return View();
        }

        public ActionResult RegistrarPago(int IdtarjetaCredito)
        {
            ViewBag.IdtarjetaCredito = IdtarjetaCredito;
            ViewBag.FechaHoy = DateTime.Now;
            return View();
        }

        public async Task<ActionResult> VerHistorial(int IdtarjetaCredito)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("api/Movimiento/Historial/" + IdtarjetaCredito);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    var objeto = JsonConvert.DeserializeObject<List<MovimientoCLS>>(jsonResponse);

                    return View(objeto);
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

        [HttpPost]
        public async Task<ActionResult> RegistrarCompra(MovimientoCLS Movimiento)
        {
            try
            {
                if (Movimiento.Descripción == null) Movimiento.Descripción = "";

                MovimientoValidator validator = new MovimientoValidator();
                ValidationResult result = validator.Validate(Movimiento);

                if (!result.IsValid || !ModelState.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.ErrorMessage);
                    }

                    return View(Movimiento);
                }

                var objetoJson = JsonConvert.SerializeObject(Movimiento);
                var Json = new StringContent(objetoJson, System.Text.Encoding.UTF8, "application/json");

                var respuesta = await _httpClient.PostAsync("/api/Movimiento/RegistrarCompra", Json);

                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "TarjetaCredito");
                }
                else
                {
                    ModelState.AddModelError("", "Ocurrio un error del lado del servidor");
                    return View(Movimiento);
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", $"Ocurrio un error del lado del servidor: {ex.Message}");
                return View(Movimiento);
            }
        }

        [HttpPost]
        public async Task<ActionResult> RegistrarPago(MovimientoCLS Movimiento)
        {
            try
            {
                if (Movimiento.Descripción == null) Movimiento.Descripción = "";

                MovimientoValidator validator = new MovimientoValidator();
                ValidationResult result = validator.Validate(Movimiento);

                if (!result.IsValid || !ModelState.IsValid)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.ErrorMessage);
                    }

                    return View(Movimiento);
                }                

                var objetoJson = JsonConvert.SerializeObject(Movimiento);
                var Json = new StringContent(objetoJson, System.Text.Encoding.UTF8, "application/json");

                var respuesta = await _httpClient.PostAsync("/api/Movimiento/RegistrarPago", Json);

                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "TarjetaCredito");
                }
                else
                {
                    ModelState.AddModelError("", "Ocurrio un error del lado del servidor");
                    return View(Movimiento);
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", $"Ocurrio un error del lado del servidor: {ex.Message}");
                return View(Movimiento);
            }
        }
    }
}

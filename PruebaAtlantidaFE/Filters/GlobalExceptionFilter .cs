using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PruebaAtlantidaFE.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            // Verificar si la excepción no está siendo manejada ya
            if (filterContext.ExceptionHandled)
            {
                return;
            }

            // Capturar la excepción
            Exception ex = filterContext.Exception;

            // Redirigir a una página de error 
            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary<HandleErrorInfo>(new HandleErrorInfo(ex, "Controlador", "Acción"))
            };
        }
    }
}
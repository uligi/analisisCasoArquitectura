using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using modular1.Repositories.Paypal;
using System.Configuration;


namespace modular1.Repositories
{
    public class Recursos
    {
        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6);
            return clave;
        }



        // Encriptación DE TEXTO en SHA256
        public static string ConvertirSha256(string texto)
        {
            StringBuilder Sb = new StringBuilder();
            // USAR LA REFERENCIA DE "System.Security.Cryptography"
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                {
                    Sb.Append(b.ToString("x2"));
                }
            }
            return Sb.ToString();
        }

        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool resultado = false;

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("iamxhimx@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("iamxhimx@gmail.com", "cpfy afmt emuk xcfv"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };

                smtp.Send(mail);
                resultado = true;
            }
            catch (Exception ex)
            {
                resultado = false;
            }

            return resultado;
        }

        public static bool EnviarCorreoConAdjunto(string correo, string asunto, string mensaje, string archivoAdjunto)
        {
            bool resultado = false;

            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);
                mail.From = new MailAddress("iamxhimx@gmail.com");
                mail.Subject = asunto;
                mail.Body = mensaje;
                mail.IsBodyHtml = true;

                if (!string.IsNullOrEmpty(archivoAdjunto))
                {
                    Attachment attachment = new Attachment(archivoAdjunto);
                    mail.Attachments.Add(attachment);
                }

                var smtp = new SmtpClient()
                {
                    Credentials = new NetworkCredential("iamxhimx@gmail.com", "cpfy afmt emuk xcfv"),
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true
                };

                smtp.Send(mail);
                resultado = true;
            }
            catch (Exception ex)
            {
                // Manejo de la excepción, podría registrarse el error.
                resultado = false;
            }

            return resultado;
        }

        // Método para crear una solicitud a PayPal
        public static async Task<Response_Paypal<Response_Checkout>> CrearSolicitudPaypal(Checkout_Order orden)
        {
            Response_Paypal<Response_Checkout> response_paypal = new Response_Paypal<Response_Checkout>();
            string urlPaypal = ConfigurationManager.AppSettings["UrlPaypal"];
            string clientId = ConfigurationManager.AppSettings["ClientId"];
            string secret = ConfigurationManager.AppSettings["Secret"];

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urlPaypal);

                    // Configurar autenticación básica en el encabezado
                    var authToken = Encoding.ASCII.GetBytes($"{clientId}:{secret}");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                    // Serializar el objeto de orden a JSON
                    var json = JsonConvert.SerializeObject(orden);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    // Enviar la solicitud POST al endpoint de PayPal
                    HttpResponseMessage response = await client.PostAsync("/v2/checkout/orders", data);

                    // Validar si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonRespuesta = await response.Content.ReadAsStringAsync();
                        Response_Checkout checkout = JsonConvert.DeserializeObject<Response_Checkout>(jsonRespuesta);
                        response_paypal.Response = checkout;
                        response_paypal.Status = true;
                    }
                    else
                    {
                        // Manejo de errores al recibir una respuesta no exitosa
                        string error = await response.Content.ReadAsStringAsync();
                        response_paypal.Status = false;
                        response_paypal.Message = $"Error en la solicitud: {error}";
                    }
                }
            }
            catch (HttpRequestException httpEx)
            {
                // Manejo de excepciones específicas de solicitudes HTTP
                response_paypal.Status = false;
                response_paypal.Message = $"Error en la comunicación con PayPal: {httpEx.Message}";
            }
            catch (JsonException jsonEx)
            {
                // Manejo de errores de deserialización
                response_paypal.Status = false;
                response_paypal.Message = $"Error al procesar la respuesta de PayPal: {jsonEx.Message}";
            }
            catch (Exception ex)
            {
                // Manejo general de excepciones
                response_paypal.Status = false;
                response_paypal.Message = $"Error inesperado: {ex.Message}";
            }

            return response_paypal;
        }

    }
}
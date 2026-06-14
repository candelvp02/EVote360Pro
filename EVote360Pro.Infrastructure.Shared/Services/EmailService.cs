using EVote360Pro.Infrastructure.Shared.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EVote360Pro.Infrastructure.Shared.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendCodigoVerificacionAsync(string destinatario, string nombreCiudadano, string codigo)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.From));
                email.To.Add(MailboxAddress.Parse(destinatario));
                email.Subject = "EVote360Pro - Código de Verificación";

                var builder = new BodyBuilder();
                builder.HtmlBody = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <div style='background-color: #1a237e; padding: 20px; text-align: center;'>
                            <h1 style='color: white; margin: 0;'>EVote360Pro</h1>
                        </div>
                        <div style='padding: 30px; background-color: #f5f5f5;'>
                            <h2>Hola, {nombreCiudadano}</h2>
                            <p>Tu código de verificación para ejercer tu voto es:</p>
                            <div style='background-color: #1a237e; color: white; font-size: 36px; 
                                        font-weight: bold; text-align: center; padding: 20px; 
                                        border-radius: 8px; letter-spacing: 10px;'>
                                {codigo}
                            </div>
                            <p style='margin-top: 20px;'>Este código es válido por <strong>10 minutos</strong>.</p>
                            <p style='color: #757575;'>Si no solicitaste este código, ignora este mensaje.</p>
                        </div>
                        <div style='background-color: #e0e0e0; padding: 10px; text-align: center;'>
                            <p style='color: #757575; margin: 0;'>EVote360Pro - Sistema de Votación Electrónica</p>
                        </div>
                    </div>";

                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception)
            {
                // log error
            }
        }

        public async Task SendResumenVotacionAsync(string destinatario, string nombreCiudadano, string nombreEleccion, List<(string puesto, string candidato)> votos)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.From));
                email.To.Add(MailboxAddress.Parse(destinatario));
                email.Subject = "EVote360Pro - Resumen de tu Votación";

                var filasVotos = string.Join("", votos.Select(v => $@"
                    <tr>
                        <td style='padding: 10px; border-bottom: 1px solid #e0e0e0;'>{v.puesto}</td>
                        <td style='padding: 10px; border-bottom: 1px solid #e0e0e0;'>{v.candidato}</td>
                    </tr>"));

                var builder = new BodyBuilder();
                builder.HtmlBody = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                        <div style='background-color: #1a237e; padding: 20px; text-align: center;'>
                            <h1 style='color: white; margin: 0;'>EVote360Pro</h1>
                        </div>
                        <div style='padding: 30px; background-color: #f5f5f5;'>
                            <h2>Hola, {nombreCiudadano}</h2>
                            <p>Tu voto en la elección <strong>{nombreEleccion}</strong> ha sido registrado exitosamente.</p>
                            <h3>Resumen de tu votación:</h3>
                            <table style='width: 100%; border-collapse: collapse; background-color: white;'>
                                <thead>
                                    <tr style='background-color: #1a237e; color: white;'>
                                        <th style='padding: 10px; text-align: left;'>Puesto Electivo</th>
                                        <th style='padding: 10px; text-align: left;'>Tu Selección</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {filasVotos}
                                </tbody>
                            </table>
                            <p style='margin-top: 20px; color: #757575;'>
                                Recuerda que tu voto es secreto. Este resumen solo confirma tu participación.
                            </p>
                        </div>
                        <div style='background-color: #e0e0e0; padding: 10px; text-align: center;'>
                            <p style='color: #757575; margin: 0;'>EVote360Pro - Sistema de Votación Electrónica</p>
                        </div>
                    </div>";

                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception)
            {
                // log error
            }
        }
    }
}
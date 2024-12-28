using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel.Communication;

namespace ChildGuard.Services
{
    public class SmsService
    {
        public async Task<bool> SendSmsInBulkAsync(string message, string[] recipients)
        {
            try
            {
                if (Sms.Default.IsComposeSupported)
                {
                    var smsMessage = new SmsMessage(message, recipients);
                    await Sms.ComposeAsync(smsMessage);
                    return true;
                }
                else
                {
                    throw new FeatureNotSupportedException("SMS is not supported on this device.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending SMS: {ex.Message}");
                return false;
            }
        }
    }
}

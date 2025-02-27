using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EdgeHub;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

// Visit https://aka.ms/iot-edge-csharp-function-tutorial to learn how to use this trigger
namespace FunctionApp14
{
    public static class EdgeHubTriggerFunction
    {
        [FunctionName("Function1")]
        public static async Task Run(
                    [EdgeHubTrigger("input1")] Message messageReceived,
                    [EdgeHub(OutputName = "output1")] IAsyncCollector<Message> output,
                    ILogger logger)
        {
            byte[] messageBytes = messageReceived.GetBytes();
            var messageString = System.Text.Encoding.UTF8.GetString(messageBytes);

            if (!string.IsNullOrEmpty(messageString))
            {
                logger.LogInformation("Info: Received one non-empty message");
                var pipeMessage = new Message(messageBytes);
                foreach (KeyValuePair<string, string> prop in messageReceived.Properties)
                {
                    pipeMessage.Properties.Add(prop.Key, prop.Value);
                }
                await output.AddAsync(pipeMessage);
                logger.LogInformation("Info: Piped out the message");
            }
        }
    }
}
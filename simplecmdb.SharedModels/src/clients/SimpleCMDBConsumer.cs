using Microsoft.EntityFrameworkCore.Query;
using simplecmdb.SharedModels.models;
namespace simplecmdb.SharedModels.clients
{
    public class SimpleCMDBConsumer : ISimpleCMDBConsumer
    {
        private readonly SimpleCMDBEventsApiClient webhookEventHttpClient;
        private readonly SimpleCMDBApiClient webhookHttpClient;
        public string Name { get; set; }
        public SimpleCMDB SimpleCMDB { get; set; }
        public string Script { get; set; }

        public SimpleCMDBConsumer(SimpleCMDBEventsApiClient webhookEventHttpClient, SimpleCMDBApiClient webhookHttpClient, string name, SimpleCMDB webhook, string script)
        {
            this.webhookEventHttpClient = webhookEventHttpClient;
            this.webhookHttpClient = webhookHttpClient;
            Name = name;
            SimpleCMDB = webhook;
            Script = script;

        }
        public static async Task<SimpleCMDBConsumer> CreateAsync(SimpleCMDBEventsApiClient webhookEventHttpClient, SimpleCMDBApiClient webhookHttpClient, string name, string webhook, string script)
        {
            var _webhook = await webhookHttpClient.GetSimpleCMDBAsync(webhook);
            return new SimpleCMDBConsumer(webhookEventHttpClient, webhookHttpClient, name, _webhook, script);
        }

        private async Task<SimpleCMDBEvent?> GetNextSimpleCMDBEventAsync()
        {
            // call the webhook API to get the next webhook event at api/webhookevents/receive/{this.webhook}
            // return the webhook event
            var result = await webhookEventHttpClient.GetSimpleCMDBEventAsync(SimpleCMDB.Id);
            return result;

        }

        public async Task<SimpleCMDBEventWorkResponse> ProcessSimpleCMDBEventAsync(SimpleCMDBEvent webhookEvent)
        {
            Console.WriteLine($"Processing webhook event {webhookEvent.Id} for webhook {webhookEvent.SimpleCMDBId}");

            var process = new System.Diagnostics.Process();
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true; // Enable stdin redirection
            process.StartInfo.RedirectStandardOutput = true;

            if (Script.EndsWith(".sh"))
            {
                process.StartInfo.FileName = Script;
                process.StartInfo.Arguments = $"{webhookEvent.Id}";
            }
            else if (Script.EndsWith(".py"))
            {
                process.StartInfo.FileName = "python";
                process.StartInfo.Arguments = $"{Script} {webhookEvent.Id}";
            }
            else
            {
                throw new Exception("Unsupported script type");
            }

            // Start the process
            process.Start();

            // Write the JSON payload to stdin
            if (!string.IsNullOrEmpty(webhookEvent.Payload))
            {
                await process.StandardInput.WriteAsync(webhookEvent.Payload);
            }
            process.StandardInput.Close(); // Close stdin to signal end of input

            // Read the output from the script
            var output = await process.StandardOutput.ReadToEndAsync();
            Console.WriteLine(output);

            // Wait for the process to exit
            await process.WaitForExitAsync();

            var result = new SimpleCMDBEventWorkResponse
            {
                Status = SimpleCMDBEventSubStatus.Success,
                ResultText = "SimpleCMDB event processed successfully"
            };

            await webhookEventHttpClient.updateSimpleCMDBEventAsync(webhookEvent.Id, result);
            return result;
        }

        public async Task<SimpleCMDBEventWorkResponseCollection> StartProcessingLoopAsync()
        {
            var results = new SimpleCMDBEventWorkResponseCollection();
            // Start the processing loop.


            while (true)
            {
                Console.WriteLine("Checking for next webhook event");
                var nextSimpleCMDB = await GetNextSimpleCMDBEventAsync();
                if (nextSimpleCMDB != null)
                {
                    // Process the webhook event.
                    var result = await ProcessSimpleCMDBEventAsync(nextSimpleCMDB);


                    // Update the results.
                    if (result.Status == SimpleCMDBEventSubStatus.Success)
                    {
                        results.ProcessedCount++;
                    }
                    else
                    {
                        results.FailedCount++;
                    }
                }
                // delay for 100ms
                await Task.Delay(100);

                if (results.TotalCount > 100)
                {
                    break;
                }
            }
            return results;


        }
    }
}

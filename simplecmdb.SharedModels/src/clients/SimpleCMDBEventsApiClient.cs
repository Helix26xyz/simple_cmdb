using simplecmdb.SharedModels.models;
using System.Net.Http.Json;
namespace simplecmdb.SharedModels.clients;

public class SimpleCMDBEventsApiClient(HttpClient httpClient)
{
    public async Task<SimpleCMDBEvent?> GetSimpleCMDBEventAsync(Guid webhookId, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.GetAsync($"/api/webhookevents/receive/{webhookId.ToString()}", cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        {
            // Return null if the server responds with 204 No Content
            return null;
        }

        // Ensure the response is successful, or throw an exception
        response.EnsureSuccessStatusCode();

        // Deserialize the response content into a SimpleCMDBEvent
        var webhookEvent = await response.Content.ReadFromJsonAsync<SimpleCMDBEvent>(cancellationToken: cancellationToken);

        return webhookEvent;
    }

    public async Task<bool> updateSimpleCMDBEventAsync(Guid webhookId, SimpleCMDBEventWorkResponse response, CancellationToken cancellationToken = default)
    {
        var res = await httpClient.PutAsJsonAsync<SimpleCMDBEventWorkResponse>($"/api/webhookevents/return/{webhookId.ToString()}", response, cancellationToken);



        return res != null;
    }
}
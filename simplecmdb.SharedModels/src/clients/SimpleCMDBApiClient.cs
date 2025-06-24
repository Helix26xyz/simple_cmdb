using simplecmdb.SharedModels.models;
using System.Net.Http.Json;
namespace simplecmdb.SharedModels.clients;

public class SimpleCMDBApiClient(HttpClient httpClient)
{
    public async Task<SimpleCMDB[]> GetSimpleCMDBsAsync(int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<SimpleCMDB>? simplecmdb = null;

        await foreach (var webhook in httpClient.GetFromJsonAsAsyncEnumerable<SimpleCMDB>("/api/simplecmdb", cancellationToken))
        {
            if (simplecmdb?.Count >= maxItems)
            {
                break;
            }
            if (webhook is not null)
            {
                simplecmdb ??= [];
                simplecmdb.Add((SimpleCMDB)webhook);
            }
        }

        return simplecmdb?.ToArray() ?? [];
    }
    public async Task<SimpleCMDB?> GetSimpleCMDBAsync(string webhookId, CancellationToken cancellationToken = default)
    {
        SimpleCMDB? webhook = await httpClient.GetFromJsonAsync<SimpleCMDB>($"/api/simplecmdb/{webhookId}", cancellationToken);

        return webhook;
    }
    public async Task<SimpleCMDB> AddSimpleCMDBAsync(SimpleCMDB newSimpleCMDB, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("/api/simplecmdb", newSimpleCMDB, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<SimpleCMDB>(cancellationToken: cancellationToken) ?? new SimpleCMDB();
    }
    public async Task<bool> DeleteSimpleCMDBAsync(Guid webhookId, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.DeleteAsync($"/api/simplecmdb/{webhookId}", cancellationToken);
        return response.IsSuccessStatusCode;
    }

    public async Task<SimpleCMDBEvent[]> GetSimpleCMDBEventsForSimpleCMDBAsync(string webhookId,  int maxItems = 10, CancellationToken cancellationToken = default)
    {
        List<SimpleCMDBEvent>? webhookevents = null;

        await foreach (var webhookevent in httpClient.GetFromJsonAsAsyncEnumerable<SimpleCMDBEvent>($"/api/webhookevents/bywebhook/{webhookId}", cancellationToken))
        {
            if (webhookevents?.Count >= maxItems)
            {
                break;
            }
            if (webhookevent is not null)
            {
                webhookevents ??= [];
                webhookevents.Add((SimpleCMDBEvent) webhookevent);
            }
        }

        return webhookevents?.ToArray() ?? [];
    }
    public async Task<SimpleCMDB> CreateSimpleCMDBAsync(SimpleCMDB newSimpleCMDB, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("/api/simplecmdb", newSimpleCMDB, cancellationToken);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<SimpleCMDB>(cancellationToken: cancellationToken) ?? new SimpleCMDB();
    }
    public async Task<bool> UpdateSimpleCMDBAsync(SimpleCMDB webhook, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PutAsJsonAsync($"/api/simplecmdb/{webhook.Id}", webhook, cancellationToken);
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }
}
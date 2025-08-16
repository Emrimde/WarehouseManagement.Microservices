using OrderMicroservice.Core.DTO.InventoryDTO;
using System.Net.Http.Json;

namespace OrderMicroservice.Core.HttpClients;
public class InventoryMicroserviceClient
{
    private readonly HttpClient _httpClient;
    public InventoryMicroserviceClient(HttpClient httpclient)
    {
        _httpClient = httpclient;
    }
    public async Task<InventoryItemResponse?> GetInventoryBySkuAsync(string sku)
    {
        HttpResponseMessage response = await _httpClient.GetAsync($"/api/Inventories/{sku}");

        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                throw new HttpRequestException($"Http request failed with status code: {response.StatusCode} ");
            }
        }

        InventoryItemResponse? inventoryItem = await response.Content.ReadFromJsonAsync<InventoryItemResponse>();

        return inventoryItem!;


    }
}

using OrderMicroservice.Core.DTO.ProductDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OrderMicroservice.Core.HttpClients
{
    public class ProductMicroserviceClient
    {
        private readonly HttpClient _client;
        public ProductMicroserviceClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<ProductResponse?> GetProductBySkuAsync(string sku)
        {
            HttpResponseMessage response = await _client.GetAsync($"/api/products/sku/{sku}");
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw new HttpRequestException($"Http request failed with status code {response.StatusCode}");
                }
            }

            ProductResponse? product = await response.Content.ReadFromJsonAsync<ProductResponse>();
            return product;
        }
    }
}

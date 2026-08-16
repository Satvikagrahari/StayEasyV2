using StayEasy.Application.Interfaces.External;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace StayEasy.Infrastructure.Services
{
    public class PaymentServiceClient :IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        // Injecting HttpClient via IHttpClientFactory pattern
        public PaymentServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto request)
        {
            // 1. Converting C# Object to JSON
            var jsonContent = new StringContent(
                    JsonSerializer.Serialize(request),
                    Encoding.UTF8,
                    "application/json"
                );
            try
            {
                // 2. Sending the POST request over the network to the microservice!
                var response = await _httpClient.PostAsync("/api/payment/process", jsonContent);

                // 3. Reading the JSON response from the microservice
                var responseString = await response.Content.ReadAsStringAsync();
                var paymentResult = JsonSerializer.Deserialize<PaymentResponseDto>(responseString, _jsonOptions);
                if(paymentResult == null)
                {
                    return new PaymentResponseDto(false, null, "failed to deserialize response from payment service.");
                }
                return paymentResult;
            }
            catch(HttpRequestException ex)
            {
                // If the microservice is offline or crashed, we catch the network error here
                return new PaymentResponseDto(false, null, $"Network error: Could not reach Payment Service. {ex.Message}");
            }
        }


    }
}

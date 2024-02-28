using System.Text.Json;
using System.Text.Json.Serialization;

namespace Training.Product.Middleware
{
    public class ErrorResponse
    {
        public int statusCode { get; set; }
        public string? message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}

using System.Text.Json.Serialization;

namespace FinanceNow.Blazor.Client.Response
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("$values")]
        public ICollection<T>? Values { get; set; }
    }
}

using System.Text.Json.Serialization;

namespace FinanceNow.FrontEnd.Client.Response
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("$values")]
        public ICollection<T>? Values { get; set; }
    }
}

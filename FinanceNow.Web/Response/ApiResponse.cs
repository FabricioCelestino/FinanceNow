using System.Text.Json.Serialization;

namespace FinanceNow.Web.Response
{
    public class ApiResponse<T>
    {
        [JsonPropertyName("$values")]
        public ICollection<T>? Values { get; set; }
    }
}

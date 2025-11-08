using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EasyFinder.Model;

[Table("MOTO")]
public class Moto: IBindableFromHttpContext<Moto>
{
    public static async ValueTask<Moto?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if (!string.IsNullOrEmpty(context.Request.ContentType) && context.Request.ContentType.Contains("xml"))

        {
            var xmlDoc = await XDocument.LoadAsync(context.Request.Body, LoadOptions.None, context.RequestAborted);
            var serializer = new XmlSerializer(typeof(Moto));
            return (Moto?)serializer.Deserialize(xmlDoc.CreateReader());
        }

        return await context.Request.ReadFromJsonAsync<Moto>();
    }
    
    [Column("ID")]
    [Key]
    [Description("Identificador único da Moto")]
    [JsonIgnore]
    public int Id { get; set; }
    
    [Column("STATUS")]
    [Description("Status da situação da Moto")]
    public string Status { get; set; } = string.Empty;
    
    [Column("MODELO")]
    [Description("Modelo da Vaga")]
    public string Modelo { get; set; } = string.Empty;
    
    [Column("MARCA")]
    [Description("Marca da Vaga")]
    public string Marca { get; set; } = string.Empty;
    
    [Column("PLACA")]
    [Description("Placa da Vaga")]
    public string Placa { get; set; } = string.Empty;
    
    [Column("CHASSI")]
    [Description("Chassi da Vaga")]
    public string Chassi { get; set; } = string.Empty;
    
    [Column("VAGA_ID")]
    [Description("Identificador único da Vaga que se encontra a Moto")]
    public int Vaga_id { get; set; }
}
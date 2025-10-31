using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EasyFinder.Model;

[Table("GALPAO")]
public class Galpao : IBindableFromHttpContext<Galpao>
{
    public static async ValueTask<Galpao?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if (!string.IsNullOrEmpty(context.Request.ContentType) && context.Request.ContentType.Contains("xml"))

        {
            var xmlDoc = await XDocument.LoadAsync(context.Request.Body, LoadOptions.None, context.RequestAborted);
            var serializer = new XmlSerializer(typeof(Galpao));
            return (Galpao?)serializer.Deserialize(xmlDoc.CreateReader());
        }

        return await context.Request.ReadFromJsonAsync<Galpao>();
    }
    
    [Column("ID")]
    [Key]
    [Description("Identificador único do Galpão")]
    public int Id { get; set; }
    
    [Column("NOME_GALPAO")]
    [Description("Nome do galpão")]
    public string Nome_galpao { get; set; } = string.Empty;

    
}
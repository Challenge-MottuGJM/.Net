using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EasyFinder.Model;

[Table("BLOCO")]
public class Bloco : IBindableFromHttpContext<Bloco>
{
    public static async ValueTask<Bloco?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if (context.Request.ContentType.Contains("xml"))
        {
            var xmlDoc = await XDocument.LoadAsync(context.Request.Body, LoadOptions.None, context.RequestAborted);
            var serializer = new XmlSerializer(typeof(Galpao));
            return (Bloco?)serializer.Deserialize(xmlDoc.CreateReader());
        }

        return await context.Request.ReadFromJsonAsync<Bloco>();
    }
    
    [Column("ID")]
    [Key]
    [Description("Identificador único do Bloco")]
    public int Id { get; set; }
    
    [Column("LETRA_BLOCO")]
    [Description("Letra do bloco")]
    public string Letra_bloco { get; set; }

    [Column("PATIO_ID")]
    [Description("Identificador único do Patio que se encontra o Bloco")]
    public int Patio_id { get; set;  }
}
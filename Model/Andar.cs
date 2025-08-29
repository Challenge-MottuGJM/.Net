using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EasyFinder.Model;

[Table("ANDAR")]
public class Andar : IBindableFromHttpContext<Andar>
{
    
    public static async ValueTask<Andar?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if (context.Request.ContentType != null && context.Request.ContentType.Contains("xml"))
        {
            var xmlDoc = await XDocument.LoadAsync(context.Request.Body, LoadOptions.None, context.RequestAborted);
            var serializer = new XmlSerializer(typeof(Andar));
            return (Andar?)serializer.Deserialize(xmlDoc.CreateReader());
        }

        return await context.Request.ReadFromJsonAsync<Andar>();
    }
    
    [Column("ID")]
    [Key]
    [Description("Identificador único do Andar")]
    public int Id { get; set; }
    
    [Column("NUMERO_ANDAR")]
    [Description("Número do andar")]
    public int Numero_andar { get; set; }
    
    [Column("GALPAO_ID")]
    [Description("Identificador único do Galpão que se encontra o Andar")]
    public int bloco_id { get; set; }
}
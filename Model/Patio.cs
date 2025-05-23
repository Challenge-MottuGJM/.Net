using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EasyFinder.Model;

[Table("PATIO")]
public class Patio : IBindableFromHttpContext<Patio>
{
    
    public static async ValueTask<Patio?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if (context.Request.ContentType.Contains("xml"))
        {
            var xmlDoc = await XDocument.LoadAsync(context.Request.Body, LoadOptions.None, context.RequestAborted);
            var serializer = new XmlSerializer(typeof(Patio));
            return (Patio?)serializer.Deserialize(xmlDoc.CreateReader());
        }

        return await context.Request.ReadFromJsonAsync<Patio>();
    }
    
    [Column("ID")]
    [Key]
    [Description("Identificador único do Patio")]
    public int Id { get; set; }
    
    [Column("NUMERO_PATIO")]
    [Description("Número do Patio")]
    public int Numero_patio { get; set; }
    
    [Column("ANDAR_ID")]
    [Description("Identificador único do Andar que se encontra o Patio")]
    public int Andar_id { get; set; }
}
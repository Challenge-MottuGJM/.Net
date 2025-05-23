using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EasyFinder.Model;

[Table("VAGA")]
public class Vaga : IBindableFromHttpContext<Vaga>
{
    public static async ValueTask<Vaga?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if (context.Request.ContentType.Contains("xml"))
        {
            var xmlDoc = await XDocument.LoadAsync(context.Request.Body, LoadOptions.None, context.RequestAborted);
            var serializer = new XmlSerializer(typeof(Vaga));
            return (Vaga?)serializer.Deserialize(xmlDoc.CreateReader());
        }

        return await context.Request.ReadFromJsonAsync<Vaga>();
    }
    
    [Column("ID")]
    [Key]
    [Description("Identificador único da Vaga")]
    public int Id { get; set; }
    
    [Column("NUMERO_VAGA")]
    [Description("Número da Vaga")]
    public int Numero_vaga { get; set; }
    
    [Column("BLOCO_ID")]
    [Description("Identificador único do Bloco que se encontra a Vaga")]
    public int Bloco_id { get; set; }
}
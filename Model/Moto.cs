using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EasyFinder.Model;

[Table("MOTO")]
public class Moto: IBindableFromHttpContext<Moto>
{
    public static async ValueTask<Moto?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if (context.Request.ContentType.Contains("xml"))
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
    public int Id { get; set; }
    
    [Column("STATUS")]
    [Description("Status da situação da Moto")]
    public string Status { get; set; }
    
    [Column("MODELO")]
    [Description("Modelo da Vaga")]
    public string Modelo { get; set; }
    
    [Column("MARCA")]
    [Description("Marca da Vaga")]
    public string Marca { get; set; }
    
    [Column("PLACA")]
    [Description("Placa da Vaga")]
    public string Placa { get; set; }
    
    [Column("CHASSI")]
    [Description("Chassi da Vaga")]
    public string Chassi { get; set; }
    
    [Column("VAGA_ID")]
    [Description("Identificador único da Vaga que se encontra a Moto")]
    public int Vaga_id { get; set; }
}
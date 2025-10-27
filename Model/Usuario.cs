using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EasyFinder.Model;

[Table("USUARIO")]
public class Usuario : IBindableFromHttpContext<Usuario>
{
    
    public static async ValueTask<Usuario?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        if (context.Request.ContentType != null && context.Request.ContentType.Contains("xml"))
        {
            var xmlDoc = await XDocument.LoadAsync(context.Request.Body, LoadOptions.None, context.RequestAborted);
            var serializer = new XmlSerializer(typeof(Andar));
            return (Usuario?)serializer.Deserialize(xmlDoc.CreateReader());
        }

        return await context.Request.ReadFromJsonAsync<Usuario>();
    }
    
    [Column("ID")]
    [Description("Identificador único do Usuário")]
    public int Id { get; set; }

    [Column("USUARIO")]
    [Description("Usuario do Usuário")]
    public String Username { get; set; } = default!;
    
    [Column("SENHA")]
    [Description("Senha do Usuário")] 
    public String Senha { get; set; } = default!;
}
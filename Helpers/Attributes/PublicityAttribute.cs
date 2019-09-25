using System.Xml.Serialization;
using Resistance.Enums;

namespace Resistance.Helpers.Attributes
{
    public class PublicityAttribute : XmlEnumAttribute
    {
        public PublicityAttribute(CommandType type) => Type = type;

        public CommandType Type { get; }
    }
}
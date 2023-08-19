
using System.Xml.Serialization;

namespace UvProject;
public class Targets
{
    [XmlElement("Target")]
    public List<Target> TargetList { get; set; } = new List<Target>();
}

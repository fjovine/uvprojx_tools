using System.Xml.Serialization;

namespace UvProject;
[XmlRoot("Project")]
public class Project
{
    [XmlElement("Targets")]
    public Targets Targets { get; set; } = new Targets();
}

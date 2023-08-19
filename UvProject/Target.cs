using System.Xml.Serialization;

namespace UvProject;
public class Target
{
    [XmlElement("TargetName")]
    public string TargetName { get; set; } = string.Empty;

    [XmlElement("Groups")]
    public Groups Groups { get; set; } = new Groups();
}
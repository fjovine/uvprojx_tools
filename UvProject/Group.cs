using System.Xml.Serialization;

namespace UvProject;
public class Group
{
    [XmlElement("GroupName")]
    public string? GroupName { get; set; }

    [XmlElement("Files")]
    public Files Files { get; set; } = new Files();
}
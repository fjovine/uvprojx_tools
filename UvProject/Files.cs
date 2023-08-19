using System.Xml.Serialization;

namespace UvProject;
public class Files
{
    [XmlElement("File")]
    public List<SourceFile> FileList { get; set; } = new List<SourceFile>();
}
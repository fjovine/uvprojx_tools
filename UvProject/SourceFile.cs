using System.Xml.Serialization;

namespace UvProject;
public class SourceFile
{
    [XmlElement("FileName")]
    public string? FileName { get; set; }

    [XmlElement("FileType")]
    public int FileType { get; set; }

    [XmlElement("FilePath")]
    public string? FilePath { get; set; }
}
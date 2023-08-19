using System.Xml.Serialization;

namespace UvProject;
    public class Groups
    {
        [XmlElement("Group")]
        public List<Group> GroupList { get; set; } = new List<Group>();
    }
using System.Xml.Serialization;

namespace APBD2.Models
{
    public class Active
    {
        [XmlAttribute(AttributeName = "studies")]
        public string name { get; set; }
        [XmlAttribute]
        public int numberOfStudents { get; set; }


        public override bool Equals(object obj)
        {
            Active studies = obj as Active;

            if (studies == null)
            {
                return false;
            }

            return studies.name == this.name;
        }

        public override int GetHashCode()
        {

            return name.GetHashCode();
        }

    }

}

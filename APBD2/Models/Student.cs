using System;
using System.Xml.Serialization;

namespace APBD2.Models {
    public class Student {

        [XmlAttribute]
        [Newtonsoft.Json.JsonProperty]

        public String indexNumber { get; set; }
        [Newtonsoft.Json.JsonProperty("fname")]

        public String firstName { get; set; }
        [Newtonsoft.Json.JsonProperty("lname")]

        public String lastName { get; set; }
        [Newtonsoft.Json.JsonProperty("birthdate")]

        public String birthDate { get; set; }
        public String email { get; set; }
        public String mothersName { get; set; }
        public String fathersName { get; set; }
        public Studies studies { get; set; }

        public override int GetHashCode() {

            var hash = indexNumber + firstName + lastName;
            return hash.GetHashCode();
        }

        public override bool Equals(object obj) {

            Student student = obj as Student;

            if (student == null) return false;

            return  student.indexNumber == this.indexNumber && student.firstName.Equals(this.firstName) && student.lastName.Equals(this.lastName);
        }
    }
}

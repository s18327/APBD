using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace APBD2.Models {
    public class University {

        public University() {
            Students = new HashSet<Student>();
            CreationDate = DateTime.Now.ToString("yyyy-MM-dd");
            ActiveCourses = new HashSet<Active>();
        }

        
        [XmlAttribute(AttributeName = "CreatedAt")]
        [JsonProperty("CreatedAt")]
        public string CreationDate { get; set; }

        [XmlAttribute]
        [JsonProperty]
        public string Author { get; set; }


        public HashSet<Student> Students { get; set; }
        public HashSet<Active> ActiveCourses { get; set; }
        public Active GetReference(Active obj) {

            if (ActiveCourses.Contains(obj)) {

                foreach (Active o in ActiveCourses) {

                    if (obj.Equals(o)) return o;
                }
            }
            return null;
        }
        public Student getReference(Student obj) {

            if (Students.Contains(obj)) {

                foreach (Student s in Students) {

                    if (obj.Equals(s)) return s;
                }
            }
            return null;
        }
    }
}

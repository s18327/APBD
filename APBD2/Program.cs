using APBD2.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml.Serialization;

namespace APBD2 {
    class Program {

        static void Main(string[] args) {

            var inputPath = args.Length > 0 ? args[1] : "Files\\data.csv";
            var outputPath = args.Length > 1 ? args[2] : "Files\\result";
            var outputType = args.Length > 2 ? args[3] : "xml";
          
            try {

                Path.GetFullPath(inputPath);
                Path.GetFullPath(outputPath);
            }
            catch (ArgumentException e) {

                File.AppendAllText(@"Files\Log.txt", $"{DateTime.UtcNow} ERR specified path is not correct: {e.Message}\n");
            }

            try {

                //Checking if file exists
                if (!File.Exists(inputPath)) throw new FileNotFoundException("ERR", inputPath.Split("\\")[^1]);

                var university = new University {

                    Author = "Jakub Goralczyk"
                };

                //Reading from file
                foreach (var line in File.ReadAllLines(inputPath)) {

                    //cheking if there is 9 arguments in line
                    var splitted = line.Split(",");

                    if (splitted.Length < 8) {

                        File.AppendAllText(@"Files\Log.txt", $"{DateTime.UtcNow} ERR not enough information in line {line}\n");
                        continue;
                    }
                    //checking if all 9 parts are present in the sequence (line)
                    int countMissing = 0;

                    for (int i = 0; i < splitted.Length; i++) {

                        if (splitted[i].Equals("")) {

                            File.AppendAllText(@"Files\Log.txt", $"{DateTime.UtcNow} ERR no information in line {line}\n");
                            countMissing++;
                        }
                    }

                    if (countMissing > 0) continue;
 
                    //creating new student and adding his attributes to the object
                    var stud = new Student {

                        indexNumber = splitted[4],
                        firstName = splitted[0],
                        lastName = splitted[1],
                        birthDate = splitted[5],
                        mothersName = splitted[7],
                        fathersName = splitted[8],
                        studies = new Studies {
                            name = splitted[2],
                            mode = splitted[3],
                        },

                        email = splitted[6],
                    };
                    //adding student to the list of all students
                    university.Students.Add(stud);
                }

                // adding studies to active studies 
                foreach (Student student in university.Students) {

                    var subject = student.studies;
                    var active = new Active {

                        name = subject.name,
                        numberOfStudents = 1,
                    };

                    if (university.GetReference(active) != null) university.GetReference(active).numberOfStudents++;

                    else university.ActiveCourses.Add(active);
                }

                
                if (outputType.Equals("json")) {

                    //Serializing to json
                    var forJson = new ToJSON {

                        university = university,
                    };

                    var serializer      = new JsonSerializer();
                    var stringWriter    = new StringWriter();

                    //serialising 
                    using (var writer   = new JsonTextWriter(stringWriter)) {

                        writer.QuoteName = false;
                        writer.Formatting = Formatting.Indented;
                        
                        serializer.Serialize(writer, forJson);
                    }

                    var jsonString = stringWriter.ToString();
                    File.WriteAllText($"{outputPath}.json", jsonString);
                }

                //serialising to xml
                else if (outputType.Equals("xml")) {

                    using var writer = new FileStream($"{outputPath}.{outputType}", FileMode.Create);
                    XmlSerializerNamespaces SerializerNamespace = new XmlSerializerNamespaces();

                    SerializerNamespace.Add("", "");
                    var serializer = new XmlSerializer(typeof(University));

                    serializer.Serialize(writer, university, SerializerNamespace);
                }
                //other fileTypes are not supported 
                else {

                    File.AppendAllText(@"Files\Log.txt", $"{DateTime.UtcNow} Provided file type is not supported : {outputType}\n");
                }

            } catch (FileNotFoundException e) {
                File.AppendAllText(@"Files\Log.txt", $"{DateTime.UtcNow} {e.Message} File not found ({e.FileName})\n");
            }
        }
    }
}
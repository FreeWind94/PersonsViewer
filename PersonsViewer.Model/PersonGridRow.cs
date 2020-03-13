using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonsViewer.Model
{
    public class PersonGridRow
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public string Departament { get; set; }
        public string Post { get; set; }
        public string DateEmploy { get; set; }
        public string DateUnemploy { get; set; }
        
        public PersonGridRow(Person person)
        {
            Name = person.LastName + " " + person.FirstName.Substring(0, 1) + "." + person.SecondName.Substring(0, 1) + ".";
            Status = person.Status.Name;
            Departament = person.Departament.Name;
            Post = person.Post.Name;
            if(person.DateEmploy == DateTime.MinValue)
            {
                DateEmploy = "";
            }
            else
            {
                DateEmploy = person.DateEmploy.Year + "-" + person.DateEmploy.Month.ToString("00") + "-" + person.DateEmploy.Day.ToString("00");
            }

            if (person.DateUnemploy == DateTime.MinValue)
            {
                DateUnemploy = "";
            }
            else
            {
                DateUnemploy = person.DateUnemploy.Year + "-" + person.DateUnemploy.Month.ToString("00") + "-" + person.DateUnemploy.Day.ToString("00");
            }
        }
    }
}

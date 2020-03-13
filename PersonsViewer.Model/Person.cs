using System;

namespace PersonsViewer.Model
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public DateTime DateEmploy { get; set; }
        public DateTime DateUnemploy { get; set; }
        public Status Status { get; set; }
        public Departament Departament { get; set; }
        public Post Post { get; set; }
    }
}
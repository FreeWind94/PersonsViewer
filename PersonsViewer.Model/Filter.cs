using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonsViewer.Model
{
    public class Filter
    {
        public Status Status { get; set; }
        public Departament Departament { get; set; }
        public Post Post { get; set; }
        public string LastName { get; set; }
    }
}

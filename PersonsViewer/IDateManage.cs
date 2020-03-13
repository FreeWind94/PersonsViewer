using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonsViewer
{
    interface IDateManage
    {
        IEnumerable<Person> GetPeople();
        IEnumerable<Person> GetPeople(Status status, DateTime startDate, DateTime endDate);
    }
}

using PersonsViewer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonsViewer.DataLayer
{
    public interface IDataManage
    {
        IEnumerable<Person> GetPeople(Filter filterOptions);

        IDictionary<DateTime, int> GetStatistic(Status status, bool isDateEmploy, DateTime startDate, DateTime endDate);
    }
}

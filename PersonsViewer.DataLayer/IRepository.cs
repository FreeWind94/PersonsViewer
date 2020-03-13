using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonsViewer.DataLayer
{
    public interface IRepository<T>
    {
        T GetEntityByID(int id);

        IEnumerable<T> GetAllEntitys();
    }
}

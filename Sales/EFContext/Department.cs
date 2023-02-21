using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.EFContext
{
    public class Department
    {
        // класс-сущность, отображающую таблицу Departments
        public Guid Id { get; set; }     //набор сущностей посторяет структуру таблицы
        public String Name { get; set; } = null!;

        public List<Manager> Managers { get; set; }

        public List<Manager> ManagersS { get; set; }
        public String ToShortString()
        {
            return Id.ToString()[..4] + "... " + Name;
        }
    }
}

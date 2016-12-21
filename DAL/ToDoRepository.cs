using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public  class ToDoRepository
    {
        private static readonly string storagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.TrimEnd(@"Debug\\".ToCharArray()), "storage.txt");
        public void AddToFile(IEnumerable<ToDo> todo)
        {
            if (!File.Exists(storagePath))
            {
                File.Create(storagePath).Close();
            }
            string json = JsonConvert.SerializeObject(todo);
            File.WriteAllText(storagePath, json);
        }
    }
}

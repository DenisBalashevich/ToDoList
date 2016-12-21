using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using todoclient.Models;
using ToDoClient.Models;

namespace todoclient.Services
{
    public class JSONWorker
    {
        private static readonly string storagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory.TrimEnd(@"Debug\\".ToCharArray()), "storage.txt");

        public static void Add(IEnumerable<ToDoToServer> toDo)
        {
            if (!File.Exists(storagePath))
            {
                File.Create(storagePath).Close();
            }

            string json = JsonConvert.SerializeObject(toDo);
            File.WriteAllText(storagePath, json);
        }

        public static IList<ToDoToServer> Get()
        {
            if (!File.Exists(storagePath))
                return null;
            return JsonConvert.DeserializeObject<IList<ToDoToServer>>(File.ReadAllText(storagePath));
        }
    }
}
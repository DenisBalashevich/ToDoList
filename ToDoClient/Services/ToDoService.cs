using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using ToDoClient.Models;
using todoclient;
using System.Linq;
using todoclient.Services;
using System.Threading.Tasks;
using todoclient.Models;

namespace ToDoClient.Services
{
    /// <summary>
    /// Works with ToDo backend.
    /// </summary>
    public class ToDoService
    {
        private static RemoteToDoService remoteService;
        private static LocalToDoService localService;
        static ToDoService()
        {
            remoteService = new RemoteToDoService();
            localService = new LocalToDoService();
            Task.Run(() => Update());
        }

        /// <summary>
        /// Creates the service.
        /// </summary>
        public ToDoService()
        {
            remoteService = new RemoteToDoService();
            localService = new LocalToDoService();
        }

        /// <summary>
        /// Gets all todos for the user.
        /// </summary>
        /// <param name="userId">The User Id.</param>
        /// <returns>The list of todos.</returns>
        public IList<ToDoItemViewModel> GetItems(int userId)
        {
            if (localService.ToDoItems.Count != 0)
            {
                List<ToDoItemViewModel> t = new List<ToDoItemViewModel>();
                for (int i = 0; i < localService.AllToDoItems.Count; i++)
                {
                    if (localService.AllToDoItems[i].MessageType == MessageType.Add
                        || localService.AllToDoItems[i].MessageType == MessageType.None)
                        t.Add(localService.AllToDoItems[i].item);
                }
                return t;
            }

            var temp = remoteService.GetItems(userId);

            for (int i = 0; i < temp.Count; i++)
            {
                localService.AddItemExist(temp[i]);
            }
            return localService.AllToDoItems.Select(a => a.item).ToList();
        }

        /// <summary>
        /// Creates a todo.UserId is taken from the model.
        /// </summary>
        /// <param name = "item" > The todo to create.</param>
        public void CreateItem(ToDoItemViewModel item)
        {
            localService.AddItem(item, MessageType.Add);
        }

        /// <summary>
        /// Updates a todo.
        /// </summary>
        /// <param name="item">The todo to update.</param>
        public void UpdateItem(ToDoItemViewModel item)
        {
            localService.UpdateItem(item);
        }

        /// <summary>
        /// Deletes a todo.
        /// </summary>
        /// <param name="id">The todo Id to delete.</param>
        public void DeleteItem(int id)
        {
            localService.RemoveItem(id);
        }
        static object locker = new object();

        public static void Update()
        {
            while (true)
            {
                Task.Run(() =>
                {
                    if (localService.ToDoItems.Count > 0)
                        lock (locker)
                        {
                            for (int i = 0; i < localService.ToDoItems.Count; i++)
                            {
                                if (localService.ToDoItems[i].MessageType == MessageType.Add)
                                {
                                    WorkWithJson(localService.ToDoItems[i]);
                                }
                                else if (localService.ToDoItems[i].MessageType == MessageType.Delete)
                                {
                                    remoteService.DeleteItem(localService.ToDoItems[i].item.ToDoId);
                                    localService.AllToDoItems.Remove(localService.ToDoItems[i]);
                                    JSONWorker.Add(localService.AllToDoItems);
                                }
                                else if (localService.ToDoItems[i].MessageType == MessageType.UpDate)
                                {
                                    WorkWithJson(localService.ToDoItems[i]);
                                }

                            }
                            localService.ToDoItems.Clear();
                        }
                });
            }
        }

        private static void WorkWithJson(ToDoToServer model)
        {
            remoteService.CreateItem(model.item);
            localService.AllToDoItems.Remove(model);
            localService.AddItemExist(model.item);
            JSONWorker.Add(localService.AllToDoItems);
        }
    }

}


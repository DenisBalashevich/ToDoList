using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using todoclient.Models;
using ToDoClient.Models;

namespace todoclient.Services
{
    public class LocalToDoService
    {
        public IList<ToDoToServer> ToDoItems { get; set; }
        public IList<ToDoToServer> AllToDoItems { get; set; }
        public LocalToDoService()
        {
            ToDoItems = new List<ToDoToServer>();
            AllToDoItems = JSONWorker.Get() ?? new List<ToDoToServer>();
            ToDoItems = AllToDoItems.Where(a => a.MessageType != MessageType.None).ToList() ?? new List<ToDoToServer>();

        }

        public void RemoveItem(int toDoId)
        {
            var temp = AllToDoItems.FirstOrDefault(a => a.item.ToDoId == toDoId);
            if (!ReferenceEquals(temp, null))
            {
                AllToDoItems.Remove(temp);
                AllToDoItems.Add(new ToDoToServer { item = temp.item, MessageType = MessageType.Delete });
                JSONWorker.Add(AllToDoItems);
            }
        }

        public void AddItem(ToDoItemViewModel toDo, MessageType message)
        {
            if (ReferenceEquals(toDo, null))
                throw new ArgumentNullException();
            AllToDoItems.Add(new ToDoToServer { item = toDo, MessageType = message });
            JSONWorker.Add(AllToDoItems);
        }

        public void UpdateItem(ToDoItemViewModel toDo)
        {
            var temp = ToDoItems.FirstOrDefault(a => a.item.UserId == toDo.UserId);
            ToDoItems.Add(new ToDoToServer { item = temp.item, MessageType = MessageType.UpDate });
            JSONWorker.Add(ToDoItems);
        }

        public void AddItemExist(ToDoItemViewModel toDo)
        {
            if (ReferenceEquals(toDo, null))
                throw new ArgumentNullException();
            AllToDoItems.Add(new ToDoToServer { item = toDo, MessageType = MessageType.None });
            JSONWorker.Add(AllToDoItems);
        }
    }
}
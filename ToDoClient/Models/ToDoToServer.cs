using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ToDoClient.Models;

namespace todoclient.Models
{
    public enum MessageType
    {
        Add = 0,
        Delete = 1,
        UpDate = 2,
        None = 3
    }
    public class ToDoToServer
    {
        public ToDoItemViewModel item { get; set; }
        public MessageType MessageType { get; set; }

    }
}
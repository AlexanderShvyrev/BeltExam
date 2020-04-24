using System.ComponentModel.DataAnnotations;

namespace BeltExam.Models
{
    public class Banana
    {
        [Key]
        public int BananaId{get; set;}
        public int ToDoId{get; set;}
        public int UserId{get; set;}
        public User NavUser{get; set;}
        public ToDo NavToDo{get; set;}
    }
}
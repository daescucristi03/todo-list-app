using System;

namespace TodoListApp.Models {
    class Task {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool Completed { get; set; }

        public Task(string title, string description, DateTime dueDate) {
            Title = title;
            Description = description;
            DueDate = dueDate;
            Completed = false;
        }
    }
}

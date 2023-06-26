using System;
using System.Collections.Generic;
using System.IO;
using TodoListApp.Models;

namespace TodoListApp.Data {
    class TaskFileManager {
        private string filePath;

        public TaskFileManager(string filePath) {
            this.filePath = filePath;
        }

        public void CheckAndCreateFile() {
            if (!File.Exists(filePath)) {
                File.Create(filePath).Close();
            }
        }

        public List<TodoListApp.Models.Task> LoadTasksFromFile() {
            List<TodoListApp.Models.Task> tasks = new List<TodoListApp.Models.Task>();

            try {
                using (StreamReader reader = new StreamReader(filePath)) {
                    string? line;

                    while ((line = reader.ReadLine()) != null) {
                        string[] taskData = line.Split('|');

                        if (taskData.Length == 4) {
                            string title = taskData[0];
                            string description = taskData[1];

                            DateTime dueDate = DateTime.Parse(taskData[2]);
                            bool completed = bool.Parse(taskData[3]);

                            TodoListApp.Models.Task task = new TodoListApp.Models.Task(title, description, dueDate);
                            task.Completed = completed;

                            tasks.Add(task);
                        }
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine("An error occurred while loading tasks: " + ex.Message);
            }

            return tasks;
        }

        public void SaveTasksToFile(List<TodoListApp.Models.Task> tasks) {
            try {
                using (StreamWriter writer = new StreamWriter(filePath)) {
                    foreach (TodoListApp.Models.Task task in tasks) {
                        string taskData = $"{task.Title}|{task.Description}|{task.DueDate}|{task.Completed}";
                        writer.WriteLine(taskData);
                    }
                }
            } catch (Exception ex) {
                Console.WriteLine("An error occurred while saving tasks: " + ex.Message);
            }
        }
    }
}
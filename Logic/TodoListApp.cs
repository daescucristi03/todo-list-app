using System;
using System.Collections.Generic;
using TodoListApp.Models;
using TodoListApp.Data;
using Task = TodoListApp.Models.Task;

namespace TodoListApp.Logic {
    enum MenuOptions {
        ViewTasks = 1,
        AddTask,
        EditTask,
        MarkTaskAsCompleted,
        DeleteTask,
        SaveAndExit
    }

    class TodoListManager {
        private List<Task> tasks;
        private TaskFileManager taskFileManager;

        public TodoListManager() {
            tasks = new List<Task>();
            taskFileManager = new TaskFileManager("tasks.txt");
        }

        public void Run() {
            taskFileManager.CheckAndCreateFile();
            tasks = taskFileManager.LoadTasksFromFile();

            while (true) {
                Console.Clear();
                DisplayMenu();
                string? choice = Console.ReadLine();
                Console.WriteLine();

                if (int.TryParse(choice, out int selectedOption)) {
                    switch (selectedOption) {
                        case (int)MenuOptions.ViewTasks:
                            ViewTasks();
                            break;
                        case (int)MenuOptions.AddTask:
                            AddTask();
                            break;
                        case (int)MenuOptions.EditTask:
                            EditTask();
                            break;
                        case (int)MenuOptions.MarkTaskAsCompleted:
                            MarkTaskAsCompleted();
                            break;
                        case (int)MenuOptions.DeleteTask:
                            DeleteTask();
                            break;
                        case (int)MenuOptions.SaveAndExit:
                            taskFileManager.SaveTasksToFile(tasks);
                            Console.WriteLine("Tasks saved. Exiting...");
                            return;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                } else {
                    Console.WriteLine("Invalid choice. Please try again.");
                }

                Console.WriteLine();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        private void DisplayMenu() {
            Console.WriteLine("Todo List Application");
            Console.WriteLine("---------------------");
            Console.WriteLine("1. View Tasks");
            Console.WriteLine("2. Add Task");
            Console.WriteLine("3. Edit Task");
            Console.WriteLine("4. Mark Task as Completed");
            Console.WriteLine("5. Delete Task");
            Console.WriteLine("6. Save and Exit");
            Console.WriteLine();
            Console.Write("Enter your choice (1-6): ");
        }

        private void ViewTasks() {
            Console.WriteLine("Tasks:");
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("| No. | Title     | Description | Due Date   | Status   |");
            // Console.WriteLine("--------------------------------------------------------");

            for (int i = 0; i < tasks.Count; i++) {
                Task task = tasks[i];
                string status = task.Completed ? "Completed" : "Pending";
                Console.WriteLine($"| {i + 1,-4} | {task.Title,-10} | {task.Description,-12} | {task.DueDate.ToString("yyyy-MM-dd"),-10} | {status,-8} |");
            }

            Console.WriteLine("--------------------------------------------------------");
        }

        private void AddTask() {
            Console.Write("Enter task title: ");
            string? title = Console.ReadLine();
            Console.Write("Enter task description: ");
            string? description = Console.ReadLine();
            Console.Write("Enter due date (yyyy-MM-dd): ");

            if (DateTime.TryParse(Console.ReadLine(), out DateTime dueDate)) {
                Task task = new Task(title ?? string.Empty, description ?? string.Empty, dueDate);
                tasks.Add(task);
                Console.WriteLine("Task added successfully.");
                taskFileManager.SaveTasksToFile(tasks);
            } else {
                Console.WriteLine("Invalid date format. Task not added.");
            }
        }

        private void EditTask() {
            ViewTasks();
            Console.Write("Enter the number of the task to edit: ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Count) {
                Task task = tasks[taskNumber - 1];

                Console.WriteLine("Enter new details for the task (leave blank to keep existing values):");
                Console.Write($"Title ({task.Title}): ");
                string? newTitle = Console.ReadLine();
                Console.Write($"Description ({task.Description}): ");
                string? newDescription = Console.ReadLine();
                Console.Write($"Due Date ({task.DueDate:yyyy-MM-dd}): ");
                string? newDueDateString = Console.ReadLine();

                if (!string.IsNullOrEmpty(newTitle))
                    task.Title = newTitle;

                if (!string.IsNullOrEmpty(newDescription))
                    task.Description = newDescription;

                if (!string.IsNullOrEmpty(newDueDateString) && DateTime.TryParse(newDueDateString, out DateTime newDueDate))
                    task.DueDate = newDueDate;

                Console.WriteLine("Task edited successfully.");
                taskFileManager.SaveTasksToFile(tasks);
            } else {
                Console.WriteLine("Invalid task number. Task not edited.");
            }
        }

        private void MarkTaskAsCompleted() {
            ViewTasks();
            Console.Write("Enter the number of the task to mark as completed: ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Count) {
                Task task = tasks[taskNumber - 1];
                task.Completed = true;
                Console.WriteLine("Task marked as completed.");
                taskFileManager.SaveTasksToFile(tasks);
            } else {
                Console.WriteLine("Invalid task number. Task not marked as completed.");
            }
        }

        private void DeleteTask() {
            ViewTasks();
            Console.Write("Enter the number of the task to delete: ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int taskNumber) && taskNumber >= 1 && taskNumber <= tasks.Count) {
                Task task = tasks[taskNumber - 1];
                tasks.Remove(task);
                Console.WriteLine("Task deleted successfully.");
                taskFileManager.SaveTasksToFile(tasks);
            } else {
                Console.WriteLine("Invalid task number. Task not deleted.");
            }
        }
    }
}

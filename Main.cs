using TodoListApp.Logic;

namespace TodoListApp {
    class Program {
        static void Main() {
            TodoListManager todoListManager = new TodoListManager();
            todoListManager.Run();
        }
    }
}

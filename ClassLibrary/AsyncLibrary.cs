namespace ClassLibrary
{
    public class AsyncLibrary
    {
        private List<string> _list = new List<string>();

        public void RunAwaiter()
        {
            _list.Clear();

            // 創建 tasks 但不開始運行
            Task task1 = new Task(() => AddToList(1000, 1));
            Task task2 = new Task(() => AddToList(1000, 2));
            Task task3 = new Task(() => AddToList(1000, 3));
            Task task4 = new Task(() => AddToList(1000, 4));

            // task2 開始
            task2.Start();

            // task1 開始
            task1.Start();

            // 在 task1 結束後開始
            task1.GetAwaiter().OnCompleted(() =>
            {
                task4.Start();
                task3.Start();
            });

            // 等待 task3 結束
            task3.Wait();

            // 等待 task4 結束
            task4.Wait();

            // 預期結果: task4, task3 一定在 task1 之後
            return;
        }

        public async Task<List<string>> RunAsync()
        {
            _list.Clear();

            // 創建 task1, task2 並執行
            Task task1 = AddToListAsync(1000, 1);
            Task task2 = AddToListAsync(1000, 2);

            // 等待 task1 完成
            await task1;

            // 創建 task3, task4 並執行
            Task task3 = AddToListAsync(1000, 3);
            Task task4 = AddToListAsync(1000, 4);
            
            // 等待 task2, task3, task4 結束
            await task2;
            await task3;
            await task4;

            // 預期結果: task3, task4 一定在 task1 之後
            return _list;
        }

        public async Task<List<string>> RunWhenAllAsync()
        {
            _list.Clear();

            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 20; i++)
            {
                Task task = AddToListAsync(1000, i);
                tasks.Add(task);
            }

            // 建立一個所有 Tasks 完成時才會完成的 Task
            Task allTask = Task.WhenAll(tasks);
            await allTask;

            return _list;
        }

        // 同步方法
        private void AddToList(int milliSeconds, int taskNum)
        {
            Thread.Sleep(milliSeconds);
            _list.Add($"I am task{taskNum}!");
        }

        // 非同步方法
        private async Task AddToListAsync(int milliSeconds, int taskNum)
        {
            await Task.Delay(milliSeconds);
            _list.Add($"I am task{taskNum}!");
        }
    }
}

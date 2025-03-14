namespace MyWebApi.Services
{
    public interface ILongRunningOperationService
    {
        void StartOperation();
        void StopOperation();
        bool IsRunning { get; }
    }

    public class LongRunningOperationService : ILongRunningOperationService
    {
        private CancellationTokenSource? _cancellationTokenSource;
        private Task? _operationTask;

        public bool IsRunning => _operationTask != null && !_operationTask.IsCompleted;

        public void StartOperation()
        {
            if (IsRunning)
            {
                throw new InvalidOperationException("Операция уже запущена.");
            }

            _cancellationTokenSource = new CancellationTokenSource();
            _operationTask = Task.Run(() => PerformLongRunningOperation(_cancellationTokenSource.Token), _cancellationTokenSource.Token);
        }

        public void StopOperation()
        {
            if (!IsRunning)
            {
                throw new InvalidOperationException("Операция не запущена.");
            }

            _cancellationTokenSource?.Cancel();
            _operationTask = null; // Очищаем ссылку на задачу после отмены
        }

        private async Task PerformLongRunningOperation(CancellationToken cancellationToken)
        {
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    Console.WriteLine($"Шаг {i + 1}...");
                    await Task.Delay(1000, cancellationToken); // Имитация работы
                }
                Console.WriteLine("Операция завершена успешно.");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Операция была отменена.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при выполнении операции: {ex.Message}");
            }
        }
    }
}

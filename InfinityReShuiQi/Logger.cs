using System.Text;
using System;

namespace InfinityReShuiQi
{
    public class Logger : IDisposable
    {
        private FileStream fileStream;
        private SemaphoreSlim semaphore = new SemaphoreSlim(1);

        public Logger(string logFilePath)
        {
            fileStream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
        }

        public async Task LogAsync(string message)
        {
            try
            {
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}\n";
                byte[] logEntryBytes = Encoding.UTF8.GetBytes(logEntry);

                await semaphore.WaitAsync();
                try
                {
                    // 异步追加到文件
                    await fileStream.WriteAsync(logEntryBytes);
                }
                finally
                {
                    semaphore.Release();
                }
            }
            catch (Exception ex)
            {
                // 如果写入日志时发生异常，可以将异常信息输出到控制台
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }

        public void Dispose()
        {
            fileStream?.Close();
            fileStream?.Dispose();
            semaphore?.Dispose();
        }
    }

}

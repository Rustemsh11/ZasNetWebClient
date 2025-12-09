using System;

namespace ZasNetWebClient.Services
{
    public class NotificationService
    {
        public event Action<string>? OnError;
        public event Action<string>? OnInfo;
        public event Action? OnClear;

        public void ShowError(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            OnError?.Invoke(message);
        }

        public void ShowInfo(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            OnInfo?.Invoke(message);
        }

        public void Clear()
        {
            OnClear?.Invoke();
        }
    }
}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    public class Logger
    {
        // 싱글톤 인스턴스
        private static readonly Logger _instance = new Logger();
        public static Logger Instance => _instance;

        // 외부에서 접근할 수 있는 정적 로그 메서드
        public static void Log(string message)
        {
            Instance.LogInternal(message);
        }

        // 이벤트: UI나 다른 클래스에서 구독 가능
        public event Action<string>? LogReceived;

        // 내부에서 호출되는 실제 구현
        private void LogInternal(string message)
        {
            string output = $"[{DateTime.Now:HH:mm:ss}] {message}";            
            LogReceived?.Invoke(output); // UI에 이벤트 알림
        }
    }
}

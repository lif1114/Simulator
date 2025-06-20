using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Simulator
{
    public class ServerSocket
    {
        private TcpListener? _listener;
        private bool _isRunning;        
        public void Start(int port)
        {
            _listener = new TcpListener(IPAddress.Any, port);
            _listener.Start();
            _isRunning = true;

            Logger.Log($"[서버 시작] 포트 {port}에서 대기 중...");            

            // 클라이언트 수락 대기 스레드
            Thread acceptThread = new Thread(AcceptClients);
            acceptThread.Start();
        }

        private void AcceptClients()
        {
            while (_isRunning)
            {
                try
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    Logger.Log("[클라이언트 연결됨]");

                    // 클라이언트 처리 스레드 시작
                    Thread clientThread = new Thread(() => HandleClient(client));
                    clientThread.Start();
                }
                catch (SocketException ex)
                {
                    Logger.Log($"[소켓 예외] {ex.Message}");
                }
            }
        }

        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            try
            {
                while (true)
                {
                    int byteCount = stream.Read(buffer, 0, buffer.Length);
                    if (byteCount == 0)
                    {
                        Logger.Log("[클라이언트 연결 종료]");
                        break;
                    }

                    string received = Encoding.UTF8.GetString(buffer, 0, byteCount);
                    Logger.Log($"[수신 데이터] {received}");

                    OnDataReceived(client, received);
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"[에러] {ex.Message}");
            }
            finally
            {
                stream.Close();
                client.Close();
            }
        }
        protected virtual void OnDataReceived(TcpClient client, string message)
        {
            Logger.Log("[기본 처리] 수신된 메시지를 처리할 메서드가 정의되지 않았습니다.");
        }

        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();
            Logger.Log("[서버 종료]");
        }
    }
}

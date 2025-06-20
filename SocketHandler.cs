using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Simulator.Controller;

namespace Simulator
{
    public enum CommandTypes
    {
        READ_DI, READ_DO, WRITE_DI, WRITE_DO, MAX
    }
    internal class CommandInfo
    {
        public CommandTypes CommandIdx { get; set; }
        public string CommandName { get; set; }
        public Action Handler { get; set; }
        public bool ResponseRequired { get; set; }
    }
    internal class SocketHandler : ServerSocket
    {
        private readonly DigitalInputController  _diCtrl = new();
        private readonly DigitalOutputController _doCtrl = new();
        private readonly AnalogInputController   _aiCtrl = new();
        private readonly AnalogOutputController  _aoCtrl = new();
        private readonly List<CommandInfo> _commands = new();

        public SocketHandler()
        {
            RegisterCommand();
            Start(5000);
        }

        private void RegisterCommand()
        {
            _commands.Add(new CommandInfo
            {
                CommandIdx = CommandTypes.READ_DI,
                CommandName = CommandTypes.READ_DI.ToString(),
                Handler = ReadDI,
                ResponseRequired = true
            });
            _commands.Add(new CommandInfo
            {
                CommandIdx = CommandTypes.READ_DO,
                CommandName = CommandTypes.READ_DO.ToString(),
                Handler = ReadDO,
                ResponseRequired = true
            });
            _commands.Add(new CommandInfo
            {
                CommandIdx = CommandTypes.WRITE_DI,
                CommandName = CommandTypes.WRITE_DI.ToString(),
                Handler = WriteDI,
                ResponseRequired = false
            });
            _commands.Add(new CommandInfo
            {
                CommandIdx = CommandTypes.WRITE_DO,
                CommandName = CommandTypes.WRITE_DO.ToString(),
                Handler = WriteDO,
                ResponseRequired = false
            });
        }

        protected override void OnDataReceived(TcpClient client, string message)
        {
            string trimmed = message.Trim().ToUpper();
            string[] parts = trimmed.Split(',');
            string command = parts.Length > 0 ? parts[0] : string.Empty;

            // 유효한 커맨드를 한번에 검색
            var cmdInfo = _commands.FirstOrDefault(cmd =>
                cmd.CommandName.Equals(command, StringComparison.OrdinalIgnoreCase));

            if (cmdInfo != null)
            {
                Logger.Log($"[유효 커맨드] {command}");

                // 필요 시 이 안에서 직접 실행도 가능
                cmdInfo.Handler();                
            }
            else
            {
                Logger.Log($"[무효 커맨드] '{command}'는 등록되지 않은 커맨드입니다.");
            }
        }

        public bool IsValidCommand(string name)
        {
            return _commands.Any(cmd =>
                cmd.CommandName.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void ReadDI()
        {

        }

        public void ReadDO()
        {

        }

        public void WriteDI()
        {

        }

        public void WriteDO()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer
{
    public class CmdExec
    {
        private Dictionary<string, List<string>> data;
        
        public CmdExec()
        {
            this.data = new Dictionary<string, List<string>>();
        }
        public string Exec(string input)
        {
            string[] args = input.Split(',');
            string cmd = args[0];
            if (cmd == "INSERT")
            {
                if (args.Length < 3)
                    return "argument invalid.";
                Insert(args[1], args[2]);
                return "INSERT Done.";
            }
            else if(cmd == "READ")
            {
                if (args.Length < 2)
                    return "argument invalid.";
                return Read(args[1]);
            }
            else
            {
                return "Command not found.";
            }
        }
        public void Insert(string key, string _data) {
            if (this.data.ContainsKey(key))
            {
                this.data[key].Add(_data);
            }
            else
            {
                this.data.Add(key, new List<string>());
                this.data[key].Add(_data);
            }
        }
        public string Read(string key)
        {
            if (this.data.ContainsKey(key))
            {
                return string.Join(",", this.data[key].ToArray()); ;
            }
            else
            {
                return "key not found.";
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowershellAI
{
    internal class Response
    {
        //Private fields
        private string Command = "";
        private Dictionary<string, string> References = new Dictionary<string, string>();
        
        //Constructor
        public Response(string Command, Dictionary<string, string> References)
        {
            setCommand(Command);
            setReferences(References);
        }
        //setters getters
        public void setCommand(string Command)
        {
            this.Command = Command;
        }
        public string getCommand()
        {
            return this.Command;
        }
        public void setReferences(Dictionary<string, string> References)
        {
            this.References = References;
        }
        public Dictionary<string, string> getReferences()
        {
            return this.References;
        }
        //Override tostring to verify response objects during requests
        public override string ToString()
        {
            string text = $"Command: {getCommand()}\nReferences: [";
            foreach (var reference in getReferences())
            {
                text += $"\n{reference.Key}: {reference.Value}";
            }
            text += "\n]";
            return text;
        }
    }
}

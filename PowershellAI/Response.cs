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
        private string _command = "";
        private Dictionary<string, string> _references = new();
        
        //Constructor
        public Response(string command, Dictionary<string, string> references)
        {
            SetCommand(command);
            SetReferences(references);
        }
        //setters getters
        public void SetCommand(string command)
        {
            _command = command;
        }
        public string GetCommand()
        {
            return _command;
        }
        public void SetReferences(Dictionary<string, string> references)
        {
            _references = references;
        }
        public Dictionary<string, string> GetReferences()
        {
            return _references;
        }
        //Override ToString to verify response objects during requests
        public override string ToString()
        {
            var text = $"Command: {GetCommand()}\nReferences: [{string.Join(
                "\n",
                GetReferences().Select(reference => $"{reference.Key}: {reference.Value}")
            )}\n]";
            return text;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class SymbolAttribute : Attribute
    {
        public string Symbol { get; set; }

        public SymbolAttribute(string symbol)
        {
            Symbol = symbol;
        }
    }
}

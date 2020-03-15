using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanPort.RDSDecoder.CommandParser.Commands
{
    public class UnsupportedRDSCommand : RDSCommand
    {
        //Represents just an unsupported command
        internal override void ReadCommand(int groupBSpecial, ushort groupC, ushort groupD)
        {
            //Do nothing...
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanPort.RDSDecoder.CommandParser.Commands
{
    public class RadioTextRDSCommand : RDSCommand
    {
        public char letterA;
        public char letterB;
        public char letterC;
        public char letterD;
        
        internal override void ReadCommand(int groupBSpecial, ushort groupC, ushort groupD)
        {
            //Decode radio text characters
            char letterA = (char)(groupC & 0xff);
            char letterB = (char)((groupC >> 8) & 0xff);
            char letterC = (char)(groupD & 0xff);
            char letterD = (char)((groupD >> 8) & 0xff);

            //Set
            this.letterA = letterA;
            this.letterB = letterB;
            this.letterC = letterC;
            this.letterD = letterD;
        }
    }
}

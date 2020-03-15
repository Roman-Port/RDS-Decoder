using RomanPort.RDSDecoder.CommandParser;
using SDRSharp.Radio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanPort.RDSDecoder
{
    public delegate void RDSStationNameFrameBeforeUpdatedEventArgs(char[] stationName, int index, char charA, char charB); //Used for updating chunks of the station name - not the whole thing
    public delegate void RDSStationNameFrameUpdatedEventArgs(char[] stationName, int index); //Used for updating chunks of the station name - not the whole thing
    public delegate void RDSStationNameUpdatedEventArgs(char[] stationName); //Used when the station name has been updated
    public delegate void RDSFrameReceivedEventArgs(RdsFrame frame); //Called when we get any frame
    public delegate void RDSCommandReceivedEventArgs(RDSCommand frame); //Called when we get any command
}

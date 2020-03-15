using RomanPort.RDSDecoder.CommandParser;
using RomanPort.RDSDecoder.CommandParser.Commands;
using SDRSharp.Radio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RomanPort.RDSDecoder
{
    public class RDSSession
    {
        /// <summary>
        /// The program identification code
        /// </summary>
        public ushort programIdentificationCode;

        /// <summary>
        /// Set to true when ANY RDS frame is found
        /// </summary>
        public bool rdsSupported;

        /// <summary>
        /// Called when a CHUNK of the RDS station name is updated. Will not contain a complete message and will likely look incorrect! Contains the station buffer before it was updated
        /// </summary>
        public event RDSStationNameFrameBeforeUpdatedEventArgs OnBeforeRdsStationNameFrameUpdated;

        /// <summary>
        /// Called when a CHUNK of the RDS station name is updated. Will not contain a complete message and will likely look incorrect!
        /// </summary>
        public event RDSStationNameFrameUpdatedEventArgs OnRdsStationNameFrameUpdated;

        /// <summary>
        /// Called when a new RDS station name has been recieved.
        /// </summary>
        public event RDSStationNameUpdatedEventArgs OnRdsStationNameUpdated;

        /// <summary>
        /// Called when a low level frame is found
        /// </summary>
        public event RDSFrameReceivedEventArgs OnRDSFrameReceived;

        /// <summary>
        /// Called when a mid level command is found
        /// </summary>
        public event RDSCommandReceivedEventArgs OnRDSCommandReceived;

        /// <summary>
        /// The current station name buffer - MAY NOT BE VALID
        /// </summary>
        public char[] stationNameBuffer;

        /// <summary>
        /// The current station name
        /// </summary>
        public string stationName;

        /// <summary>
        /// Has the station name been fully recieved?
        /// </summary>
        public bool hasFullName;

        /// <summary>
        /// Set to true when we first get radio data
        /// </summary>
        private bool _hasFirstRadioTextBatchDownloaded;

        public RDSSession()
        {
            Reset();
        }

        public void Reset()
        {
            _hasFirstRadioTextBatchDownloaded = false;
            hasFullName = false;
            stationNameBuffer = new char[8];
            programIdentificationCode = 0;
            rdsSupported = false;
            stationName = null;
        }

        public void PushChange(RdsFrame frame)
        {
            //Fire events
            rdsSupported = true;
            OnRDSFrameReceived?.Invoke(frame);

            //Get the command
            RDSCommand c = RDSCommand.ReadRdsFrame(frame);
            OnRDSCommandReceived?.Invoke(c);

            //Set info
            programIdentificationCode = c.programIdentificationCode;

            //Switch on the type
            Type commandType = c.GetType();
            if (commandType == typeof(BasicDataRDSCommand))
                HandleBasicDataRDSCommand((BasicDataRDSCommand)c);
        }

        private void HandleBasicDataRDSCommand(BasicDataRDSCommand cmd)
        {
            //Set radio text
            //Sent event
            OnBeforeRdsStationNameFrameUpdated?.Invoke(stationNameBuffer, cmd.stationNameIndex, cmd.letterA, cmd.letterB);

            //If this is the first index, the previous index is OK. Send an event for it
            if (cmd.stationNameIndex == 0)
            {
                if (_hasFirstRadioTextBatchDownloaded)
                {
                    hasFullName = true;
                    OnRdsStationNameUpdated?.Invoke(stationNameBuffer);
                    stationName = new string(stationNameBuffer);
                } else
                {
                    _hasFirstRadioTextBatchDownloaded = true;
                }
            }

            //Update and send event
            stationNameBuffer[cmd.stationNameIndex+0] = cmd.letterA;
            stationNameBuffer[cmd.stationNameIndex+1] = cmd.letterB;
            OnRdsStationNameFrameUpdated?.Invoke(stationNameBuffer, cmd.stationNameIndex);
        }
    }

    
}

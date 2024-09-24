﻿using System.Text;
using IRIS.Protocols.LINE.Data;
using IRIS.Utility;

namespace IRIS.Protocols.LINE
{
    /// <summary>
    /// Represents a protocol for basic text communication with a line limit of 128 characters.
    /// </summary>
    public struct LineProtocol : IProtocol
    {
        public static byte[] EncodeData<TData>(TData inputData) where TData : unmanaged
        {
            // Check if TData is LineData
            if (inputData is not LineTransactionData lineData)
                throw new NotSupportedException("This feature is not supported for LINE protocol");
            
            // Copy data from input to output
            return Encoding.ASCII.GetBytes(lineData.data.ToString());
        }

        public static unsafe bool DecodeData<TData>(byte[]? inputData, out TData outputData) where TData : unmanaged
        {
            outputData = new TData();
            
            // Check if input data is null
            if (inputData == null) return false;
           
            // Check if TData is LineReadResponse
            if (outputData is not LineTransactionData)
                throw new NotSupportedException("This feature is not supported for LINE protocol");
            
            // Create pointer access to the output data
            fixed (TData* outputDataPtr = &outputData)
            {
                // Cast output data to LineReadResponse
                LineTransactionData* lineReadResponse = (LineTransactionData*) outputDataPtr;

                // We need to find first ASCII character to trim
                // Windows shitty handshake, because SerialPort class is a piece of 
                // garbage and doesn't provide any way to disable it.
                //
                // We could cover ASCII checking on the device side, but this is just
                // an additional check to make sure we don't have any garbage data.
                int firstValidIndex = -1;
                for(int i = 0; i < inputData.Length; i++)
                {
                    // Check if character is valid (ASCII)
                    if (inputData[i] >= 128) continue;
                    firstValidIndex = i;
                    break;
                }
                
                // Copy data from input to output
                string text = Encoding.ASCII.GetString(inputData, firstValidIndex, inputData.Length - firstValidIndex);
                lineReadResponse->data.CopyFrom(text);

                // Check if input data length is less than 128, if not, return false
                // to indicate issue with decoding
                return inputData.Length >= UnmanagedString128.MAX_LENGTH;
            }
        }
    }
}
using System.Text;
using IRIS.Protocols.RUSTIC.Data;

namespace IRIS.Protocols.RUSTIC
{
    public struct RusticProtocol : IProtocol
    {
        public static byte[] EncodeData<TData>(TData inputData) where TData : unmanaged
        {
            // Calculate the request text based on RUSTIC specification
            string? requestText = inputData switch
            {
                // Check data type and encode it
                GetValueRequestData getRequest => $"{getRequest.name}=?\r\n",
                SetValueRequestData setRequest => $"{setRequest.name}={setRequest.value}\r\n",
                _ => null
            };

            // If the request text is not null, return the ASCII-encoded bytes
            if (requestText != null) return System.Text.Encoding.ASCII.GetBytes(requestText);
            
            // Otherwise, throw an exception
            throw new NotSupportedException("Unsupported data type");
        }

        public static unsafe bool DecodeData<TData>(byte[] inputData, out TData outputData) where TData : unmanaged
        {
            // Decode the ASCII-encoded bytes to a string
            string responseText = Encoding.ASCII.GetString(inputData);
            
            // Split string via equals sign
            string[] splitResponse = responseText.Split('=');
            
            outputData = default;
            
            // Check if the split response is valid
            if (splitResponse.Length != 2) return false;

            switch (outputData)
            {
                // Check data type and decode it
                case GetValueResponseData:
                {
                    // Create pointer access to the response data
                    fixed(TData* getResponseDataPtr = &outputData)
                    {
                        // Convert the obtained ptr to correct type
                        GetValueResponseData* getResponsePtr = (GetValueResponseData*) getResponseDataPtr;
                    
                        // Copy the name and value to the response data
                        getResponsePtr->name.CopyFrom(splitResponse[0]);
                        getResponsePtr->value.CopyFrom(splitResponse[1]);
                    
                        // Return true to indicate successful decoding
                        return true;
                    }
                }
                case SetValueResponseData:
                {
                    // Create pointer access to the response data
                    fixed(TData* setResponseDataPtr = &outputData)
                    {
                        // Convert the obtained ptr to correct type
                        SetValueResponseData* setResponsePtr = (SetValueResponseData*) setResponseDataPtr;
                    
                        // Copy the name and value to the response data
                        setResponsePtr->name.CopyFrom(splitResponse[0]);
                        setResponsePtr->value.CopyFrom(splitResponse[1]);
                    
                        // Return true to indicate successful decoding
                        return true;
                    }
                }
                default:
                    // Throw an exception if the data type is not supported
                    throw new NotSupportedException("Unsupported data type");
            }
        }
    }
}
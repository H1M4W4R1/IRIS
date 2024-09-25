using System.Text;
using IRIS.DataEncoders.RUSTIC.Data;

namespace IRIS.DataEncoders.RUSTIC
{
    public struct RusticDataEncoder : IDataEncoder
    {
        public static byte[] EncodeData<TData>(TData inputData) where TData : struct
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

        public static bool DecodeData<TData>(byte[] inputData, out TData outputData) where TData : struct
        {
            // Decode the ASCII-encoded bytes to a string
            string responseText = Encoding.ASCII.GetString(inputData);

            // Clear the response text from CR LF
            responseText = responseText.Replace("\r", "").Replace("\n", "");

            // Split string via equals sign
            string[] splitResponse = responseText.Split('=');

            // Create temporary memory allocation for the output data
            outputData = new TData();

            // Check if the split response is valid
            if (splitResponse.Length != 2) return false;

            switch (outputData)
            {
                // Check data type and decode it
                case GetValueResponseData:
                {
                    GetValueResponseData response0 = new(splitResponse[0], splitResponse[1]);
                    
                    // Convert the obtained response to the correct type
                    outputData = (TData) Convert.ChangeType(response0, typeof(TData));
                    return true;
                }
                case SetValueResponseData:
                {
                    SetValueResponseData response1 = new(splitResponse[0], splitResponse[1]);
                    
                    // Convert the obtained response to the correct type
                    outputData = (TData) Convert.ChangeType(response1, typeof(TData));
                    return true;
                }
                default:
                    // Throw an exception if the data type is not supported
                    throw new NotSupportedException("Unsupported data type");
            }
        }
    }
}
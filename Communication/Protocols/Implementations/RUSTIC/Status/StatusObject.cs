using IRIS.Communication.Protocols.Implementations.Abstractions;
using System.Text;

namespace IRIS.Communication.Protocols.Implementations.RUSTIC.Status
{
    public class StatusObject : IReceivedDataObject<StatusObject>
    {
        #region STATUSES
        private const string OK_ID = "OK";
        private static StatusObject OK = new OKStatus();

        private static StatusObject UNKNOWN = new UnknownStatus();
        #endregion

        public static StatusObject Decode(byte[] data)
        {
            // Decode text
            var text = Encoding.ASCII.GetString(data);

            if (text.StartsWith(OK_ID))
                return OK;
                        

            return UNKNOWN;
        }
    }
}

using System.Runtime.InteropServices;

namespace IRIS.Utility
{
    /// <summary>
    /// Represents a fixed-size unmanaged string of 128 characters.
    /// Also contains additional byte for null-terminator.
    /// </summary>
    public unsafe struct UnmanagedString128() : IDisposable
    {
        public const int MAX_LENGTH = 128; 
        
        private char* _data = (char*) Marshal.AllocHGlobal(MAX_LENGTH * sizeof(char) + 1);

        public char this[int index]
        {
            get
            {
                return index switch
                {
                    < 0 => _data[0],
                    >= 128 => _data[MAX_LENGTH - 1],
                    _ => _data[index]
                };
            }
            set
            {
                if (index is < 0 or >= MAX_LENGTH) return;
                _data[index] = value;
            }
        }
        
        public void CopyFrom(string source)
        {
            int strLength = Math.Min(source.Length, MAX_LENGTH);
            
            for (int i = 0; i < strLength; i++)
                _data[i] = source[i];
            
            // Null-terminate the string
            _data[strLength] = '\0';
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal((IntPtr)_data);
            _data = null;
        }

        public override string ToString()
        {
            return new string(_data);
        }

        public static UnmanagedString128 FromString(string source)
        {
            UnmanagedString128 result = new();
            result.CopyFrom(source);
            return result;
        }
    }
}
using System.Runtime.InteropServices;

namespace IRIS.Utility
{
    public unsafe struct UnmanagedArray<TUnmanagedType>(int length) : IDisposable
        where TUnmanagedType : unmanaged
    {
        /// <summary>
        /// Internal data array
        /// </summary>
        private TUnmanagedType* _data = (TUnmanagedType*) Marshal.AllocHGlobal(length * sizeof(TUnmanagedType));

        public TUnmanagedType this[int index]
        {
            get
            {
                if(index < 0) return _data[0];
                if(index >= length) return _data[length - 1];
                return _data[index];
            }
            set
            {
                if (index < 0 || index >= length) return;
                _data[index] = value;
            }
        }
        
        /// <summary>
        /// Copy data from source array to unmanaged array
        /// </summary>
        public void CopyFrom(TUnmanagedType[] source)
        {
            for (int i = 0; i < Math.Max(source.Length, length); i++)
                _data[i] = source[i];
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal((IntPtr)_data);
            _data = null;
            length = 0;
        }
        
        
    }
}
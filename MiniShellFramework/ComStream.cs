// <copyright>
//     Copyright (c) Victor Derks. See README.TXT for the details of the software licence.
// </copyright>

using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace MiniShellFramework
{
    /// <summary>
    /// Provides a managed wrapper around a COM object that supports the COM IStream interface.
    /// </summary>
    /// <remarks>
    /// The Shell prefers to pass IStream interfaces to shell extensions rather then filenames.
    /// This to abstract away that some shell items are not actually items on the filesystem.
    /// Most .NET base class libary code expect a Stream derived object when dealing with file system items.
    /// This class wraps the COM IStream interface and allows it to be used as a .NET stream object.
    /// </remarks>
    public class ComStream : Stream
    {
        private IStream stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="ComStream"/> class.
        /// </summary>
        /// <param name="stream">The COM stream interface.</param>
        public ComStream(IStream stream)
        {
            Contract.Requires(stream != null);
            this.stream = stream;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override long Length
        {
            get 
            { 
                //if (m_pOrigStream == 0)
                //        throw new ObjectDisposedException("m_pOrigStream");

                STATSTG statstg;
                stream.Stat(out statstg, 1 /* STATFLAG_NONAME */);
                Contract.Assume(statstg.cbSize >= 0);
                return statstg.cbSize;
            }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public unsafe override int Read(byte[] buffer, int offset, int count)
        {
            uint bytesRead = 0;

            if (offset != 0)
            {
                var tmpBuffer = new byte[count];
                stream.Read(tmpBuffer, count, new IntPtr(&bytesRead));
                Array.Copy(tmpBuffer, 0, buffer, offset, bytesRead);
            }
            else
            {
                stream.Read(buffer, count, new IntPtr(&bytesRead));
            }

            return (int) bytesRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is greater than the buffer length. </exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="buffer"/> is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// <paramref name="offset"/> or <paramref name="count"/> is negative. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support writing. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            base.Close();

            if (stream == null)
                return;

            stream.Commit(0);  // STGC_DEFAULT
            System.Runtime.InteropServices.Marshal.ReleaseComObject(stream);
            stream = null;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices.ComTypes;

namespace MiniShellFramework
{
    public class ComStream : Stream
    {
        IStream stream;

        public ComStream(IStream stream)
        {
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

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
 	        base.Close();

            if (stream != null)
            {
                stream.Commit(0);  // STGC_DEFAULT
                System.Runtime.InteropServices.Marshal.ReleaseComObject(stream);
                stream = null;
            }
        }
    }
}

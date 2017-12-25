﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HgVersionTests.Helpers
{
    public class TestStream : Stream
    {
        private readonly string path;
        private readonly TestFileSystem testFileSystem;
        private MemoryStream underlying = new MemoryStream();

        public TestStream(string path, TestFileSystem testFileSystem)
        {
            this.path = path;
            this.testFileSystem = testFileSystem;
        }

        protected override void Dispose(bool disposing)
        {
            Flush();
            base.Dispose(disposing);
        }

        public override void Flush()
        {
            underlying.Position = 0;
            var readToEnd = new StreamReader(underlying).ReadToEnd();
            testFileSystem.WriteAllText(path, readToEnd);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return underlying.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            underlying.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return underlying.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            underlying.Write(buffer, offset, count);
        }

        public override void WriteByte(byte value)
        {
            base.WriteByte(value);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return base.BeginWrite(buffer, offset, count, callback, state);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            return base.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override bool CanRead { get { return underlying.CanRead; } }
        public override bool CanSeek { get { return underlying.CanSeek; } }
        public override bool CanWrite { get { return underlying.CanWrite; } }
        public override long Length { get { return underlying.Length; } }

        public override long Position
        {
            get { return underlying.Position; }
            set { underlying.Position = value; }
        }

    }
}
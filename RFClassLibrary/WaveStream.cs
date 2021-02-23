//  THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY
//  KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
//  IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR
//  PURPOSE.
//
//  This material may not be duplicated in whole or in part, except for 
//  personal use, without the express written consent of the author. 
//
//  Email:  ianier@hotmail.com
//
//  Copyright (C) 1999-2003 Ianier Munoz. All Rights Reserved.

using System;
using System.IO;

namespace WaveLib
{
	/// <summary>
    /// Copyright (C) 1999-2003 Ianier Munoz. All Rights Reserved.
	/// </summary>
    public class WaveStream : Stream, IDisposable
	{
		private Stream m_Stream;
		private long m_DataPos;
		private long m_Length;

		private WaveFormat m_Format;

		/// <summary>
		/// 
		/// </summary>
        public WaveFormat Format
		{
			get { return m_Format; }
		}

		private string ReadChunk(BinaryReader reader)
		{
			byte[] ch = new byte[4];
			reader.Read(ch, 0, ch.Length);
			return System.Text.Encoding.ASCII.GetString(ch);
		}

		private void ReadHeader()
		{
			BinaryReader Reader = new BinaryReader(m_Stream);
			if (ReadChunk(Reader) != "RIFF")
				throw new Exception("Invalid file format");

			Reader.ReadInt32(); // File length minus first 8 bytes of RIFF description, we don't use it

			if (ReadChunk(Reader) != "WAVE")
				throw new Exception("Invalid file format");

			if (ReadChunk(Reader) != "fmt ")
				throw new Exception("Invalid file format");

			if (Reader.ReadInt32() != 16) // bad format chunk length
				throw new Exception("Invalid file format");

			m_Format = new WaveFormat(22050, 16, 2); // initialize to any format
			m_Format.wFormatTag = Reader.ReadInt16();
			m_Format.nChannels = Reader.ReadInt16();
			m_Format.nSamplesPerSec = Reader.ReadInt32();
			m_Format.nAvgBytesPerSec = Reader.ReadInt32();
			m_Format.nBlockAlign = Reader.ReadInt16();
			m_Format.wBitsPerSample = Reader.ReadInt16(); 

			// assume the data chunk is aligned
			while(m_Stream.Position < m_Stream.Length && ReadChunk(Reader) != "data")
				;

			if (m_Stream.Position >= m_Stream.Length)
				throw new Exception("Invalid file format");

			m_Length = Reader.ReadInt32();
			m_DataPos = m_Stream.Position;

			Position = 0;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileName"></param>
        public WaveStream(string fileName) : this(new FileStream(fileName, FileMode.Open))
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="S"></param>
        public WaveStream(Stream S)
		{
			m_Stream = S;
			ReadHeader();
		}

        /// <summary>
        /// destructor
        /// </summary>
		~WaveStream()
		{
			Dispose();
		}
        
        
        /// <summary>
        /// public void Dispose()
        /// </summary>
        new public void Dispose()//09/15/2006 rgc added new keyword
		{
			if (m_Stream != null)
				m_Stream.Close();
			GC.SuppressFinalize(this);
		}

		
        /// <summary>
        /// 
        /// </summary>
        public override bool CanRead
		{
			get { return true; }
		}

        /// <summary>
        /// 
        /// </summary>
		public override bool CanSeek
		{
			get { return true; }
		}

		/// <summary>
		/// 
		/// </summary>
        public override bool CanWrite
		{
			get { return false; }
		}
		
        /// <summary>
        /// 
        /// </summary>
        public override long Length
		{
			get { return m_Length; }
		}

        /// <summary>
        /// 
        /// </summary>
		public override long Position
		{
			get { return m_Stream.Position - m_DataPos; }
			set { Seek(value, SeekOrigin.Begin); }
		}

        /// <summary>
        /// 
        /// </summary>
		public override void Close()
		{
			Dispose();
		}

        /// <summary>
        /// 
        /// </summary>
		public override void Flush()
		{
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="len"></param>
		public override void SetLength(long len)
		{
			throw new InvalidOperationException();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="o"></param>
        /// <returns></returns>
		public override long Seek(long pos, SeekOrigin o)
		{
			switch(o)
			{
				case SeekOrigin.Begin:
					m_Stream.Position = pos + m_DataPos;
					break;
				case SeekOrigin.Current:
					m_Stream.Seek(pos, SeekOrigin.Current);
					break;
				case SeekOrigin.End:
					m_Stream.Position = m_DataPos + m_Length - pos;
					break;
			}
			return this.Position;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="ofs"></param>
        /// <param name="count"></param>
        /// <returns></returns>
		public override int Read(byte[] buf, int ofs, int count)
		{
			int toread = (int)Math.Min(count, m_Length - Position);
			return m_Stream.Read(buf, ofs, toread);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buf"></param>
        /// <param name="ofs"></param>
        /// <param name="count"></param>
		public override void Write(byte[] buf, int ofs, int count)
		{
			throw new InvalidOperationException();
		}
	}
}

using System;
//added
using System.IO; // Stream

namespace RFClassLibrary
{
    /// <summary>
    /// 05/28/03 Rick Crone
    /// </summary>
    public class RCSound
    {
        private WaveLib.WaveOutPlayer m_Player;
        private WaveLib.WaveFormat m_Format;
        private Stream m_AudioStream;

        /// <summary>
        /// empty constructor
        /// This class to make playing wav files easy.
        /// 05/28/03 Rick Crone
        /// </summary>
        public RCSound()
        {

        }

        private void Stop()
        {
            if (m_Player != null)
                try
                {
                    m_Player.Dispose();
                }
                finally
                {
                    m_Player = null;
                }
        }

        private void Filler(IntPtr data, int size)
        {
            byte[] b = new byte[size];
            if (m_AudioStream != null)
            {
                int pos = 0;
                while (pos < size)
                {
                    int toget = size - pos;
                    int got = m_AudioStream.Read(b, pos, toget);
                    if (got < toget)
                        m_AudioStream.Position = 0; // loop if the file ends
                    pos += got;
                }
            }
            else
            {
                for (int i = 0; i < b.Length; i++)
                    b[i] = 0;
            }
            System.Runtime.InteropServices.Marshal.Copy(b, 0, data, size);
        }

        private void CloseFile()
        {
            Stop();
            if (m_AudioStream != null)
                try
                {
                    m_AudioStream.Close();
                }
                finally
                {
                    m_AudioStream = null;
                }
        }


        /// <summary>
        /// Play the wav file.
        /// Rick Crone
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="millisec"></param>
        public void PlayWavFile(string strFileName, int millisec)
        {
            //load
            CloseFile();
            //WaveLib.WaveStream S = new WaveLib.WaveStream("C:\\FORD\\CARBRAKE.WAV");
            WaveLib.WaveStream S = new WaveLib.WaveStream(strFileName);

            m_Format = S.Format;
            if (m_Format.wFormatTag != (short)WaveLib.WaveFormats.Pcm && m_Format.wFormatTag != (short)WaveLib.WaveFormats.Float)
                throw new Exception("Only PCM files are supported");

            m_AudioStream = S;
            //play
            Stop();
            if (m_AudioStream != null)
            {
                m_AudioStream.Position = 0;
                m_Player = new WaveLib.WaveOutPlayer(-1, m_Format, 16384, 3, new WaveLib.BufferFillEventHandler(Filler));
            }

            Time.Wait(millisec);
            Stop();

            return;
        }
    }
}

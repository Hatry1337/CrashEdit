using System;

namespace Crash.Audio
{
    public sealed class RIFFData : RIFFItem
    {
        private byte[] data;

        public RIFFData(string name,byte[] data) : base(name)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (data == null)
                throw new ArgumentNullException("data");
            this.data = data;
        }

        public override int Length
        {
            get { return data.Length + 8; }
        }

        public override byte[] Save()
        {
            byte[] result = new byte [8 + data.Length];
            result[0] = (byte)Name[0];
            result[1] = (byte)Name[1];
            result[2] = (byte)Name[2];
            result[3] = (byte)Name[3];
            BitConv.ToInt32(result,4,data.Length);
            data.CopyTo(result,8);
            return result;
        }
    }
}
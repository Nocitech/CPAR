using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPAR.Communication
{
   public class Packet
   {
      public Packet(byte code, byte length)
      {
         this.code = code;
         this.length = length;
         data = new byte[length];
      }

      public Packet(byte[] frame)
      {
         if (frame.Length < 2)
         {
            throw new PacketFormatException("");
         }
         if (frame.Length != frame[1] + 2)
         {
            throw new PacketFormatException("");
         }

         code = frame[0];
         length = frame[1];
         data = new byte[length];

         for (int i = 0; i < length; ++i)
            data[i] = frame[i + 2];
      }

      public Packet CreateResponse(byte length)
      {
         return new Packet(code, length);
      }

      public byte Code
      {
         get
         {
            return code;
         }
      }

      public bool IsFunction
      {
         get
         {
            return code < 128;
         }
      }

      public byte Length
      {
         get
         {
            return length;
         }
      }

      public bool Empty
      {
         get
         {
            return length == 0;
         }
      }

      public bool ReverseEndianity { get; set; }

      public byte[] ToArray()
      {
         byte[] retValue = new byte[length + 3];
         retValue[0] = code;
         retValue[1] = length;

         for (int i = 0; i < length; ++i)
            retValue[i + 2] = data[i];

         return retValue;
      }

      public void InsertByte(int position, byte value)
      {
         VerifyPosition(position, 1);
         data[position] = value;
      }

      public void InsertUInt16(int position, UInt16 value)
      {
         Serialize(position, BitConverter.GetBytes(value));
      }

      public void InsertInt16(int position, Int16 value)
      {
         Serialize(position, BitConverter.GetBytes(value));
      }

      public void InsertUInt32(int position, UInt32 value)
      {
         Serialize(position, BitConverter.GetBytes(value));
      }

      public void InsertInt32(int position, Int32 value)
      {
         Serialize(position, BitConverter.GetBytes(value));
      }

      public void InsertString(int position, String value)
      {
         System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
         var bytes = encoding.GetBytes(value);
         VerifyPosition(position, bytes.Length);

         for (int i = 0; i < bytes.Length; ++i)
            data[position + i] = bytes[i];
      }

      public byte GetByte(int position)
      {
         VerifyPosition(position, 1);
         return data[position];
      }

      public UInt16 GetUInt16(int position)
      {
         return BitConverter.ToUInt16(Deserialize(position, 2), 0);
      }

      public Int16 GetInt16(int position)
      {
         return BitConverter.ToInt16(Deserialize(position, 2), 0);
      }

      public UInt32 GetUInt32(int position)
      {
         return BitConverter.ToUInt32(Deserialize(position, 4), 0);
      }

      public Int32 GetInt32(int position)
      {
         return BitConverter.ToInt32(Deserialize(position, 4), 0);
      }

      public String GetString(int position, int size)
      {
         VerifyPosition(position, size);
         var bytes = new byte[size];

         for (int i = 0; i < size; ++i)
         {
            bytes[i] = data[position + i];

            if (bytes[i] == 0)
               bytes[i] = 32;
         }

         return System.Text.Encoding.ASCII.GetString(bytes);
      }

      private void Serialize(int position, byte[] bytes)
      {
         VerifyPosition(position, bytes.Length);

         if (ReverseEndianity)
            Array.Reverse(bytes);

         for (int i = 0; i < bytes.Length; ++i)
            data[position + i] = bytes[i];
      }

      private byte[] Deserialize(int position, int size)
      {
         VerifyPosition(position, size);
         var retValue = new byte[size];

         for (int i = 0; i < size; ++i)
            retValue[i] = data[position + i];

         if (ReverseEndianity)
            Array.Reverse(retValue);

         return retValue;
      }

      private void VerifyPosition(int position, int size)
      {
         if (!ValidPosition(position, size))
            throw new ArgumentOutOfRangeException();
      }

      private bool ValidPosition(int position, int size)
      {
         return (position < length) &&
                (position + size <= length);
      }

      private readonly byte code;
      private readonly byte length;
      private readonly byte[] data;
   }
}

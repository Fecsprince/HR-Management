using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Drawing;
using System.IO;

namespace HRManagement.Utility
{
    public class Utilities
    {
      

      
        public Image GetBinaryImage(Binary binaryData)
        {
            if (binaryData == null) return null;

            byte[] buffer = binaryData.ToArray();
            MemoryStream memStream = new MemoryStream();
            memStream.Write(buffer, 0, buffer.Length);
            return Image.FromStream(memStream);
        }


    }
}
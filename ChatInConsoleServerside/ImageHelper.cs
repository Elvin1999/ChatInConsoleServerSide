using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatInConsoleServerside
{
   public class ImageHelper
    {

        public string GetImagePath(byte[] buffer, int counter)
        {
            string path = Directory.GetCurrentDirectory();
            ImageConverter ic = new ImageConverter();
            try
            {

                Image img = ic.ConvertFrom(buffer) as Image;
                Bitmap bitmap1 = new Bitmap(img);
                bitmap1.Save($@"{path}\image{counter}.png");
                var imagepath = $@"{path}\image{counter}.png";
                return imagepath;
            }
            catch (Exception)
            {
                return "";
            }
        }
        public byte[] GetBytesOfImage(string path)
        {
            var image = new Bitmap(path);
            ImageConverter imageconverter = new ImageConverter();
            var imagebytes = ((byte[])imageconverter.ConvertTo(image, typeof(byte[])));
            return imagebytes;
        }
    }
}

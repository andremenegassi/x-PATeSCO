using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace CrossPlatformCompatibility.Support
{
    public static class ImageUtils
    {

        public static Image ScaleByPercent(Image img, int Percent)
        {

            float nPercent = ((float)Percent / 100);

            int sourceWidth = img.Width;     //store original width of source image.
            int sourceHeight = img.Height;   //store original height of source image.
            int sourceX = 0;        //x-axis of source image.
            int sourceY = 0;        //y-axis of source image.

            int destX = 0;          //x-axis of destination image.
            int destY = 0;          //y-axis of destination image.
            //Calcuate height and width of resized image.
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            //Create a new bitmap object.
            Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
            //Set resolution of bitmap.
            bmPhoto.SetResolution(img.HorizontalResolution,
                                    img.VerticalResolution);
            //Create a graphics object and set quality of graphics.
            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //Draw image by using DrawImage() method of graphics class.
            grPhoto.DrawImage(img, new Rectangle(destX, destY, destWidth, destHeight), new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);

            grPhoto.Dispose();  //Dispose graphics object.
            return bmPhoto;
        }


        public static Image Scale(Image img, int width, int height)
        {

            int sourceWidth = img.Width;     //store original width of source image.
            int sourceHeight = img.Height;   //store original height of source image.
            int sourceX = 0;        //x-axis of source image.
            int sourceY = 0;        //y-axis of source image.

            int destX = 0;          //x-axis of destination image.
            int destY = 0;          //y-axis of destination image.
            //Calcuate height and width of resized image.
            int destWidth = width;
            int destHeight = height;

            //Create a new bitmap object.
            Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
            //Set resolution of bitmap.
            bmPhoto.SetResolution(img.HorizontalResolution, img.VerticalResolution);
            //Create a graphics object and set quality of graphics.
            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //Draw image by using DrawImage() method of graphics class.
            grPhoto.DrawImage(img, new Rectangle(destX, destY, destWidth, destHeight), new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight), GraphicsUnit.Pixel);

            grPhoto.Dispose();  //Dispose graphics object.
            return bmPhoto;
        }


        public static Image ScaleProportionally(Image img, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / img.Width;
            var ratioY = (double)maxHeight / img.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(img.Width * ratio);
            var newHeight = (int)(img.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(img, 0, 0, newWidth, newHeight);

            return newImage;
        }


        public static double Diff(Image img1, Image img2)
        {
            Bitmap bitmap1 = new Bitmap(img1);
            Bitmap bitmap2 = new Bitmap(img2);


            float diff = 0;

            for (int y = 0; y < bitmap1.Height; y++)
            {
                for (int x = 0; x < bitmap1.Width; x++)
                {
                    diff += (float)Math.Abs(bitmap1.GetPixel(x, y).R - bitmap2.GetPixel(x, y).R) / 255;
                    diff += (float)Math.Abs(bitmap1.GetPixel(x, y).G - bitmap2.GetPixel(x, y).G) / 255;
                    diff += (float)Math.Abs(bitmap1.GetPixel(x, y).B - bitmap2.GetPixel(x, y).B) / 255;
                }
            }

           return 100 * diff / (bitmap1.Width * bitmap1.Height * 3);

        }

        public static double Similarity(Image img1, Image img2)
        {
            double diff = Diff(img1, img2);

            if (diff == 0)
                return 100;
            else return 100 - diff; 
        }


        public static string ExtractOCR(Image img)
        {
            string dirTemp = System.Windows.Forms.Application.StartupPath + @"\ocr";
            string fileName = dirTemp + @"/" + Guid.NewGuid() + ".png";
            string text = "";

            if (Directory.Exists(dirTemp))
                Directory.CreateDirectory(dirTemp);

            img.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);

            var dataPath = System.Windows.Forms.Application.StartupPath + @"\Resources\tessdata";

            using (var tEngine = new TesseractEngine(dataPath, "eng", EngineMode.Default)) //creating the tesseract OCR engine with English as the language
            {
                using (var imgPix = Pix.LoadFromFile(fileName)) // Load of the image file from the Pix object which is a wrapper for Leptonica PIX structure
                {
                    using (var page = tEngine.Process(imgPix)) //process the specified image
                    {
                        text = page.GetText(); //Gets the image's content as plain text.
                    }
                }
            }

            return text;
        }

    }


}

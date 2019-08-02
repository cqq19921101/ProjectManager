using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Lib_SQM_Domain.SharedLibs
{
    public class ImageHelper
    {
        public enum ImageType
        {
            BMP,
            EMF,
            EXIF,
            GIF,
            ICON,
            JPEG,
            MEMORYBMP,
            PNG,
            TIFF,
            WMF
        }

        public ImageFormat GetImageType(string sImageType)
        {
            ImageFormat imaFormat = null;
            switch (sImageType)
            {
                case "BMP":
                    imaFormat = ImageFormat.Bmp;
                    break;
                case "EMF":
                    imaFormat = ImageFormat.Emf;
                    break;
                case "EXIF":
                    imaFormat = ImageFormat.Exif;
                    break;
                case "GIF":
                    imaFormat = ImageFormat.Gif;
                    break;
                case "ICON":
                    imaFormat = ImageFormat.Icon;
                    break;
                case "JPEG":
                    imaFormat = ImageFormat.Jpeg;
                    break;
                case "MEMORYBMP":
                    imaFormat = ImageFormat.MemoryBmp;
                    break;
                case "PNG":
                    imaFormat = ImageFormat.Png;
                    break;
                case "TIFF":
                    imaFormat = ImageFormat.Tiff;
                    break;
                case "WMF":
                    imaFormat = ImageFormat.Wmf;
                    break;
            }
            return imaFormat;
        }

        public ImageFormat GetImageType(Image imaOrigin)
        {
            ImageFormat imaFormat = null;
            if (ImageFormat.Bmp.Equals(imaOrigin.RawFormat))
                imaFormat = ImageFormat.Bmp;
            else if (ImageFormat.Emf.Equals(imaOrigin.RawFormat))
                imaFormat = ImageFormat.Emf;
            else if (ImageFormat.Exif.Equals(imaOrigin.RawFormat))
                imaFormat = ImageFormat.Exif;
            else if (ImageFormat.Gif.Equals(imaOrigin.RawFormat))
                imaFormat = ImageFormat.Gif;
            else if (ImageFormat.Icon.Equals(imaOrigin.RawFormat))
                imaFormat = ImageFormat.Icon;
            else if (ImageFormat.Jpeg.Equals(imaOrigin.RawFormat))
                imaFormat = ImageFormat.Jpeg;
            else if (ImageFormat.MemoryBmp.Equals(imaOrigin.RawFormat))
                imaFormat = ImageFormat.MemoryBmp;
            else if (ImageFormat.Png.Equals(imaOrigin.RawFormat))
                imaFormat = ImageFormat.Png;
            else if (ImageFormat.Tiff.Equals(imaOrigin.RawFormat))
                imaFormat = ImageFormat.Tiff;
            else if (ImageFormat.Wmf.Equals(imaOrigin.RawFormat))
                imaFormat = ImageFormat.Wmf;
            return imaFormat;
        }

        public string GetImageTypeOfString(string InsertImgPath)
        {
            Image OriginalImg = Image.FromFile(InsertImgPath);

            string sImageType = string.Empty;
            if (ImageFormat.Bmp.Equals(OriginalImg.RawFormat))
                sImageType = "BMP";
            else if (ImageFormat.Emf.Equals(OriginalImg.RawFormat))
                sImageType = "EMF";
            else if (ImageFormat.Exif.Equals(OriginalImg.RawFormat))
                sImageType = "EXIF";
            else if (ImageFormat.Gif.Equals(OriginalImg.RawFormat))
                sImageType = "GIF";
            else if (ImageFormat.Icon.Equals(OriginalImg.RawFormat))
                sImageType = "ICON";
            else if (ImageFormat.Jpeg.Equals(OriginalImg.RawFormat))
                sImageType = "JPEG";
            else if (ImageFormat.MemoryBmp.Equals(OriginalImg.RawFormat))
                sImageType = "MEMORYBMP";
            else if (ImageFormat.Png.Equals(OriginalImg.RawFormat))
                sImageType = "PNG";
            else if (ImageFormat.Tiff.Equals(OriginalImg.RawFormat))
                sImageType = "TIFF";
            else if (ImageFormat.Wmf.Equals(OriginalImg.RawFormat))
                sImageType = "WMF";

            //Dispose
            if (OriginalImg != null) OriginalImg.Dispose();

            return sImageType;
        }

        public string GetImageTypeOfString(Image imaOrigin)
        {
            string sImageType = string.Empty;
            if (ImageFormat.Bmp.Equals(imaOrigin.RawFormat))
                sImageType = "BMP";
            else if (ImageFormat.Emf.Equals(imaOrigin.RawFormat))
                sImageType = "EMF";
            else if (ImageFormat.Exif.Equals(imaOrigin.RawFormat))
                sImageType = "EXIF";
            else if (ImageFormat.Gif.Equals(imaOrigin.RawFormat))
                sImageType = "GIF";
            else if (ImageFormat.Icon.Equals(imaOrigin.RawFormat))
                sImageType = "ICON";
            else if (ImageFormat.Jpeg.Equals(imaOrigin.RawFormat))
                sImageType = "JPEG";
            else if (ImageFormat.MemoryBmp.Equals(imaOrigin.RawFormat))
                sImageType = "MEMORYBMP";
            else if (ImageFormat.Png.Equals(imaOrigin.RawFormat))
                sImageType = "PNG";
            else if (ImageFormat.Tiff.Equals(imaOrigin.RawFormat))
                sImageType = "TIFF";
            else if (ImageFormat.Wmf.Equals(imaOrigin.RawFormat))
                sImageType = "WMF";
            return sImageType;
        }

        public void Watermaker(string OriginalImgPath, string WatermarkImgPath)
        {
            //Load Original Image and convert to Bitmap
            Image OriginalImg = Image.FromFile(OriginalImgPath);
            Image TempImage = new Bitmap(OriginalImg.Width, OriginalImg.Height);
            TempImage = new Bitmap(OriginalImg);
            //Set font and location of watermark
            string watermarkFont = "Lite-On confidential.";
            int watermarkFontSize = ((TempImage.Width * 6) / (watermarkFont.Length * 4));
            int watermarkWidth = 0;
            int watermarkHeight = 0;

            //Set font style
            StringFormat DrawFormat = new StringFormat();
            DrawFormat.Alignment = StringAlignment.Center;
            DrawFormat.FormatFlags = StringFormatFlags.NoWrap;

            //Draw font on originalimg
            Graphics watermarkGraphics = Graphics.FromImage(TempImage);

            for (int i = 1; i <= 10; i = i + 2)
            {
                watermarkWidth = TempImage.Width * i / 10;
                watermarkHeight = TempImage.Height * i / 10;
                watermarkGraphics.DrawString(watermarkFont, new Font("Arial", watermarkFontSize, FontStyle.Bold), new SolidBrush(Color.FromArgb(100, 215, 215, 193)), watermarkWidth, watermarkHeight, DrawFormat);
            }

            //Save the new watermarkImage
            TempImage.Save(WatermarkImgPath);

            //Dispose
            if (TempImage != null) TempImage.Dispose();
            if (OriginalImg != null) OriginalImg.Dispose();
            if (watermarkGraphics != null) watermarkGraphics.Dispose();

            //Delete Originl Image file
            if (System.IO.File.Exists(OriginalImgPath))
                System.IO.File.Delete(OriginalImgPath);

            // Move the file.
            System.IO.File.Move(WatermarkImgPath, OriginalImgPath);
        }

        /// 將 Byte 陣列轉換為 Image。
        /// </summary>
        /// <param name="Buffer">Byte 陣列。</param>        
        public Image BufferToImage(byte[] Buffer)
        {
            if (Buffer == null || Buffer.Length == 0) { return null; }
            byte[] data = null;
            Image oImage = null;
            Bitmap oBitmap = null;
            //建立副本
            data = (byte[])Buffer.Clone();
            try
            {
                MemoryStream oMemoryStream = new MemoryStream(Buffer);
                //設定資料流位置
                oMemoryStream.Position = 0;
                oImage = System.Drawing.Image.FromStream(oMemoryStream);
                //建立副本
                oBitmap = new Bitmap(oImage);
            }
            catch (Exception ex)
            {
                throw;
            }
            //return oImage;
            return oBitmap;
        }

        /// 將 Image 轉換為 Byte 陣列。
        /// </summary>
        /// <param name="Image">Image 。</param>
        /// <param name="imageFormat">指定影像格式。</param>        
        public byte[] ImageToBuffer(Image Image, System.Drawing.Imaging.ImageFormat imageFormat)
        {
            if (Image == null) { return null; }
            byte[] data = null;
            using (MemoryStream oMemoryStream = new MemoryStream())
            {
                //建立副本
                using (Bitmap oBitmap = new Bitmap(Image))
                {
                    //儲存圖片到 MemoryStream 物件，並且指定儲存影像之格式
                    oBitmap.Save(oMemoryStream, imageFormat);
                    //設定資料流位置
                    oMemoryStream.Position = 0;
                    //設定 buffer 長度
                    data = new byte[oMemoryStream.Length];
                    //將資料寫入 buffer
                    oMemoryStream.Read(data, 0, Convert.ToInt32(oMemoryStream.Length));
                    //將所有緩衝區的資料寫入資料流
                    oMemoryStream.Flush();
                }
            }
            return data;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_1
{
    public static class TgaLoader
    {
        public unsafe static Bitmap LoadTga(string fileName)
        {
            using (var f = File.OpenRead(fileName))
            using (var reader = new BinaryReader(f))
            {
                // Читаем заголовок TGA (18 байт)
                f.Seek(12, SeekOrigin.Begin);
                short width = reader.ReadInt16();
                short height = reader.ReadInt16();
                byte bitDepth = reader.ReadByte();
                f.Seek(1, SeekOrigin.Current); // Пропускаем дескриптор

                if (bitDepth != 24 && bitDepth != 32)
                    throw new Exception("Поддерживаются только 24 и 32 битные TGA");

                Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
                BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                byte[] bytes = reader.ReadBytes(width * height * (bitDepth / 8));
                byte* ptr = (byte*)data.Scan0;

                for (int i = 0; i < width * height; i++)
                {
                    int srcIdx = i * (bitDepth / 8);
                    int dstIdx = i * 4;
                    ptr[dstIdx] = bytes[srcIdx];         // B
                    ptr[dstIdx + 1] = bytes[srcIdx + 1]; // G
                    ptr[dstIdx + 2] = bytes[srcIdx + 2]; // R
                    ptr[dstIdx + 3] = (bitDepth == 32) ? bytes[srcIdx + 3] : (byte)255; // A
                }

                bmp.UnlockBits(data);
                bmp.RotateFlip(RotateFlipType.RotateNoneFlipY); // TGA часто перевернуты
                return bmp;
            }
        }
    }
}

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using dicom_viewer_winform.Entities;

namespace dicom_viewer_winform
{
    /// <summary>
    /// Utility class that generates simple MPR bitmaps from an ImageSet.
    /// </summary>
    public static class MprRenderer
    {
        private static byte Window(double value, double level, double width)
        {
            double min = level - width / 2.0;
            double max = level + width / 2.0;
            if (value <= min) return 0;
            if (value >= max) return 255;
            return (byte)(255 * ((value - min) / width));
        }

        private static byte[] GetWindowedPixels(ImageData image, double level, double width)
        {
            int length = image.Width * image.Height;
            byte[] result = new byte[length];

            if (image.BytesPerPixel == 1)
            {
                byte[] buffer = new byte[length];
                Marshal.Copy(image.Pixels, buffer, 0, length);
                for (int i = 0; i < length; i++)
                {
                    double val = buffer[i] * image.Slope + image.Intercept;
                    result[i] = Window(val, level, width);
                }
            }
            else
            {
                short[] buffer = new short[length];
                Marshal.Copy(image.Pixels, buffer, 0, length);
                for (int i = 0; i < length; i++)
                {
                    double raw = image.IsSigned ? buffer[i] : (ushort)buffer[i];
                    double val = raw * image.Slope + image.Intercept;
                    result[i] = Window(val, level, width);
                }
            }
            return result;
        }

        private static Bitmap CreateBitmap(byte[] pixels, int width, int height)
        {
            var bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            var data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            int stride = data.Stride;
            unsafe
            {
                byte* ptr = (byte*)data.Scan0;
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        byte val = pixels[y * width + x];
                        ptr[y * stride + x * 3] = val;
                        ptr[y * stride + x * 3 + 1] = val;
                        ptr[y * stride + x * 3 + 2] = val;
                    }
                }
            }
            bmp.UnlockBits(data);
            return bmp;
        }

        private static Bitmap ResizeBitmap(Bitmap bitmap, int width, int height)
        {
            if (width == bitmap.Width && height == bitmap.Height)
            {
                return bitmap;
            }
            var resized = new Bitmap(width, height);
            using (var g = Graphics.FromImage(resized))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(bitmap, 0, 0, width, height);
            }
            bitmap.Dispose();
            return resized;
        }

        public static Bitmap GenerateAxial(ImageSet volume)
        {
            if (volume.Slices.Count == 0) return new Bitmap(1,1);
            int index = volume.Slices.Count / 2;
            var slice = volume.Slices[index];
            double level = slice.WindowLevel;
            double width = slice.WindowWidth > 0 ? slice.WindowWidth : slice.MaxRescaledValue - slice.MinRescaledValue;
            var pixels = GetWindowedPixels(slice, level, width);
            return CreateBitmap(pixels, slice.Width, slice.Height);
        }

        public static Bitmap GenerateSagittal(ImageSet volume)
        {
            if (volume.Slices.Count == 0) return new Bitmap(1,1);
            int xIndex = volume.Dimensions.X / 2;
            var centerSlice = volume.Slices[volume.Slices.Count/2];
            double level = centerSlice.WindowLevel;
            double width = centerSlice.WindowWidth > 0 ? centerSlice.WindowWidth : centerSlice.MaxRescaledValue - centerSlice.MinRescaledValue;

            int widthOut = volume.Dimensions.Z;
            int heightOut = volume.Dimensions.Y;
            byte[] result = new byte[widthOut * heightOut];

            for (int z = 0; z < volume.Dimensions.Z; z++)
            {
                var slice = volume.Slices[z];
                var slicePixels = GetWindowedPixels(slice, level, width);
                for (int y = 0; y < volume.Dimensions.Y; y++)
                {
                    result[y * widthOut + z] = slicePixels[y * volume.Dimensions.X + xIndex];
                }
            }
            var bmp = CreateBitmap(result, widthOut, heightOut);
            var scale = volume.VoxelSpacing.Z / volume.VoxelSpacing.Y;
            var newWidth = (int)Math.Round(widthOut * scale);
            if (newWidth < 1) newWidth = 1;
            return ResizeBitmap(bmp, newWidth, heightOut);
        }

        public static Bitmap GenerateCoronal(ImageSet volume)
        {
            if (volume.Slices.Count == 0) return new Bitmap(1,1);
            int yIndex = volume.Dimensions.Y / 2;
            var centerSlice = volume.Slices[volume.Slices.Count/2];
            double level = centerSlice.WindowLevel;
            double width = centerSlice.WindowWidth > 0 ? centerSlice.WindowWidth : centerSlice.MaxRescaledValue - centerSlice.MinRescaledValue;

            int widthOut = volume.Dimensions.X;
            int heightOut = volume.Dimensions.Z;
            byte[] result = new byte[widthOut * heightOut];

            for (int z = 0; z < volume.Dimensions.Z; z++)
            {
                var slice = volume.Slices[z];
                var slicePixels = GetWindowedPixels(slice, level, width);
                for (int x = 0; x < volume.Dimensions.X; x++)
                {
                    result[z * widthOut + x] = slicePixels[yIndex * volume.Dimensions.X + x];
                }
            }
            var bmp = CreateBitmap(result, widthOut, heightOut);
            var scale = volume.VoxelSpacing.Z / volume.VoxelSpacing.X;
            var newHeight = (int)Math.Round(heightOut * scale);
            if (newHeight < 1) newHeight = 1;
            return ResizeBitmap(bmp, widthOut, newHeight);
        }
    }
}

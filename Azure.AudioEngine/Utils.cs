namespace Azure.MediaUtils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    public static class Utils
    {
        private static bool Prepared;
        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        public static void Prepare()
        {
            if (!Utils.Prepared)
            {
                Un4seen.Bass.BassNet.Registration("zollai@outlook.com", "2X3924291824822");
                Un4seen.Bass.BassNet.OmitCheckVersion = true;
                Utils.Prepared = true;
            }
        }

        public static string ProcessorArchitecture
        {
            get
            {
                Prepare();
                switch (Un4seen.Bass.Utils.Is64Bit)
                {
                    case true: return "x64";
                    case false: return "x86";
                    default: return "unknown"; // that's weird :-)
                }
            }
        }

        public static Dictionary<Int32, String> QueryOutputDevices(out int defaultDeviceID)
        {
            Prepare();
            defaultDeviceID = -1;
            Dictionary<Int32, String> outputDevices = new Dictionary<int, string>();
            Un4seen.Bass.BASS_DEVICEINFO[] devices = Un4seen.Bass.Bass.BASS_GetDeviceInfos();
            outputDevices.Add(-1, "<system default>");
            for (int n = 0; n < devices.Length; n++)
            {
                if (devices[n].IsEnabled)
                {
                    outputDevices.Add(n, devices[n].name);
                }

                if (devices[n].IsDefault)
                {
                    defaultDeviceID = n;
                }
            }
            return outputDevices;
        }

        public static IEnumerable<String> GetSupportedFilesPath(String directoryPath, String fileExtensions, Boolean allDirectories = false)
        {
            return fileExtensions.Split(';')
                .SelectMany(f => Directory.EnumerateFiles(directoryPath, f, (allDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly)))
                .OrderBy(x => x);
        }

		public static IEnumerable<String> FilterFiles(IEnumerable<String> files, String fileExtensions)
		{
			List<FileInfo> fileInfos = files.Where(f => File.Exists(f)).Select(f => new FileInfo(f)).ToList();
			var extensions = fileExtensions.Split(';').Select(x => x.Replace("*", String.Empty));
			return fileInfos.Where(fi => extensions.Contains(fi.Extension)).Select(fi => fi.FullName);
		}

        private static byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        private static System.Windows.Media.Imaging.BitmapImage BitmapImageFromBuffer(Byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            System.Windows.Media.Imaging.BitmapImage image = new System.Windows.Media.Imaging.BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        public static System.Windows.Media.Imaging.BitmapImage BitmapImageFromImage(System.Drawing.Image image)
        {
            return Utils.BitmapImageFromBuffer(Utils.ImageToByteArray(image));
        }
    }

}

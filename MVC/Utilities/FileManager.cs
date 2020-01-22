using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace MVC.Utilities
{
    public static class FileManager
    {

        private static string _blobStoragePath = ConfigurationManager.AppSettings["BlobStoragePath"];
        public static byte[] ReadFile(Guid blobId)
        {
            var filename = blobId.ToString("N");

            var filePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, _blobStoragePath, filename);
            if (new FileInfo(filePath) is FileInfo fi && fi.Exists)
                return File.ReadAllBytes(fi.FullName);
            throw new FileNotFoundException($"{filePath} not found");

        }

        public static byte[] ToByteArray(this Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

    }
}
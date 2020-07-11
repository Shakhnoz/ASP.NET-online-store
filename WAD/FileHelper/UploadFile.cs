using System.IO;
using System.Web;

namespace WAD.FileHelper
{
    public class UploadFile
    {

        public static bool UploadPhoto(HttpPostedFileBase file, string folder, string name)
        {
            if (file == null || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(folder))
            {
                return false;
            }
            try
            {
                string path = string.Empty;

                if (file != null)
                {
                    path = Path.Combine(HttpContext.Current.Server.MapPath(folder), name);//resolves website virtual path to physical path 
                    file.SaveAs(path);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        file.InputStream.CopyTo(ms);//file content is streamed into memory
                        byte[] array = ms.GetBuffer();

                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
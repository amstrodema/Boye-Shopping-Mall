using SixLabors.ImageSharp.Formats.Png;

namespace Boye.Services
{
    public class ImageService
    {  
        private static void ResizeImage(byte[] bytes, int width, int height, string imageID, string folderName)
        {

            // Load the image from the byte array using ImageSharp
            using (Image image = Image.Load(bytes))
            {
                // Resize the image while retaining its aspect ratio and quality
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(width, height),
                    Mode = ResizeMode.Max,
                    Compand = false,
                    Sampler = KnownResamplers.Lanczos3,
                }));


                var roota = AppDomain.CurrentDomain;
                var root = roota.BaseDirectory;
                string folder = Path.Combine(root, "Images");
                folder = Path.Combine(folder, "Small");
                folder = Path.Combine(folder, folderName);
                string uniqueFileName = imageID + ".png";
                string filePath = Path.Combine(folder, uniqueFileName);

                if (!(Directory.Exists(folder)))
                {
                    Directory.CreateDirectory(folder);
                }

                // Convert the image back to a base64 string
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, new PngEncoder());
                    byte[] imageBytes = ms.ToArray();

                    try
                    {

                        MemoryStream ms2 = new MemoryStream(imageBytes);

                        try
                        {
                            using (Stream stream = new FileStream(filePath, FileMode.Create))
                            {
                                ms2.CopyTo(stream);
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }
            }
        }

        public static string SaveImageInFolder(byte[] data, string imageID, string folderName)
        {
            var roota = AppDomain.CurrentDomain;
            var root = roota.BaseDirectory;
            string folder = Path.Combine(root, "Images");
            folder = Path.Combine(folder, "Large");
            folder = Path.Combine(folder, folderName);
            string uniqueFileName = imageID + ".png";
            string filePath = Path.Combine(folder, uniqueFileName);

            if (!(Directory.Exists(folder)))
            {
                Directory.CreateDirectory(folder);
            }

            try
            {
                ResizeImage(data, 250, 250, imageID, folderName);
            }
            catch (Exception)
            {
                throw;
            }

            MemoryStream ms = new MemoryStream(data);

            try
            {
                using (Stream stream = new FileStream(filePath, FileMode.Create))
                {
                    ms.CopyTo(stream);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return uniqueFileName;
        }
     


        public static string GetImageFromFolder(string uniqueFileName, string folderName)
        {

            try
            {

                var roota = AppDomain.CurrentDomain;
                var root = roota.BaseDirectory;
                string folder = Path.Combine(root, "Images");
                folder = Path.Combine(folder, "Large");
                folder = Path.Combine(folder, folderName);

                string filePath = Path.Combine(folder, uniqueFileName);

                byte[] imageArray = File.ReadAllBytes(filePath);
                string val = "data:image/png;base64," + Convert.ToBase64String(imageArray);

                return val;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetSmallImageFromFolder(string uniqueFileName, string folderName)
        {


            try
            {
                var roota = AppDomain.CurrentDomain;
                var root = roota.BaseDirectory;
                string folder = Path.Combine(root, "Images");
                folder = Path.Combine(folder, "Small");
                folder = Path.Combine(folder, folderName);

                string filePath = Path.Combine(folder, uniqueFileName);

                byte[] imageArray = File.ReadAllBytes(filePath);
                string val = "data:image/png;base64," + Convert.ToBase64String(imageArray);

                return val;
            }
            catch (Exception)
            {
                return "";
            }
        }



        public static async Task<byte[]> GetByte(IFormFile formFile)
        {
            using (var stream = new MemoryStream())
            {
                await formFile.CopyToAsync(stream);

                //if (stream.Length < 2097152)
                //{
                //}

                return stream.ToArray();
            }

        }

    }
}

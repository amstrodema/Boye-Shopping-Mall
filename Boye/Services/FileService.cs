namespace Boye.Services
{
    public class FileService
    {


        public static void WriteToFile(string data, string fileName)
        {
            try
            {
                var roota = AppDomain.CurrentDomain;
                var root = roota.BaseDirectory;
                string folder = Path.Combine(root, "Files");
                string uniqueFileName = fileName + ".txt";
                string filePath = Path.Combine(folder, uniqueFileName);

                List<string> readData = new List<string>();
                if (!(Directory.Exists(folder)))
                {
                    Directory.CreateDirectory(folder);
                }
                else
                {
                    try
                    {
                        readData = File.ReadAllLines(filePath).ToList();
                    }
                    catch (Exception)
                    {
                    }
                }

                int count = 0;

                // Create or overwrite the text file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (string item in readData)
                    {
                        // Write each item to a new line in the file
                        if (count > 0)
                        {
                            writer.Write(",");
                        }
                        writer.Write(item);
                        count++;
                    }

                    if (count > 0)
                    {
                        writer.Write(",");
                    }
                    writer.Write(data);
                }
            }
            catch (IOException e)
            {
                throw;
            }
        }
  public static void UpdateFile(string data, string fileName)
        {
            try
            {
                var roota = AppDomain.CurrentDomain;
                var root = roota.BaseDirectory;
                string folder = Path.Combine(root, "Files");
                string uniqueFileName = fileName + ".txt";
                string filePath = Path.Combine(folder, uniqueFileName);

                // Create or overwrite the text file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(data.Substring(1, data.Length-2));
                }
            }
            catch (IOException e)
            {
                throw;
            }
        }

        public static string ReadFromFile(string fileName)
        {
            try
            {
                var roota = AppDomain.CurrentDomain;
                var root = roota.BaseDirectory;
                string folder = Path.Combine(root, "Files");
                string uniqueFileName = fileName + ".txt";
                string filePath = Path.Combine(folder, uniqueFileName);

                // Read all lines from the text file into an array
                var readData = File.ReadAllLines(filePath);

                return "[" + readData[0] + "]";
            }
            catch (Exception e)
            {
                return "";
            }


        }


    }

}

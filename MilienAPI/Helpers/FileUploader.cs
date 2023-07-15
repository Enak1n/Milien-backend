using Renci.SshNet;

namespace MilienAPI.Helpers
{
    public static class FileUploader
    {
        private static IConfiguration _configuration;

        public static void FileUploaderConfigure(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static List<string> UploadImageToServer(IFormFileCollection? files, string remoteFolderPath)
        {
            string host = "37.140.199.105";
            int port = 22;
            string username = "root";
            string password = "f4%ibio_LbnY";
            List<string> imagePaths = new List<string>();

            var pathToServer = _configuration.GetSection("Endpoints:Http:Url").Value;
            using (var client = new SftpClient(host, port, username, password))
            {
                client.Connect();

                string uniqueFileName = null;
                foreach (var file in files)
                {
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    using (var stream = file.OpenReadStream())
                    {
                        string remoteFilePath = $"{remoteFolderPath}/{uniqueFileName}";
                        client.UploadFile(stream, remoteFilePath);

                        string imagePathOnServer = Path.Combine(remoteFolderPath, uniqueFileName);
                        imagePaths.Add($"{pathToServer}/images/{uniqueFileName}");

                    }
                }
                client.Disconnect();
            }

            return imagePaths;
        }

        public static string UploadImageToServer(IFormFile file, string remoteFolderPath)
        {
            string host = "37.140.199.105";
            int port = 22;
            string username = "root";
            string password = "f4%ibio_LbnY";
            string imagePath;

            var pathToServer = _configuration.GetSection("Endpoints:Http:Url").Value;
            using (var client = new SftpClient(host, port, username, password))
            {
                client.Connect();

                string uniqueFileName = null;
                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                using (var stream = file.OpenReadStream())
                {
                    string remoteFilePath = $"{remoteFolderPath}/{uniqueFileName}";
                    client.UploadFile(stream, remoteFilePath);

                    string imagePathOnServer = Path.Combine(remoteFolderPath, uniqueFileName);
                    imagePath = $"{pathToServer}/avatars/{uniqueFileName}";

                }

                client.Disconnect();
            }

            return imagePath;
        }

        public static void DeleteFileFromServer(string filePath)
        {
            string host = "37.140.199.105";
            int port = 22;
            string username = "root";
            string password = "f4%ibio_LbnY";

            using (var client = new SftpClient(host, port, username, password))
            {
                client.Connect();
                if (client.Exists(filePath))
                    client.DeleteFile(filePath);

                client.Disconnect();
            }
        }
    }
}

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PushTechnology.ClientInterface.Client.Factories;
using PushTechnology.ClientInterface.Client.Topics;


namespace DiffusionTopicAdd
{
    public class TopicAddFromDir
    {
      
        static async Task Main(string[] args)
        {
            // Take input for file location and topic path
            Console.WriteLine("Enter source directory location : ");
            var OneDriveDirPath = Console.ReadLine();
            if (OneDriveDirPath == "")
            {
                Console.WriteLine("Please specify source directory path, it can't be null");
                return;
            }
            if (!Path.IsPathRooted(OneDriveDirPath))
            {
                Console.WriteLine("Please enter full path for source directory");
                return;
            }

            Console.WriteLine();

            Console.WriteLine("Enter Diffussion Topic path : ");
            var topicPath = Console.ReadLine();

            if (topicPath == "")
            {
                Console.WriteLine("Diffission topic path can't be null");
                return;
            }

            Console.WriteLine();

          /*  Console.WriteLine("Enter login information for Diffusion Console");
            Console.Write("Username : ");
            var username = Console.ReadLine();
            Console.Write("\nPassword : ");
            var pwd = ReadPassword();
            var host ="wss://d1-msa-angstrom.eu.diffusion.cloud/";*/


            // Crediatials from app.config
            var username = System.Configuration.ConfigurationManager.AppSettings["Principal"];
           var pwd = System.Configuration.ConfigurationManager.AppSettings["Password"];
           var host = System.Configuration.ConfigurationManager.AppSettings["Host"];
     
            
                var session = Diffusion.Sessions
                .Principal(username)
                .Password(pwd)
                .Open(host);

            Console.WriteLine("");
            Console.WriteLine("Connection Open to add topic");


            try
            {
                DirectoryInfo di = new DirectoryInfo(OneDriveDirPath);
                FileInfo[] files = di.GetFiles("*.json");            

                for (int i = 0; i < files.Length; i++)
                {
                    var fileToRead = (@"" + di + "/" + files[i].Name);
                    var fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileToRead);
                    string readText = File.ReadAllText(di + "/" + files[i].Name);

                    String jsonDataFromFile = System.IO.File.ReadAllText(fileToRead);

                    String newTopicToAdd = topicPath + "/" + Regex.Replace(fileNameWithoutExt, @"[\s_.]", "");

                    await session.TopicControl.AddTopicAsync(newTopicToAdd, TopicType.JSON);
                    var jsonDataType = Diffusion.DataTypes.JSON;

                    var value = jsonDataType.FromJSONString(readText);

                    //Update topic with value
                    var result = session.TopicUpdate.SetAsync(newTopicToAdd, value);
                }
                Console.WriteLine(files.Length + " topics added to diffusion");
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"The file was not found: '{e}'");
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine($"The directory was not found: '{e}'");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception Occured : '{e}'");
            }
            
            // Close the session
            session.Close();
           // Console.WriteLine(numOfTopics + " topics added successfully");
            Console.WriteLine("Connection Closed");
            Environment.Exit(0);
            Console.ReadKey();
        }

        public static string ReadPassword()
        {
            ConsoleKeyInfo keyInfo;
            string password = "";
            do
            {
                keyInfo = Console.ReadKey(true);
                // Skip if Backspace or Enter is Pressed
                if (keyInfo.Key != ConsoleKey.Backspace && keyInfo.Key != ConsoleKey.Enter)
                {
                    password += keyInfo.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (keyInfo.Key == ConsoleKey.Backspace && password.Length > 0)
                    {
                        // Remove last charcter if Backspace is Pressed
                        password = password.Substring(0, (password.Length - 1));
                        Console.Write("b b");
                    }
                }
            }
            // Stops Getting Password Once Enter is Pressed
            while (keyInfo.Key != ConsoleKey.Enter);
            return password;
        }
    }
}
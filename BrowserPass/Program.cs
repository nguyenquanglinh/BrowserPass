using System;
using System.Collections.Generic;
using System.IO;

namespace BrowserPass
{
    // Missing windows.security? https://software.intel.com/en-us/articles/using-winrt-apis-from-desktop-applications
    class Program
    {


        static void GetAllFolderInPath(string path, List<string> list)
        {
            DirectoryInfo dx = new DirectoryInfo(path);//Assuming Test is your Folder
            DirectoryInfo[] diArr = dx.GetDirectories();
            // Display the names of the directories.

            foreach (DirectoryInfo dri in diArr)
                try
                {
                    GetAllFolderInPath(path + "\\" + dri.Name, list);

                    list.Add(path + "\\" + dri.Name);
                }

                catch
                {
                    Console.WriteLine("không tìm thấy  " + path + "\\" + dri.Name);
                }

            if (diArr.Length == 0)
                return;
        }


        static List<string> GetAllFileInFolder(List<string> listFolder)
        {
            List<string> listFile = new List<string>();
            foreach (var path in listFolder)
            {
                try
                {
                    DirectoryInfo dx = new DirectoryInfo(@"C:\Users\thang\AppData\Local\Google\Chrome\User Data\Default");
                    FileInfo[] Files = dx.GetFiles("*"); //Getting Text files

                    foreach (FileInfo file in Files)
                    {
                        if (file.Extension == "" &&listFile.Contains(file.FullName)==false)
                            listFile.Add(file.FullName);
                    }

                }
                catch
                {

                }

            }
            return listFile;
        }

        static void Main(string[] args)
        {
            List<IPassReader> readers = new List<IPassReader>();
            DirectoryInfo dx = new DirectoryInfo(@"C:\Users\thang\AppData\Local");//Assuming Test is your Folder
            FileInfo[] Files = dx.GetFiles(); //Getting Text files
            string str = "";
            foreach (FileInfo file in Files)
            {
                str += " " + file.Name;
                Console.WriteLine(str);
            }
            //DirectoryInfo di = new DirectoryInfo("c:\\");

            // Get a reference to each directory in that directory.


            var path = @"C:\Users\thang\AppData\Local";
            var listFolder = new List<string>();
            GetAllFolderInPath(path, listFolder);
            var xx = GetAllFileInFolder(listFolder);
            var chrome = new ChromePassReader();
            foreach (var d in chrome.ReadPasswordsList(xx))
                Console.WriteLine($"{d.Url}\r\n\tU: {d.Username}\r\n\tP: {d.Password}\r\n");

            readers.Add(chrome);

            readers.Add(new FirefoxPassReader());
            readers.Add(new IE10PassReader());
            foreach (var reader in readers)
            {
                Console.WriteLine($"== {reader.BrowserName} ============================================ ");
                try
                {
                    foreach (var d in reader.ReadPasswords())
                        Console.WriteLine($"{d.Url}\r\n\tU: {d.Username}\r\n\tP: {d.Password}\r\n");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading {reader.BrowserName} passwords: " + ex.Message);
                }
            }
            Console.ReadKey();
            Console.ReadLine();
        }
    }
}

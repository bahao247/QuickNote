using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using Foundation;
using UIKit;
using SQLite.Net.Interop;
using Xamarin.Forms;

[assembly: Dependency(typeof(QuickNote1.iOS.Config))]

namespace QuickNote1.iOS
{
    class Config : IConfig
    {
        private string directoryDB;
        private ISQLitePlatform platform;
        public string DirectoryDB
        {
            get
            {
                if (string.IsNullOrEmpty(directoryDB))
                {
                    var directory = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    directoryDB = System.IO.Path.Combine(directory, "..", "Library");
                }

                return directoryDB;

            }
        }

        public ISQLitePlatform Platform
        {
            get
            {
                if (platform == null)
                {
                    platform = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
                }

                return platform;
            }
        }
    }
}

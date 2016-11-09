using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Interop;

namespace QuickNote1
{
    public interface IConfig
    {
        //Directory Database
        string DirectoryDB{ get; }
        ISQLitePlatform Platform { get; }
    }
}

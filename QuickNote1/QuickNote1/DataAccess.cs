using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using Xamarin.Forms;
using System.IO;

namespace QuickNote1
{
    class DataAccess : IDisposable
    {
        private SQLiteConnection connection;

        public DataAccess()
        {
            var config = DependencyService.Get<IConfig>();
            connection = new SQLiteConnection(config.Platform, Path.Combine(config.DirectoryDB, "QuickNote1.db3"));
            connection.CreateTable<QuickNote>();

        }

        public void InsertNote(QuickNote quicknote)
        {
            connection.Insert(quicknote);
        }

        public void UpdateNote(QuickNote quicknote)
        {
            connection.Update(quicknote);
        }

        public void DeleteNote(QuickNote quicknote)
        {
            connection.Delete(quicknote);
        }

        public QuickNote GetNote(int IDNote)
        {
            return connection.Table<QuickNote>().FirstOrDefault(connection => connection.IDNote == IDNote);
        }

        public List<QuickNote> GetNotes()
        {
            return connection.Table<QuickNote>().OrderBy(connection => connection.TypeNote).ToList();
        }

        public void Dispose()
        {
            connection.Dispose();
        }
    }
}

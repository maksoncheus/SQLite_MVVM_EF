using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
namespace SQLite_MVVM_EF
{
    internal class ApplicationViewModel
    {
        private ApplicationContext db = new();
        public ObservableCollection<User> Users { get; set; }
        public ApplicationViewModel()
        {
            db.Users.Load();
            Users = db.Users.Local.ToObservableCollection();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using SQLite_MVVM_EF.View;
using System.Collections.ObjectModel;
namespace SQLite_MVVM_EF
{
    internal class ApplicationViewModel
    {
        private readonly ApplicationContext db = new();
        CommonCommand<object?>? addCommand;
        CommonCommand<object?>? editCommand;
        CommonCommand<object?>? deleteCommand;
        public ObservableCollection<User> Users { get; set; }
        public ApplicationViewModel()
        {
            db.Users.Load();
            Users = db.Users.Local.ToObservableCollection();
        }
        public CommonCommand<object?> AddCommand
        {
            get
            {
                return addCommand ??= new CommonCommand<object?>
                (
                    (o) =>
                    {
                        UserWindow userWindow = new (new User());
                        if (userWindow.ShowDialog() == true)
                        {
                            User user = userWindow.User;
                            db.Users.Add(user);
                            db.SaveChanges();
                        }
                    }
                );
            }
        }
        public CommonCommand<object?> EditCommand
        {
            get
            {
                return editCommand ??= new CommonCommand<object?>
                (
                    (selectedItem) =>
                    {
                        if (selectedItem is not User user) return;

                        User vm = new()
                        {
                            ID = user.ID,
                            Name = user.Name,
                            Age = user.Age
                        };
                        UserWindow userWindow = new(vm);


                        if (userWindow.ShowDialog() == true)
                        {
                            user.Name = userWindow.User.Name;
                            user.Age = userWindow.User.Age;
                            db.Entry(user).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                );
            }
        }
        public CommonCommand<object?> DeleteCommand
        {
            get
            {
                return deleteCommand ??= new CommonCommand<object?>
                (
                    (selectedItem) =>
                    {
                        if (selectedItem is not User user) return;
                        db.Users.Remove(user);
                        db.SaveChanges();
                    }
                );
            }
        }
    }
}

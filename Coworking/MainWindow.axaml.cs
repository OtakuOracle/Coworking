using System.Linq;
using Avalonia.Controls;
using Coworking;
using Coworking.Models;
using MsBox.Avalonia;

namespace Coworking;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }


    public void Guest_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RoomWindow catW = new RoomWindow();
        catW.Show();
        Close();

    }


    public void Auth_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var context = new TrenirovkaContext();
        var email = EmailTextBox.Text;
        var password = PasswordTextBox.Text;

        var user = context.Users.FirstOrDefault(x => x.Email == email && x.Password == password);

        if(user != null)
        {
            if(user.RoleId == 1)
            {
                Class1.isAdmin = true;
                Class1._user = user;

            }

            RoomWindow catW = new RoomWindow(user);
            catW.Show();
            Close();

        }

    }



}
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

    private void Guest_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        RoomWindow roomWindow = new RoomWindow();
        roomWindow.Show();
        Close();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private async void Auth_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var context = new TrenirovkaContext();
        var email = LoginTextBox.Text;
        var password = PasswordTextBox.Text;

        var user = context.Users.FirstOrDefault(x => x.Email == email && x.Password == password);

        if (user != null)
        {
            if (user.RoleId == 1)
            {
                Class1.isAdmin = true;
                Class1._user = user;
            }

            RoomWindow roomWindow = new RoomWindow(user);
            roomWindow.Show();
            Close();
        }
        else
        {
            var message = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Введены неверные данные", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            await message.ShowAsync();
        }
    }
}
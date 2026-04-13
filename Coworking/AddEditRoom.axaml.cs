using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Coworking.Models;
using MsBox.Avalonia;

namespace Coworking;

public partial class AddEditRoom : Window
{
    User localUser;
    private Room _room;
    private string ImageName;
    private string _currentPhotoPath;


    public AddEditRoom()
    {
        InitializeComponent();
        LoadTypeRoom();
        DataContext = new Room();
        GetInfo();
    }


    public AddEditRoom(User user)
    {
        localUser = user;
    }

    public AddEditRoom(User user, Room room)
    {
        InitializeComponent();
        using var context = new TrenirovkaContext();
        _room = room;
        localUser = user;
        GetInfo();
        LoadTypeRoom();
        DataContext = _room;
        EditBut.IsVisible = true;
        DeleteBut.IsVisible = true;
        ImageBox.Source = _room.GetPhoto;
        var a = _room.RoomTypeId;

        RoomType.SelectedItem = context.RoomTypes.Where(x => x.RoomTypeId == a).Select(x => x.RoomTypeName).FirstOrDefault();

    }


    public void GetInfo()
    {
        TrenirovkaContext context = new TrenirovkaContext();

        ListEquipment.ItemsSource = context.Equipment.ToList();
    }


    private void Button_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var a = ListEquipment.ItemsSource as List<Equipment>;
    }


    private void CheckBox_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var room = DataContext as Room;

        TrenirovkaContext context = new TrenirovkaContext();
        var equipment = context.Equipment.FirstOrDefault(x => x.EquipmentId == (int)(sender as CheckBox)!.Tag!);

        room!.Equipment.Add(equipment);

        DataContext = room;
    }

    private void Back_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var roomWindow = new RoomWindow(localUser);
        roomWindow.Show();
        this.Close();
    }

    private bool ValidateProduct(Room r)
    {
        if (r.Cost.HasValue && r.Cost < 0)
        {
            var errorPrice = MessageBoxManager.GetMessageBoxStandard(
                "Ошибка",
                "Цена не должна быть отрицательной",
                MsBox.Avalonia.Enums.ButtonEnum.Ok,
                MsBox.Avalonia.Enums.Icon.Error);
            errorPrice.ShowAsync();
            return false;
        }

        return true;
    }

    private async void Add_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            using var context = new TrenirovkaContext();
            var newRoom = DataContext as Room;

            if (!ValidateProduct(newRoom))
            {
                return;
            }

            if (RoomType.SelectedItem != null)
            {
                var type = RoomType.SelectedItem.ToString();


                var typeFin = context.RoomTypes.Where(x => x.RoomTypeName == type).Select(x => x.RoomTypeId).FirstOrDefault();


                newRoom.RoomTypeId = typeFin;

                newRoom.Photo = "images/" + ImageName;


                context.Rooms.Add(newRoom);
                await context.SaveChangesAsync();

                var message = MessageBoxManager.GetMessageBoxStandard("Успех", "Комната создана", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
                await message.ShowAsync();

                if (Class1.isAdmin == true)
                {
                    var roomWindow = new RoomWindow(Class1._user);
                    roomWindow.Show();
                    this.Close();
                }
                else
                {
                    var roomWindow = new RoomWindow();
                    roomWindow.Show();
                    this.Close();
                }
            }
            else
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Пожалуйста, заполните все поля", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                await error.ShowAsync();
            }
        }
        catch (Exception ex)
        {
            var excep = ex.ToString();
            var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", excep, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            error.ShowAsync();
        }
    }

    private void LoadTypeRoom()
    {
        using var context = new TrenirovkaContext();
        var type = context.RoomTypes.Select(x => x.RoomTypeName).ToList();
        RoomType.ItemsSource = type;
    }

    private async void AddImage_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new Avalonia.Platform.Storage.FilePickerSaveOptions
        {
            Title = "Добавить изображение",
            FileTypeChoices = new[]
            {
                FilePickerFileTypes.All
            }
        });

        if (file != null)
        {
            ImageBox.Source = new Bitmap(file.Path.LocalPath);
            ImageName = Guid.NewGuid().ToString() + ".png";
            var targetPath = AppDomain.CurrentDomain.BaseDirectory + "/images/" + ImageName;
            File.Copy(file.Path.LocalPath, targetPath);

        }
    }


    private async void Delete_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var context = new TrenirovkaContext();

        var roomId = _room.RoomId;

        var roomToDelete = context.Rooms.Where(x => x.RoomId == roomId).FirstOrDefault();

        if (roomToDelete != null)
        {
            context.Remove(roomToDelete);
            context.SaveChanges();
        }

        var nice = MessageBoxManager.GetMessageBoxStandard("Успех", "Комната удалена", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
        await nice.ShowAsync();

        var catalog = new RoomWindow();
        catalog.Show();
        this.Close();
    }

    private async void Edit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var context = new TrenirovkaContext();

        try
        {
            var type = RoomType.SelectedItem.ToString();


            var typeFin = context.RoomTypes.Where(x => x.RoomTypeName == type).Select(x => x.RoomTypeId).FirstOrDefault();


            _room.RoomTypeId = typeFin;


            if (!string.IsNullOrEmpty(ImageName))
            {
                _room.Photo = "images/" + ImageName;
            }
            else if (!string.IsNullOrEmpty(_currentPhotoPath))
            {
                _room.Photo = _currentPhotoPath;
            }


            if (!ValidateProduct(_room))
            {
                return;
            }

            context.Rooms.Update(_room);
            await context.SaveChangesAsync();

            var message = MessageBoxManager.GetMessageBoxStandard("Успех", "Комната изменена", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
            await message.ShowAsync();

            var catalog = new RoomWindow();
            catalog.Show();
            this.Close();

        }
        catch (Exception ex)
        {
            var exec = ex.ToString();
            var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", exec, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            error.ShowAsync();
        }

    }
}
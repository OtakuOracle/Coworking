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
    private Room updateroom;
    private string ImageName;
    private string _currentPhotoPath;


    public AddEditRoom() //добавление
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

    public AddEditRoom(User user, Room room) //редактирование
    {
        InitializeComponent();
        using var context = new TrenirovkaContext();
        updateroom = room;
        localUser = user;
        GetInfo();
        LoadTypeRoom();
        DataContext = updateroom;
        EditBut.IsVisible = true;
        DeleteBut.IsVisible = true;
        ImageBox.Source = updateroom.GetPhoto;

        RoomType.SelectedItem = updateroom?.RoomType?.RoomTypeName;
    }


    public void GetInfo()
    {
        using var context = new TrenirovkaContext();

        var allEquipment = context.Equipment.ToList();

        if (DataContext is Room currentRoom)
        {
            foreach (var eq in allEquipment)
            {
                bool isAssigned = currentRoom.Equipment != null &&
                                  currentRoom.Equipment.Any(re => re.EquipmentId == eq.EquipmentId);
            }
        }
        ListEquipment.ItemsSource = allEquipment;
    }






    private void CheckBox_IsCheckedChanged(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var checkBox = sender as CheckBox;
        var equipmentId = (int)checkBox!.Tag!;
        var room = DataContext as Room;

        if (room == null)
        {
            return;
        }

        if (room.Equipment == null)
        {
            room.Equipment = new List<Equipment>();
        }

        bool isChecked = (bool)checkBox.IsChecked!;

        if (isChecked)
        {
            if (!room.Equipment.Any(re => re.EquipmentId == equipmentId))
            {
                room.Equipment.Add(new Equipment { EquipmentId = equipmentId });
            }
        }
        else
        {
            var existingRoomEquipment = room.Equipment.FirstOrDefault(re => re.EquipmentId == equipmentId);
            if (existingRoomEquipment != null)
            {
                room.Equipment.Remove(existingRoomEquipment);
            }
        }

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
            if(RoomType.SelectedItem != null)
            { 
                newRoom.RoomType = context.RoomTypes.FirstOrDefault(x => x.RoomTypeName == RoomType.SelectedItem!.ToString())!;


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
            await error.ShowAsync();
        }
    }

    private void LoadTypeRoom()
    {
        using var context = new TrenirovkaContext();
        RoomType.ItemsSource = context.RoomTypes.Select(x => x.RoomTypeName).ToList();
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

        var roomId = updateroom.RoomId;

        var roomToDelete = context.Rooms.Where(x => x.RoomId == roomId).FirstOrDefault();

        //if (roomToDelete != null)
        //{
            context.Rooms.Remove(roomToDelete);
            context.SaveChanges();
        //}

        var nice = MessageBoxManager.GetMessageBoxStandard("Успех", "Комната удалена", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
        await nice.ShowAsync();

        if (Class1.isAdmin == true)
        {
            var cat = new RoomWindow(Class1._user);
            cat.Show();
            this.Close();
        }
        else
        {
            var cat = new RoomWindow();
            cat.Show();
            this.Close();
        }

    }

    private async void Edit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        using var context = new TrenirovkaContext();

        try
        {
            updateroom.RoomType = context.RoomTypes.FirstOrDefault(x => x.RoomTypeName == RoomType.SelectedItem!.ToString());



            if (!string.IsNullOrEmpty(ImageName))
            {
                updateroom.Photo = "images/" + ImageName;
            }
            else if (!string.IsNullOrEmpty(_currentPhotoPath))
            {
                updateroom.Photo = _currentPhotoPath;
            }


            if (!ValidateProduct(updateroom))
            {
                return;
            }

            context.Rooms.Update(updateroom);
            await context.SaveChangesAsync();

            var message = MessageBoxManager.GetMessageBoxStandard("Успех", "Комната изменена", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
            await message.ShowAsync();

            if (Class1.isAdmin == true)
            {
                var cat = new RoomWindow(Class1._user);
                cat.Show();
                this.Close();
            }
            else
            {
                var cat = new RoomWindow();
                cat.Show();
                this.Close();
            }


        }
        catch (Exception ex)
        {
            var exec = ex.ToString();
            var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", exec, MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            await error.ShowAsync();
        }

    }
}
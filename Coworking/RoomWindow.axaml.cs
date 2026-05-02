using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Coworking;
using Coworking.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coworking;

public partial class RoomWindow : Window
{
    User localUser;

    public RoomWindow()
    {
        InitializeComponent();
        Visibility(3);
        Get();
        LoadBox();
        FioTextBlock.Text = "Гость";
        using var context = new TrenirovkaContext();
    }

    public RoomWindow(User user)
    {
        InitializeComponent();
        localUser = user;
        Visibility(user.RoleId);
        FioTextBlock.Text = user.FullName;
        using var context = new TrenirovkaContext();
        Get();
        LoadBox();
    }

    public void Visibility(int roleId)
    {
        switch (roleId)
        {
            case 1: SearchBox.IsVisible = true; Sort.IsVisible = true; Filter.IsVisible = true; AddButton.IsVisible = true; BookingButton.IsVisible = true; break;
            case 2: SearchBox.IsVisible = true; Sort.IsVisible = true; Filter.IsVisible = true; BookingButton.IsVisible = true; break; 
        }
    }

    private void Get()
    {
        using var context = new TrenirovkaContext();

        var allRooms = context.Rooms
                                .Include(x => x.RoomType)
                                .Include(x => x.Equipment) 
                                .ToList();

        switch (Sort.SelectedIndex)
        {
            case 0:
                allRooms = allRooms.OrderBy(x => x.Capacity).ToList();
                break;
            case 1:
                allRooms = allRooms.OrderByDescending(x => x.Capacity).ToList();
                break;
            case 2:
                allRooms = allRooms.OrderBy(x => x.Cost).ToList();
                break;
            case 3:
                allRooms = allRooms.OrderByDescending(x => x.Cost).ToList();
                break;
            default:
                allRooms = allRooms.OrderBy(x => x.Capacity).ToList();
                break;
        }


        if (Filter.SelectedItem != null && Filter.SelectedIndex != 0)
        {
            allRooms = allRooms.Where(x => x.RoomType!.RoomTypeName == Filter.SelectedItem.ToString()).ToList();
        }


        if (!string.IsNullOrWhiteSpace(SearchBox.Text))
        {
            allRooms = allRooms.Where(x => x.RoomName!.ToLower().Contains(SearchBox.Text.ToLower()) ||
            x.RoomType != null && x.RoomType.RoomTypeName != null && x.RoomType.RoomTypeName.ToLower().Contains(SearchBox.Text.ToLower()) ||
            x.Equipment != null && x.Equipment.Any(eq => eq.EquipmentName!.ToLower().Contains(SearchBox.Text.ToLower()))

                ).ToList();
        }

        RoomsBox.ItemsSource = allRooms;
    }

    private void LoadRoomEquipment(Room room) 
    {
        if (room != null && room.Equipment != null)
        {
            var equipmentList = room.Equipment.ToList();

            if (this.FindControl<ItemsControl>("RoomEquipmentList") is ItemsControl equipmentListControl)
            {
                equipmentListControl.ItemsSource = equipmentList;
            }
        }
     
    }




    private void SearchBox_KeyUp(object? sender, Avalonia.Input.KeyEventArgs e)
    {
        Get();
    }

    private void Sort_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Get();
    }

    private void Filter_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Get();

    }


    private void LoadBox()
    {
        using var context = new TrenirovkaContext();

        var sup = context.RoomTypes.Select(x => x.RoomTypeName).ToList();

        sup.Add("Все типы");

        Filter.ItemsSource = sup.OrderByDescending(x => x == "Все типы");

        Filter.SelectedIndex = 0;
    }

    private void AddButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var add = new AddEditRoom();
        add.Show();
        this.Close();
    }

    private void BookingButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var book = new BookingWindow();
        book.Show();
        this.Close();
    }

    private void RoomsBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (RoomsBox.SelectedItem is Room room)
        {
            LoadRoomEquipment(room);

            if (localUser != null)
            {
                var addedit = new AddEditRoom(localUser, room);
                addedit.Show();
                this.Close();
            }
            else
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Пожалуйста, войдите в систему, чтобы редактировать", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
                error.ShowAsync();
            }
        }
    }

    private void Back_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var main = new MainWindow();
        main.Show();
        this.Close();
    }
}

using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Coworking.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;

namespace Coworking;

public partial class AddEditBooking : Window
{

    User? _localUser; 
    private Booking? _updatebooking; 

    public AddEditBooking() //добавление
    {
        InitializeComponent(); 
        LoadBookingStatus();
        LoadRoom();
        LoadService();
        LoadUser();

        DataContext = new Booking(); 

        EditBut.IsVisible = false;
        DeleteBut.IsVisible = false;
    }

    public AddEditBooking(User user)
    {
        
        _localUser = user; 
     
    }

    public AddEditBooking(User user, Booking booking) //редактирование
    {
        InitializeComponent();
        _updatebooking = booking; 
        _localUser = user;

        LoadBookingStatus();
        LoadRoom();
        LoadService();
        LoadUser();


        DateBookPicker.SelectedDate = new DateTimeOffset(booking.Date.Value.ToDateTime(TimeOnly.MinValue));
        TimeBookPicker.SelectedTime = TimeBookPicker.SelectedTime = booking.Time.Value.ToTimeSpan();



        DataContext = _updatebooking;

        EditBut.IsVisible = true;
        DeleteBut.IsVisible = true;

   
        Service.SelectedItem = _updatebooking?.Service?.ServiceName;
        BookingStatus.SelectedItem = _updatebooking?.BookingStatus?.BookingStatusName;
        Room.SelectedItem = _updatebooking?.Room?.RoomName;
        User.SelectedItem = _updatebooking?.User?.FullName;
    }



    private void Back_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (Class1.isAdmin == true)
        {
            var cat = new BookingWindow(Class1._user);
            cat.Show();
            this.Close();
        }
        else
        {
            var cat = new BookingWindow();
            cat.Show();
            this.Close();
        }
    }


    private async void Add_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        try
        {
            using var context = new TrenirovkaContext();
            var newBooking = DataContext as Booking;

            if (newBooking != null && Service.SelectedItem != null && BookingStatus.SelectedItem != null && Room.SelectedItem != null && User.SelectedItem != null)
            {
         
                newBooking.Service = context.Services.FirstOrDefault(x => x.ServiceName == Service.SelectedItem!.ToString())!;
                newBooking.BookingStatus = context.BookingStatuses.FirstOrDefault(x => x.BookingStatusName == BookingStatus.SelectedItem!.ToString())!;
                newBooking.Room = context.Rooms.FirstOrDefault(x => x.RoomName == Room.SelectedItem!.ToString())!;
                newBooking.User = context.Users.FirstOrDefault(x => x.FullName == User.SelectedItem!.ToString())!;

                newBooking.Date = new System.DateOnly(DateBookPicker.SelectedDate!.Value.Year, DateBookPicker.SelectedDate.Value.Month, DateBookPicker.SelectedDate.Value.Day);
                newBooking.Time = new System.TimeOnly(TimeBookPicker.SelectedTime!.Value.Hours, TimeBookPicker.SelectedTime.Value.Minutes, TimeBookPicker.SelectedTime.Value.Seconds);


                context.Bookings.Add(newBooking);
                await context.SaveChangesAsync();

                var message = MessageBoxManager.GetMessageBoxStandard("Успех", "Бронирование создано", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
                await message.ShowAsync();

                if (Class1.isAdmin == true)
                {
                    var cat = new BookingWindow(Class1._user);
                    cat.Show();
                    this.Close();
                }
                else
                {
                    var cat = new BookingWindow();
                    cat.Show();
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
            var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", ex.ToString(), MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            await error.ShowAsync();
        }
    }

    private void LoadUser()
    {
        using var context = new TrenirovkaContext();
        User.ItemsSource = context.Users.Select(x => x.FullName).ToList();
    }

    private void LoadRoom()
    {
        using var context = new TrenirovkaContext();
        Room.ItemsSource = context.Rooms.Select(x => x.RoomName).ToList();
    }

    private void LoadBookingStatus()
    {
        using var context = new TrenirovkaContext();
        BookingStatus.ItemsSource = context.BookingStatuses.Select(x => x.BookingStatusName).ToList();
    }

    private void LoadService()
    {
        using var context = new TrenirovkaContext();
        Service.ItemsSource = context.Services.Select(x => x.ServiceName).ToList();
    }


    private async void Delete_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

        using var context = new TrenirovkaContext();

        var bookingId = _updatebooking.BookingId;

        var bookingToDelete = context.Bookings.FirstOrDefault(x => x.BookingId == bookingId);

        context.Bookings.Remove(bookingToDelete);
        context.SaveChanges();

        var nice = MessageBoxManager.GetMessageBoxStandard("Успех", "Бронирование удалено", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
        await nice.ShowAsync();

            if (Class1.isAdmin == true)
            {
                var cat = new BookingWindow(Class1._user);
                cat.Show();
                this.Close();
            }
            else
            {
                var cat = new BookingWindow();
                cat.Show();
                this.Close();
            }
        
       
    }

    private async void Edit_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {

        using var context = new TrenirovkaContext();
        var _updatebooking = DataContext as Booking;

        try
        {
            
       
                _updatebooking!.Service = context.Services.FirstOrDefault(x => x.ServiceName == Service.SelectedItem!.ToString());
                _updatebooking!.BookingStatus = context.BookingStatuses.FirstOrDefault(x => x.BookingStatusName == BookingStatus.SelectedItem!.ToString());
                _updatebooking!.Room = context.Rooms.FirstOrDefault(x => x.RoomName == Room.SelectedItem!.ToString());
                _updatebooking!.User = context.Users.FirstOrDefault(x => x.FullName == User.SelectedItem!.ToString());

                _updatebooking.Date = new System.DateOnly(DateBookPicker.SelectedDate!.Value.Year, DateBookPicker.SelectedDate.Value.Month, DateBookPicker.SelectedDate.Value.Day);
                _updatebooking.Time = new System.TimeOnly(TimeBookPicker.SelectedTime!.Value.Hours, TimeBookPicker.SelectedTime.Value.Minutes, TimeBookPicker.SelectedTime.Value.Seconds);

          


            context.Bookings.Update(_updatebooking);

                await context.SaveChangesAsync();

                var message = MessageBoxManager.GetMessageBoxStandard("Успех", "Бронирование изменено", MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Success);
                await message.ShowAsync();

            if (Class1.isAdmin == true)
            {
                var cat = new BookingWindow(Class1._user);
                cat.Show();
                this.Close();
            }
            else
            {
                var cat = new BookingWindow();
                cat.Show();
                this.Close();
            }

        }
        catch (Exception ex)
        {
            var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", ex.ToString(), MsBox.Avalonia.Enums.ButtonEnum.Ok, MsBox.Avalonia.Enums.Icon.Error);
            await error.ShowAsync();
        }
    }


}

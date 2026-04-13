using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Coworking;
using Coworking.Models;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;

namespace Coworking;

public partial class BookingWindow : Window
{
    User localUser;

    public BookingWindow()
    {
        InitializeComponent();
        Get();
        LoadBox();
        //using var context = new TrenirovkaContext();


    }

    public BookingWindow(User user)
    {
        InitializeComponent();
        using var context = new TrenirovkaContext();
        localUser = user;
        Visibility(user.RoleId);
        Get();
        LoadBox();
    }


    public void Visibility(int roleId)
    {
        switch (roleId)
        {
            //1 не видит сфильтрации, только добавление\удал\редактиров
            //case 1:  Filter.IsVisible = true; UserFilter.IsVisible = true; break;
            case 2:  Filter.IsVisible = true; UserFilter.IsVisible = true;  break;
            default:
                Filter.IsVisible = false;
                UserFilter.IsVisible = false;
                break;
        }
    }

    private void Get()
    {
        using var context = new TrenirovkaContext();

        var allBookings = context.Bookings
            .Include(x => x.User)
            .Include(x => x.Room)
            .Include(x => x.BookingStatus)
            .Include(x => x.Service)
            .ToList();

        if (Filter.SelectedItem != null && Filter.SelectedItem.ToString() != "Все статусы")
        {
            allBookings = allBookings.Where(x => x.BookingStatus.BookingStatusName == Filter.SelectedItem.ToString()).ToList();
        }

        if (UserFilter.SelectedItem != null && UserFilter.SelectedItem.ToString() != "Все пользователи")
        {
            allBookings = allBookings.Where(x => x.User.FullName == UserFilter.SelectedItem.ToString()).ToList();
        }


        BookingsBox.ItemsSource = allBookings;
    }


    private void Filter_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Get();

    }


    private void LoadBox()
    {
        using var context = new TrenirovkaContext();

        var st = context.BookingStatuses.Select(x => x.BookingStatusName).ToList();
        st.Add("Все статусы");
        Filter.ItemsSource = st.OrderByDescending(x => x == "Все статусы");
        Filter.SelectedIndex = 0;

        var users = context.Users.Select(x => x.FullName).ToList();
        users.Add("Все пользователи");
        UserFilter.ItemsSource = users.OrderByDescending(x => x == "Все пользователи");
        UserFilter.SelectedIndex = 0;


    }



    private void UserFilter_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        Get(); 
    }




    private void Back_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var roomWindow = new RoomWindow(localUser);
        roomWindow.Show();
        this.Close();
    }
}

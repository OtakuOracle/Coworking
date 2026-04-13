using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;

namespace Coworking.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public string? RoomName { get; set; }

    public int? RoomTypeId { get; set; }

    public int? Capacity { get; set; }

    public int? Cost { get; set; }

    public int? MinTime { get; set; }

    public int? Discount { get; set; }
    public string ColorDiscount
    {
        get
        {
            if (Discount > 20)
            {
                return "#20B2AA";
            }
            else
            {
                return "";
            }
        }
    }

    public string? Description { get; set; }

    public decimal? Rating { get; set; }
    public string ColorRating
    {
        get
        {
            // Используем точку как десятичный разделитель и добавляем 'm' для Decimal
            if (Rating > 4.5m)
            {
                return "#ffa500"; // Оранжевый цвет
            }
            // Можно добавить еще условия для других цветов, например:
            // else if (Rating > 3.5m)
            // {
            //     return "#ffd700"; // Золотой
            // }
            else
            {
                return ""; // Возвращаем пустую строку, если нет особого цвета
            }
        }
    }

  
    public string? Photo { get; set; }
    public Bitmap GetPhoto
    {
        get
        {
            if (Photo != null && Photo != "")
            {
                return new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "/" + Photo);
            }
            else
            {
                return new Bitmap(AppDomain.CurrentDomain.BaseDirectory + "/images/not.png");
            }
        }
    }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual RoomType? RoomType { get; set; }

    public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();
}

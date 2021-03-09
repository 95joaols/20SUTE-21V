using System;
using System.Collections.Generic;

namespace Lab02.Domain
{
    public class User
    {
        public string Name { get; }

        public User(string name)
        {
            Name = name;
        }

        public List<Booking> Bookings { get; } = new List<Booking>();

        public User CreateBocking(DateTime startTime,
            string location,
            int DurationInMin,
            float stansadprise,
            Discount discount,
            float customDiscountValue)
        {
            Booking booking = new Booking(startTime, location, this, DurationInMin, stansadprise, discount, customDiscountValue);
            return AddBookings(booking);
        }

        public User AddBookings(Booking booking)
        {
            if (booking.User == this)
            {
                Bookings.Add(booking);
                return this;
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

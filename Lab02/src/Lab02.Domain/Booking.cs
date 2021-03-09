using System;

namespace Lab02.Domain
{
    public class Booking
    {
        public DateTime StartTime { get; }
        public TimeSpan Duration { get; }
        public float Stansadprise { get; }
        public float CustomDiscountValue { get; }
        public Discount Discount { get; }

        public string Name => User.ToString() + " " + Location;
        public User User { get; }
        public string Location { get; }
        public bool IsCancel { get; private set; } = false;



        public Booking(DateTime startTime, string location, User user, int DurationInMin, float stansadprise, Discount discount, float customDiscountValue)
        {
            Duration = TimeSpan.FromMinutes(DurationInMin);

            BookingsArgomentCheck(startTime, location, stansadprise);
            StartTime = startTime;
            Stansadprise = stansadprise;
            Discount = discount;
            CustomDiscountValue = customDiscountValue;
            Location = location;
            User = user;
        }

        private void BookingsArgomentCheck(DateTime startTime, string location, float stansadprise)
        {
            if (startTime == new DateTime())
            {
                throw new ArgumentException($"'{nameof(startTime)}' Most be set", nameof(startTime));
            }
            if (string.IsNullOrWhiteSpace(location))
            {
                throw new ArgumentException($"'{nameof(location)}' cannot be null or whitespace", nameof(location));
            }
            if (Duration.TotalMinutes < 15)
            {
                throw new InvalidOperationException($"'{nameof(Duration.TotalMinutes)}' cant be less then 15 min");
            }

            if (Duration.TotalMinutes > 60)
            {
                throw new InvalidOperationException($"'{nameof(Duration.TotalMinutes)}'cant be more the 60 min");
            }

            if (stansadprise < 0)
            {
                throw new InvalidOperationException($"'{nameof(stansadprise)}' prise cant be negativ");
            }
        }

        public float GetPrise()
        {
            switch (Discount)
            {
                case Discount.noDiscount:
                    return Stansadprise;
                case Discount.customDiscount:
                    return Stansadprise * CustomDiscountValue;
                case Discount.pensioners:
                    return Stansadprise * 0;
                case Discount.teenagers:
                    return Stansadprise * 0.7f;
                case Discount.children:
                    return Stansadprise * 0.5f;
                default:
                    return Stansadprise;
            }
        }
        public void Cancel()
        {
            if(StartTime < DateTime.Now.AddMinutes(30))
            {
                throw new InvalidOperationException();  
            }
            IsCancel = true;
        }
    }

    public enum Discount
    {
        noDiscount,
        customDiscount,
        pensioners,
        teenagers,
        children
    }
}

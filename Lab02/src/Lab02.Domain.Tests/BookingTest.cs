using FluentAssertions;

using System;
using System.Collections;
using System.Collections.Generic;

using Xunit;

namespace Lab02.Domain.Tests
{
    public class BookingTest
    {
        private const string defaltUsername = "test";
        private User defaltUser = new User(defaltUsername);

        private Booking GetDefaltBooking(DateTime? startTime = null,
            string location = "Borås",
            User user = null,
            int DurationInMin = 15,
            float stansadprise = 100,
            Discount discount = Discount.noDiscount,
            float customDiscountValue = 0)
        {
            user = GetAUser(user);

            return new Booking(GetDateTime(startTime), location, GetAUser(user), DurationInMin, stansadprise, discount, customDiscountValue);
        }

       

        private void CreateBadBooking<T>(DateTime? startTime = null,
            string location = "Borås",
            User user = null,
            int DurationInMin = 15,
            float stansadprise = 100,
            Discount discount = Discount.noDiscount,
            float customDiscountValue = 0) where T : Exception
        {
            Action sut = () => new Booking(GetDateTime(startTime), location, GetAUser(user), DurationInMin, stansadprise, discount, customDiscountValue);

            sut.Should().Throw<T>();

        }
        private static User GetAUser(User user)
        {
            if (user == null)
            {
                user = new User("Test");
            }

            return user;
        }

        private static DateTime GetDateTime(DateTime? startTime)
        {
            DateTime dateTime = DateTime.Now;
            if (startTime.HasValue)
            {
                dateTime = startTime.Value;
            }

            return dateTime;
        }

        [Fact]
        public void Check_That_GetDefaltBooking_Is_Ok()
        {
            // arrange
            Booking sut = GetDefaltBooking();

            // act
            // assert
            sut.Should().NotBeNull();
        }

        [Fact]
        public void Booking_Must_Haver_A_Start_Time()
        {

            // act
            // assert
            CreateBadBooking<ArgumentException>(startTime: new DateTime());
        }
        [Theory]
        [ClassData(typeof(OkDateTime))]
        public void Check_That_the_Right_Date_Is_Sat(DateTime okDt)
        {
            // arrange

            // act
            Booking sut = GetDefaltBooking(startTime: okDt);

            // assert
            sut.StartTime.Should().Be(okDt);
        }
        public class OkDateTime : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { DateTime.Now };
                yield return new object[] { DateTime.Now.AddDays(5) };
                yield return new object[] { DateTime.Now.AddHours(10) };
                yield return new object[] { DateTime.Now.AddMonths(4) };
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }


        [Theory]
        [InlineData(15)]
        [InlineData(50)]
        [InlineData(40)]
        [InlineData(20)]
        [InlineData(16)]
        public void Booking_Whid_Ok_Duration(int duration)
        {
            // arrange
            // act
            Booking sut = GetDefaltBooking(DurationInMin: duration);

            // assert
            sut.Duration.Minutes.Should().Be(duration);
        }

        [Theory]
        [InlineData(14)]
        [InlineData(10)]
        [InlineData(5)]
        [InlineData(1)]
        [InlineData(-5)]
        [InlineData(61)]
        [InlineData(70)]
        [InlineData(100)]
        public void Booking_Cant_be_lest_then_15_Or_More_Then_60(int duration)
        {

            // act
            // assert
            CreateBadBooking<InvalidOperationException>(DurationInMin: duration);

        }
        [Theory]
        [InlineData(14)]
        [InlineData(10)]
        [InlineData(5)]
        [InlineData(0)]
        [InlineData(60)]
        [InlineData(61)]
        [InlineData(70)]
        [InlineData(100)]
        public void Booking_Whid_Prise_Is_OK(int prise)
        {
            // arrange
            // act
            Booking sut = GetDefaltBooking(stansadprise: prise);
            // assert
            sut.Stansadprise.Should().Be(prise);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-4)]
        [InlineData(-545)]
        [InlineData(-5)]
        [InlineData(-61)]
        [InlineData(-70)]
        [InlineData(-100)]
        public void Booking_Cant_Have_Lest_then_0_In_Price(int price)
        {
            // arrange
            // act
            // assert
            CreateBadBooking<InvalidOperationException>(stansadprise: price);
        }
        [Theory]
        [ClassData(typeof(DiscountTest))]
        public void Testing_The_discount_On_Booking(Discount discount, float procent, int price, float customDiscount)
        {
            // arrange
            // act
            Booking sut = GetDefaltBooking(stansadprise: price, discount: discount, customDiscountValue: customDiscount);
            // assert
            sut.GetPrise().Should().Be(price * procent);
        }
        private class DiscountTest : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { Discount.noDiscount, 1, 100, 0 };
                yield return new object[] { Discount.customDiscount, 0.8, 100, 0.8 };
                yield return new object[] { Discount.pensioners, 0, 100, 0 };
                yield return new object[] { Discount.teenagers, 0.7, 100, 0 };
                yield return new object[] { Discount.children, 0.5, 100, 0 };

            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        [InlineData("       ")]
        public void Booking_Must_Have_A_locations(string invaledLocations)
        {
            // arrange
            // act
            // assert
            CreateBadBooking<ArgumentException>(location: invaledLocations);
        }
        [Theory]
        [InlineData("London")]
        [InlineData("stockolm")]
        [InlineData("Borås")]
        [InlineData("danmark")]
        public void Booking_Whid_locations_Is_Ok(string Locations)
        {
            // arrange
            Booking sut = GetDefaltBooking(location: Locations);
            // act
            // assert

            sut.Location.Should().Be(Locations);
        }

        [Fact]
        public void Can_Cancel_Bookings()
        {
            // arrange
            Booking sut = GetDefaltBooking(startTime:DateTime.Now.AddMinutes(31));
            // act
            sut.Cancel();

            // assert
            sut.IsCancel.Should().Be(true);
        }

        [Theory]
        [InlineData("test", "London")]
        [InlineData("Ralief", "stockolm")]

        public void The_Name_should_be_the_user_name_AND_the_location_name(string userName, string locationName)
        {
            // arrange
            User user = new User(userName);
            // act
            Booking sut = GetDefaltBooking(user: user, location: locationName);
            // assert

            sut.Name.Should().Be(userName + " " + locationName);
        }

        [Fact]
        public void User_Can_Add_A_Booking_that_User_is_On()
        {
            // arrange
            User sut = new User("sut");
            Booking booking = GetDefaltBooking(user: sut);
            // act
            sut.AddBookings(booking);

            // assert
            sut.Bookings[0].Should().Be(booking);
        }
        [Fact]
        public void User_Can_Not_Add_A_Booking_that_User_is_Not_On()
        {
            // arrange
            User sut = new User("sut");
            Booking booking = GetDefaltBooking();
            // act
            Action mut = () => sut.AddBookings(booking);


            // assert
            mut.Should().Throw<InvalidOperationException>();
        }
        [Fact]
        public void User_Can_Create_Booking_On_them()
        {
            // arrange
            User sut = new User("sut");
            // act
            sut.CreateBocking(startTime: DateTime.Now,
                location: "here",
                DurationInMin: 30,
                stansadprise: 0,
                discount: Discount.noDiscount,
                customDiscountValue: 0);


            // assert
            sut.Bookings[0].Should().NotBeNull();
        }

        [Theory]
        [ClassData(typeof(Lesthen30Min))]

        public void User_Cant_cancle_When_start_Time_Is_Less_then_30_min(DateTime startTime)
        {
            // arrange
            User sut = new User("Sut");
            sut.CreateBocking(startTime: startTime,
                location: "here",
                DurationInMin: 30,
                stansadprise: 0,
                discount: Discount.noDiscount,
                customDiscountValue: 0);
            // act
            Action Mut = () => sut.Bookings[0].Cancel();

            // assert

            Mut.Should().Throw<InvalidOperationException>();
        }
        public class Lesthen30Min : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { DateTime.Now };
                yield return new object[] { DateTime.Now.AddMinutes(5) };
                yield return new object[] { DateTime.Now.AddMinutes(10) };
                yield return new object[] { DateTime.Now.AddMinutes(4) };
                yield return new object[] { DateTime.Now.AddMinutes(-4) };
            }
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        [Theory]
        [InlineData("Sut", "465", "borås")]
        [InlineData("test", "328", "stokolm")]
        [InlineData("joacim", "1582", "here")]
        [InlineData("någon", "124", "there")]
        public void Booking_Name_For_Companies_Should_be_Company_Name_And_Company_Reg_No_And_Location_name(string name,string regnr,string location)
        {
            // arrange
            Company sut = new Company(name, regnr);
            // act
            sut.CreateBocking(startTime: DateTime.Now,
                location: location,
                DurationInMin: 30,
                stansadprise: 0,
                discount: Discount.noDiscount,
                customDiscountValue: 0);


            // assert
            sut.Bookings[0].Name.Should().Be(name +" "+ regnr+" "+ location);
        }

    }
}

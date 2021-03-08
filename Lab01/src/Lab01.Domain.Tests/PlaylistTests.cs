using System;
using Xunit;
using FluentAssertions;
using System.Collections.ObjectModel;

namespace Lab01.Domain.Tests
{
    public class PlaylistTests
    {
        [Fact]
        public void Playlist_should_be_active()
        {
            // arrange


            // act
            var sut = new Playlist("testing");
            //sut.IsActive = true;

            // assert
            //Assert.True(sut.IsActive);
            sut.IsActive.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Playlist_Invaled_name(string name)
        {
            // arrange
            Playlist sut = null;

            // act
            // assert
            sut.Invoking(s => new Playlist(name)).Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Added_Song_Is_The_Same()
        {
            // arrange
            var sut = new Playlist("New List");
            Song song = new Song("sest", "t");

            // act
            sut.AddSong(song);

            // assert
            sut.GetSong()[0].Should().BeSameAs(song);
        }
        [Fact]
        public void Playlist_Can_Add_Song()
        {
            // arrange
            var sut = new Playlist("New List");

            // act
            sut.AddSong(new Song("sest", "t"));

            // assert
            sut.GetSong().Count.Should().Be(1);
        }
        [Fact]
        public void Playlist_Can_Have_Emty_List()
        {
            // arrange

            // act
            var sut = new Playlist("New List");


            // assert
            sut.GetSong().Should().NotBeNull();
            sut.GetSong().Should().BeEmpty();
        }

        [Fact]
        public void Playlist_Cannt_Have_Abba()
        {
            // arrange
            var sut = new Playlist("New List");

            // act
            sut.AddSong(new Song("sest", "Abba"));

            // assert
            sut.GetSong().Should().NotBeNull();
            sut.GetSong().Should().BeEmpty();
        }

        [Fact]
        public void Playlist_Can_Clear_Song()
        {
            // arrange
            var sut = new Playlist("New List")
            .AddSong(new Song("dfg", "gfhg"))
            .AddSong(new Song("gfh", "gfh"))
            .AddSong(new Song("dgj", "gfh"));
            sut.GetSong().Should().NotBeEmpty();

            // act
            sut.ClearSong();

            // assert
            sut.GetSong().Should().NotBeNull();
            sut.GetSong().Should().BeEmpty();
        }
        [Fact]
        public void Duplicate_Song_Silently_ignors()
        {
            // arrange
            var sut = new Playlist("New List");

            // act
            Song s = new Song("sest", "djg");
            sut.AddSong(s)
            .AddSong(new Song("sest", "djg"))
            .AddSong(new Song("dsfg", "ghjf"));


            // assert
            sut.GetSong().Should().NotBeNull();
            sut.GetSong().Should().HaveCount(2);
        }
        [Fact]
        public void New_Song_Name_have_curent_Year_front()
        {
            // arrange
            var sut = new Playlist("New List");

            // act
            sut.AddSong(new Song("test", "djg"));


            // assert
            sut.GetSong().Should().NotBeNull();
            sut.GetSong()[0].Name.Should().Be(DateTime.Now.Year + " " + "test");
        }

        [Fact]
        public void Is_Song_In_Right_Order()
        {
            // arrange
            var sut = new Playlist("New List");

            // act
            sut.AddSong(new Song("test1", "a"))
            .AddSong(new Song("test4", "d"))
            .AddSong(new Song("test2", "b"))
            .AddSong(new Song("test3", "e"))
            .AddSong(new Song("test5", "b"));


            // assert
            sut.GetSong().Should().NotBeNull();
            sut.GetSong()[0].Name.Should().Be("2021 test1");
            sut.GetSong()[1].Name.Should().Be("2021 test2");
            sut.GetSong()[2].Name.Should().Be("2021 test3");
            sut.GetSong()[3].Name.Should().Be("2021 test4");
            sut.GetSong()[4].Name.Should().Be("2021 test5");
        }

        [Fact]
        public void Cant_Add_song_longer_then_8_min()
        {
            // arrange
            var sut = new Playlist("New List");

            // act
            sut.AddSong(new Song("test", "djg", 9));


            // assert
            sut.GetSong().Should().BeEmpty();
        }
        [Fact]
        public void Get_Unick_Artist()
        {
            // arrange
            Playlist sut = new Playlist("New List")
            .AddSong(new Song("test", "noob"))
            .AddSong(new Song("test2", "noob"))
            .AddSong(new Song("test3", "noob1"))
            .AddSong(new Song("test4", "noob2"));

            // act
            ReadOnlyCollection<Song> result = sut.GetUniqueArtists();

            // assert
            result.Should().HaveCount(3);
        }

        [Fact]
        public void I_Get_A_Event_When_I_Add_Song()
        {
            //arrange
            Playlist sut = new Playlist("New List");
            bool eventRun = false;
            sut.NewSong += (e, t) => eventRun = true;

            //act
            sut.AddSong(new Song("tt", "a"));

            //assert
            eventRun.Should().BeTrue();
        }
        [Fact]
        public void I_Get_A_Event_When_I_Revove_Song()
        {
            //arrange
            Playlist sut = new Playlist("New List");
            sut.AddSong(new Song("tt", "a"));
            bool eventRun = false;
            sut.DelietedSong += (e, t) => eventRun = true;

            //act
            sut.ClearSong();

            //assert
            eventRun.Should().BeTrue();
        }
    }
}

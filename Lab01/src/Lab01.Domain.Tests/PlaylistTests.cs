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
            Assert.Throws<ArgumentException>(() => sut = new Playlist(name));
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
            Assert.Same(song, sut.GetSong()[0]);
        }
        [Fact]
        public void Playlist_Can_Add_Song()
        {
            // arrange
            var sut = new Playlist("New List");

            // act
            sut.AddSong(new Song("sest", "t"));

            // assert
            Assert.Equal(1, sut.GetSong()?.Count);
        }
        [Fact]
        public void Playlist_Can_Have_Emty_List()
        {
            // arrange

            // act
            var sut = new Playlist("New List");


            // assert
            Assert.NotNull(sut.GetSong());
            Assert.Equal(0, sut.GetSong()?.Count);
        }

        [Fact]
        public void Playlist_Cannt_Have_Abba()
        {
            // arrange
            var sut = new Playlist("New List");

            // act
            sut.AddSong(new Song("sest", "Abba"));

            // assert
            Assert.NotNull(sut.GetSong());
            Assert.Equal(0, sut.GetSong()?.Count);
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
            Assert.NotNull(sut.GetSong());
            Assert.Equal(0, sut.GetSong()?.Count);
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
            Assert.NotNull(sut.GetSong());
            Assert.Equal(2, sut.GetSong()?.Count);
        }
        [Fact]
        public void New_Song_Name_have_curent_Year_front()
        {
            // arrange
            var sut = new Playlist("New List");

            // act
            sut.AddSong(new Song("test", "djg"));


            // assert
            Assert.NotNull(sut.GetSong());
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
            Assert.NotNull(sut.GetSong());
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
            var sut = new Playlist("New List")
            .AddSong(new Song("test", "noob"))
            .AddSong(new Song("test2", "noob"))
            .AddSong(new Song("test3", "noob1"))
            .AddSong(new Song("test4", "noob2"));

            // act
            ReadOnlyCollection<Song> result = sut.GetUniqueArtists();

            // assert
            result.Should().HaveCount(3);
        }
    }
}

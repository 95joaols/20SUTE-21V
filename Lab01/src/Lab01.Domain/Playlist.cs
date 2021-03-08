using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Lab01.Domain
{
    public class Playlist
    {
        public string Title { get; set; }
        public bool IsActive { get; set; } = true;

        public event EventHandler NewSong;
        public event EventHandler DelietedSong;


        private SortedSet<Song> song = new SortedSet<Song>();

        public Playlist(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException($"'{nameof(title)}' cannot be null or whitespace.", nameof(title));
            }
            Title = title;
        }

        public Playlist AddSong(Song Newsong)
        {
            if (!Newsong.Artist.ToLower().Contains("abba") && Newsong.Duration <= 8)
            {
                Newsong.Name = DateTime.Now.Year + " " + Newsong.Name;
                song.Add(Newsong);
                NewSong?.Invoke(this, EventArgs.Empty);
                return this;
            }
            return this;
        }
        public ReadOnlyCollection<Song> GetUniqueArtists()
        {
            List<Song> returnList = new List<Song>();
            IEnumerable<IGrouping<string, Song>> artist = song.GroupBy(s => s.Artist);
            foreach (IGrouping<string, Song> item in artist)
            {
                returnList.Add(item.FirstOrDefault());
            }
            return returnList.AsReadOnly();
        }

        public Playlist ClearSong()
        {
            song.Clear();
            DelietedSong?.Invoke(this, EventArgs.Empty);
            return this;
        }
        public ReadOnlyCollection<Song> GetSong()
        {
            return song.ToList().AsReadOnly();
        }
    }
}

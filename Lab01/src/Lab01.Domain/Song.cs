using System;
using System.Collections.Generic;

namespace Lab01.Domain
{
    public class Song : IEqualityComparer<Song>, IComparer<Song>, IComparable<Song>
    {
        public string Name { get; set; }
        public string Artist { get; set; }

        public uint Duration { get; set; }

        public Song(string name, string artist, uint duration = 0)
        {
            Name = name;
            Artist = artist;
            Duration = duration;
        }

        public bool Equals(Song x, Song y)
        {
            return String.Equals(x.Name, y.Name);
        }

        public int GetHashCode(Song obj)
        {
            return obj.Name.GetHashCode();
        }

        public int Compare(Song x, Song y)
        {
            int ArtistCompire = string.Compare(x.Artist, y.Artist);

            if (ArtistCompire != 0)
            {
                return ArtistCompire;
            }
            else
            {
                return string.Compare(x.Name, y.Name);
            }
        }

        public int CompareTo(Song other)
        {
            return this.Name.CompareTo(other.Name);
        }
    }
}

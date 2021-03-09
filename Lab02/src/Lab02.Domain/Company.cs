namespace Lab02.Domain
{
    public class Company : User
    {
        public string Regno { get; }
        public Company(string name, string regno) : base(name: name)
        {
            Regno = regno;
        }
        public override string ToString()
        {
            return Name + " " + Regno;
        }
    }
}

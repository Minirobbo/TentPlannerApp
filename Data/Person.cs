namespace TentPlannerApp.Data
{
    public class Person
    {
        public string Identifier { get; set; } = "Names";
        public string AttendeeID { get; init; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public bool Editing { get; set; } = false;

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Person))
            {
                return false;
            }
            Person oth = (Person)obj;
            return oth.AttendeeID == this.AttendeeID;
        }
    }
}

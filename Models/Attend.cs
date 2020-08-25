namespace activityCenter.Models
{
    public class Attend
    {
        public int AttendID {get; set;}

        public int UserID {get; set;}

        public int ActivityyID {get; set;}

        public User Guest {get; set;}

        public Activityy MyActivityy {get; set;}
    }
}
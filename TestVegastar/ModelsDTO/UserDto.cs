namespace TestVegastar.ModelsDTO
{
    public class UserDto
    {
        public int Userid { get; set; }

        public string Login { get; set; } = null!;

        public string Password { get; set; } = null!;

        public DateOnly? Createddate { get; set; }

        public int Usergroupid { get; set; }

        public int Userstateid { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestVegastar.Models;

public partial class User
{
    public int Userid { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateOnly? Createddate { get; set; }

    public int Usergroupid { get; set; }
    public virtual Usergroup Usergroup { get; set; }
    public int Userstateid { get; set; }
    public virtual Userstate Userstate { get; set; }
}

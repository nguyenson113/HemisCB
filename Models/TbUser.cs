using System;
using System.Collections.Generic;

namespace HemisCB.Models;

public partial class TbUser
{
    public int IdNguoi { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public int? IdPhong { get; set; }
}

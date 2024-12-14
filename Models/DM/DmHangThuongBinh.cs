using System;
using System.Collections.Generic;

namespace HemisCB.Models.DM;

public partial class DmHangThuongBinh
{
    public int IdHangThuongBinh { get; set; }

    public string? HangThuongBinh { get; set; }

    public virtual ICollection<TbNguoi> TbNguois { get; set; } = new List<TbNguoi>();
}

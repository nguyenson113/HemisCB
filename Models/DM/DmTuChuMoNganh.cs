using System;
using System.Collections.Generic;

namespace HemisCB.Models.DM;

public partial class DmTuChuMoNganh
{
    public int IdTuChuMoNganh { get; set; }

    public string? TuChuMoNganh { get; set; }

    public virtual ICollection<TbDanhSachNganhDaoTao> TbDanhSachNganhDaoTaos { get; set; } = new List<TbDanhSachNganhDaoTao>();
}

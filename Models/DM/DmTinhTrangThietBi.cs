using System;
using System.Collections.Generic;

namespace HemisCB.Models.DM;

public partial class DmTinhTrangThietBi
{
    public int IdTinhTrangThietBi { get; set; }

    public string? TinhTrangThietBi { get; set; }

    public virtual ICollection<TbChiTietTaiSanDonVi> TbChiTietTaiSanDonVis { get; set; } = new List<TbChiTietTaiSanDonVi>();
}

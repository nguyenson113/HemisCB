using System;
using System.Collections.Generic;

namespace HemisCB.Models.DM;

public partial class DmTapChiKhoaHocQuocTe
{
    public int IdTapChiKhoaHocQuocTe { get; set; }

    public string? TapChiKhoaHocQuocTe { get; set; }

    public virtual ICollection<TbBaiBaoKhdaCongBo> TbBaiBaoKhdaCongBos { get; set; } = new List<TbBaiBaoKhdaCongBo>();
}

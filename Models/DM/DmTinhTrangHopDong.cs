﻿using System;
using System.Collections.Generic;

namespace HemisCB.Models.DM;

public partial class DmTinhTrangHopDong
{
    public int IdTinhTrangHopDong { get; set; }

    public string? TinhTrangHopDong { get; set; }

    public virtual ICollection<TbHopDong> TbHopDongs { get; set; } = new List<TbHopDong>();
}

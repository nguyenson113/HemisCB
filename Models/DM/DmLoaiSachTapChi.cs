﻿using System;
using System.Collections.Generic;

namespace HemisCB.Models.DM;

public partial class DmLoaiSachTapChi
{
    public int IdLoaiSachTapChi { get; set; }

    public string? LoaiSachTapChi { get; set; }

    public virtual ICollection<TbSachDaXuatBan> TbSachDaXuatBans { get; set; } = new List<TbSachDaXuatBan>();
}

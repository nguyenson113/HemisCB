﻿using System;
using System.Collections.Generic;

namespace HemisCB.Models.DM;

public partial class DmLoaiGiaiThuongKhcn
{
    public int IdLoaiGiaiThuongKhcn { get; set; }

    public string? LoaiGiaiThuongKhcn { get; set; }

    public virtual ICollection<TbGiaiThuongKhoaHoc> TbGiaiThuongKhoaHocs { get; set; } = new List<TbGiaiThuongKhoaHoc>();
}

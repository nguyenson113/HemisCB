﻿using System;
using System.Collections.Generic;

namespace HemisCB.Models.DM;

public partial class DmLoaiLienKet
{
    public int IdLoaiLienKet { get; set; }

    public string? LoaiLienKet { get; set; }

    public virtual ICollection<TbDonViLienKetDaoTaoGiaoDuc> TbDonViLienKetDaoTaoGiaoDucs { get; set; } = new List<TbDonViLienKetDaoTaoGiaoDuc>();
}

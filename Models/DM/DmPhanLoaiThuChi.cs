using System;
using System.Collections.Generic;

namespace HemisCB.Models.DM;

public partial class DmPhanLoaiThuChi
{
    public int IdPhanLoaiThuChi { get; set; }

    public string? PhanLoaiThuChi { get; set; }

    public virtual ICollection<TbLoaiThuChi> TbLoaiThuChis { get; set; } = new List<TbLoaiThuChi>();
}

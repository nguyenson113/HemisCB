using System;
using System.Collections.Generic;

namespace HemisCB.Models.DM;

public partial class DmLoaiPhongHoc
{
    public int IdLoaiPhongHoc { get; set; }

    public string? LoaiPhongHoc { get; set; }

    public virtual ICollection<TbPhongHocGiangDuongHoiTruong> TbPhongHocGiangDuongHoiTruongs { get; set; } = new List<TbPhongHocGiangDuongHoiTruong>();
}

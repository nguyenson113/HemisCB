using System;
using System.Collections.Generic;

namespace HemisCB.Models;

public partial class TbVanBanTuChu
{
    public int IdVanBanTuChu { get; set; }

    public string? LoaiVanBan { get; set; }

    public string? NoiDungVanBan { get; set; }

    public string? QuyetDinhBanHanh { get; set; }

    public string? CoQuanQuyetDinhBanHanh { get; set; }
}

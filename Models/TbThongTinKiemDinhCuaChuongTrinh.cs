using HemisCB.Models.DM;
using System;
using System.Collections.Generic;

namespace HemisCB.Models;

public partial class TbThongTinKiemDinhCuaChuongTrinh
{
    public int IdThongTinKiemDinhCuaChuongTrinh { get; set; }

    public int IdChuongTrinhDaoTao { get; set; }

    public int? IdToChucKiemDinh { get; set; }

    public int? IdKetQuaKiemDinh { get; set; }

    public string? SoQuyetDinh { get; set; }

    public DateOnly? NgayCapChungNhanKiemDinh { get; set; }

    public DateOnly? ThoiHanKiemDinh { get; set; }

    public virtual TbChuongTrinhDaoTao IdChuongTrinhDaoTaoNavigation { get; set; } = null!;

    public virtual DmKetQuaKiemDinh? IdKetQuaKiemDinhNavigation { get; set; }

    public virtual DmToChucKiemDinh? IdToChucKiemDinhNavigation { get; set; }
}

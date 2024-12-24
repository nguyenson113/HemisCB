using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HemisCB.Models.DM;
using HemisCB.Models.DM;

namespace HemisCB.Models;

public partial class TbKhoaBoiDuongTapHuanThamGiaCuaCanBo
{
    [Display(Name = "Mã hồ sơ khóa bồi dưỡng tập huấn tham gia của cán bộ")]
    public int IdKhoaBoiDuongTapHuanThamGiaCuaCanBo { get; set; }
    [Display(Name = "Cán bộ")]
    public int? IdCanBo { get; set; }
    [Display(Name = "Tên khóa bồi dưỡng tập huấn")]
    public string? TenKhoaBoiDuongTapHuan { get; set; }
    [Display(Name = "Đơn vị tổ chức")]
    public string? DonViToChuc { get; set; }
    [Display(Name = "Loại bồi dưỡng")]
    public int? IdLoaiBoiDuong { get; set; }
    [Display(Name = "Địa điểm tổ chức")]
    public string? DiaDiemToChuc { get; set; }
    [Display(Name = "Thời gian bắt đầu")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]

    public DateOnly? ThoiGianBatDau { get; set; }
    [Display(Name = "Thời gian kết thúc")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? ThoiGianKetThuc { get; set; }

    [Display(Name = "Nguồn kinh phí")]
    public int? IdNguonKinhPhi { get; set; }
    [Display(Name = "Chứng chỉ")]
    public string? ChungChi { get; set; }
    [Display(Name = "Ngày cấp")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateOnly? NgayCap { get; set; }

    public virtual TbCanBo? IdCanBoNavigation { get; set; }

    public virtual DmLoaiBoiDuong? IdLoaiBoiDuongNavigation { get; set; }

    public virtual DmNguonKinhPhi? IdNguonKinhPhiNavigation { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HemisCB.Models.DM;
using HemisCB.Models.DM;

namespace HemisCB.Models;

public partial class TbDonViCongTacCuaCanBo
{
    [Display(Name = "Mã hồ sơ đơn vị công tác của cán bộ ")]
    public int IdDvct { get; set; }
    [Display(Name = "Cán bộ ")]
    public int? IdCanBo { get; set; }
    [Display(Name = "Mã phòng ban đơn vị")]
    public string? MaPhongBanDonVi { get; set; }
    [Display(Name = "Chức vụ")]
    public int? IdChucVu { get; set; }
    [Display(Name = "Hình thức bổ nhiệm ")]
    public int? IdHinhThucBoNhiem { get; set; }
    [Display(Name = "Số quyết định")]
    public string? SoQuyetDinh { get; set; }
    [Display(Name = "Ngày quyết định")]
    [DataType(DataType.Date)]
    //ĐỊNh dạng cho ngày/tháng/năm và cho phép cập nhật trong Edot 
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? NgayQuyetDinh { get; set; }
    [Display(Name = "Là đơn vị chính")]
    public bool? LaDonViChinh { get; set; }
    [Display(Name = "Là đơn vị giảng dạy")]
    public bool? LaDonViGiangDay { get; set; }
    [Display(Name = "Thời gian có hiệu lực")]
    [DataType(DataType.Date)]
    //ĐỊNh dạng cho ngày/tháng/năm và cho phép cập nhật trong Edot 
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? ThoiGianCoHieuLuc { get; set; }
    [Display(Name = "Thời gian hết hiệu lực")]
    [DataType(DataType.Date)]
    //ĐỊNh dạng cho ngày/tháng/năm và cho phép cập nhật trong Edot 
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? ThoiGianHetHieuLuc { get; set; }

    public virtual TbCanBo? IdCanBoNavigation { get; set; }

    public virtual DmChucVu? IdChucVuNavigation { get; set; }

    public virtual DmHinhThucBoNhiem? IdHinhThucBoNhiemNavigation { get; set; }
}

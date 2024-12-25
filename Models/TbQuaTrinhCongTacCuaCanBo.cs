using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HemisCB.Models.DM;
using HemisCB.Models.DM;

namespace HemisCB.Models;

public partial class TbQuaTrinhCongTacCuaCanBo
{
    [Display(Name = "Mã hồ sơ quá trình công tác của cán bộ")]
    public int IdQuaTrinhCongTacCuaCanBo { get; set; }
    [Display(Name = " Cán bộ")]
    public int? IdCanBo { get; set; }

    [Display(Name = "Từ tháng năm")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? TuThangNam { get; set; }

    [Display(Name = "Đến tháng năm")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? DenThangNam { get; set; }

    [Display(Name = "Chức vụ công tác")]
    public int? IdChucVu { get; set; }

    [Display(Name = "Chức danh giảng viên")]
    public int? IdChucDanhGiangVien { get; set; }
    [Display(Name = "Đơn vị công tác ")]

    public string? DonViCongTac { get; set; }

    public virtual TbCanBo? IdCanBoNavigation { get; set; }

    public virtual DmChucDanhGiangVien? IdChucDanhGiangVienNavigation { get; set; }

    public virtual DmChucVu? IdChucVuNavigation { get; set; }
}

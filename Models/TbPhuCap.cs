using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HemisCB.Models.DM;
using HemisCB.Models.DM;

namespace HemisCB.Models;

public partial class TbPhuCap
{
    [Display(Name = "Mã hồ sơ phụ cấp")]
    public int IdPhuCap { get; set; }

    [Display(Name = "Cán bộ ")]
    public int? IdCanBo { get; set; }

    [Display(Name = "Phụ cấp thu hút nghề")]
    public int? PhuCapThuHutNghe { get; set; }

    [Display(Name = "Phụ cấp thâm niên ")]
    public int? PhuCapThamNien { get; set; }

    [Display(Name = "Phụ cấp ưu đãi nghề")]
    public int? PhuCapUuDaiNghe { get; set; }

    [Display(Name = "Phụ cấp chức vụ")]
    public int? PhuCapChucVu { get; set; }

    [Display(Name = "Phụ cấp độc hại ")]
    public int? PhuCapDocHai { get; set; }

    [Display(Name = "Phụ cấp khác")]
    public int? PhuCapKhac { get; set; }
    [Display(Name = "Bậc lương")]
    public int? IdBacLuong { get; set; }
    [Display(Name = "Phần trăm vượt khung")]

    public int? PhanTramVuotKhung { get; set; }

    [Display(Name = "Hệ số lương")]
    public int? IdHeSoLuong { get; set; }

    [Display(Name = "Ngày bắt đầu hưởng lương")]
    [DataType(DataType.Date)]
    //ĐỊNh dạng cho ngày/tháng/năm và cho phép cập nhật trong Edot 
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? NgayThangNamHuongLuong { get; set; }

    public virtual DmBacLuong1? IdBacLuongNavigation { get; set; }

    public virtual TbCanBo? IdCanBoNavigation { get; set; }

    public virtual DmHeSoLuong? IdHeSoLuongNavigation { get; set; }
}
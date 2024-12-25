using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HemisCB.Models.DM;
using HemisCB.Models.DM;

namespace HemisCB.Models;

public partial class TbQuaTrinhDaoTaoCuaCanBo
{
    [Display(Name = "Mã hồ sơ quá trình đào tạo của cán bộ")]
    public int IdQuaTrinhDaoTaoCuaCanBo { get; set; }
    [Display(Name = " Cán bộ")]
    public int? IdCanBo { get; set; }
    [Display(Name = "Trình độ đào tạo")]
    public int? IdTrinhDoDaoTao { get; set; }

    [Display(Name = "Quốc gia đào tạo")]
    public int? IdQuocGiaDaoTao { get; set; }
    [Display(Name = "Cơ sở đào tạo")]
    public string? CoSoDaoTao { get; set; }
    [Display(Name = "Thời gian bắt đầu")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? ThoiGianBatDau { get; set; }

    [Display(Name = "Thời gian kết thúc")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? ThoiGianKetThuc { get; set; }
    [Display(Name = "Ngành đào tạo")]
    public int? IdNganhDaoTao { get; set; }
    [Display(Name = "Năm tốt nghiệp")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? NamTotNghiep { get; set; }
    [Display(Name = "Loại hình đào tạo")]
    public int? IdLoaiHinhDaoTao { get; set; }

    public virtual TbCanBo? IdCanBoNavigation { get; set; }

    public virtual DmLoaiHinhDaoTao? IdLoaiHinhDaoTaoNavigation { get; set; }

    public virtual DmNganhDaoTao? IdNganhDaoTaoNavigation { get; set; }

    public virtual DmQuocTich? IdQuocGiaDaoTaoNavigation { get; set; }

    public virtual DmTrinhDoDaoTao? IdTrinhDoDaoTaoNavigation { get; set; }
}

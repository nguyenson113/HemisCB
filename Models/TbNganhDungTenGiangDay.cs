using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HemisCB.Models.DM;
using HemisCB.Models.DM;

namespace HemisCB.Models;

public partial class TbNganhDungTenGiangDay
{
    [Display(Name = "Mã hồ sơ ngành dùng tên giảng dạy  ")]
    public int IdNganhDungTenGiangDay { get; set; }
    [Display(Name = "Cán bộ  ")]
    public int? IdCanBo { get; set; }
    [Display(Name = "Ngành đào tạo ")]
    public int? IdNganhDaoTao { get; set; }

    [Display(Name = "Ngành giảng dạy  ")]
    public string? TenNganhGiangDay { get; set; }
    [Display(Name = " Ngày bắt đầu ")]
    [DataType(DataType.Date)]
    //Nguyen Tan Son
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateTime? NgayBatDau { get; set; }
    [Display(Name = " Ngày kết thúc ")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateTime? NgayKetThuc { get; set; }
    [Display(Name = "Trọng số  ")]
    public double? TrongSo { get; set; }

    public virtual TbCanBo? IdCanBoNavigation { get; set; }

    public virtual DmNganhDaoTao? IdNganhDaoTaoNavigation { get; set; }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HemisCB.Models.DM;
using HemisCB.Models.DM;

namespace HemisCB.Models;

public partial class TbGiangVienNn
{
    [Display(Name = "Mã hồ sơ giảng viên ngoại ngữ ")]
    public int IdGvnn { get; set; }
    [Display(Name = " Cán bộ ")]
    public int? IdCanBo { get; set; }
    [Display(Name = "Cơ quan chủ quản ở nước ngoài ")]
    public string? CoQuanChuQuanOnuocNgoai { get; set; }
    [Display(Name = "Nội dung hoạt động tại Việt Nam")]
    public int? IdNoiDungHoatDongTaiVietNam { get; set; }

    public virtual TbCanBo? IdCanBoNavigation { get; set; }

    public virtual DmNoiDungHoatDongTaiVietNam? IdNoiDungHoatDongTaiVietNamNavigation { get; set; }
}
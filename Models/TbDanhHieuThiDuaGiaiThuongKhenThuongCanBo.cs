using System;
using System.Collections.Generic;
using HemisCB.Models.DM;
using System.ComponentModel.DataAnnotations;
using HemisCB.Models.DM;

namespace HemisCB.Models;

public partial class TbDanhHieuThiDuaGiaiThuongKhenThuongCanBo
{
    [Display(Name = "Mã hồ sơ đánh giá danh hiệu thi đua giải thưởng khen thưởng cán bộ")]
    public int IdDanhHieuThiDuaGiaiThuongKhenThuongCanBo { get; set; }

    [Display(Name = "Cán bộ")]
    public int? IdCanBo { get; set; }

    [Display(Name = "Loại danh hiệu")]
    public int? IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong { get; set; }
    [Display(Name = "Thi đua giải thưởng khen thưởng")]
    public int? IdThiDuaGiaiThuongKhenThuong { get; set; }
    [Display(Name = "Số quyết định")]
    public int? SoQuyetDinh { get; set; }
    [Display(Name = "Phương thức khen thưởng ")]

    public int? IdPhuongThucKhenThuong { get; set; }

    [Display(Name = "Năm khen thưởng ")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = false)]

    public DateOnly NamKhenThuong { get; set; }
    [Display(Name = "Cấp khen thưởng")]
    public int? IdCapKhenThuong { get; set; }

    public virtual TbCanBo? IdCanBoNavigation { get; set; }

    public virtual DmCapKhenThuong? IdCapKhenThuongNavigation { get; set; }

    public virtual DmLoaiDanhHieuThiDuaGiaiThuongKhenThuong? IdLoaiDanhHieuThiDuaGiaiThuongKhenThuongNavigation { get; set; }
    public virtual DmPhuongThucKhenThuong? IdPhuongThucKhenThuongNavigation { get; set; }
    public virtual DmThiDuaGiaiThuongKhenThuong? IdThiDuaGiaiThuongKhenThuongNavigation { get; set; }
}

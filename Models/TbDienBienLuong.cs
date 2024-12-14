using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HemisCB.Models.DM;
using HemisCB.Models.DM;

namespace HemisCB.Models;

public partial class TbDienBienLuong
{
    [Display(Name = "ID Diễn biến lương")]
    public int IdDienBienLuong { get; set; }
    [Display(Name = "ID Cán bộ")]
    public int? IdCanBo { get; set; }
    [Display(Name = "Trình độ đào tạo")]
    public int? IdTrinhDoDaoTao { get; set; }
    [Display(Name = "Ngày/tháng/năm")]
    [DataType(DataType.Date)]
    //ĐỊNh dạng cho ngày/tháng/năm và cho phép cập nhật trong Edot 
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? NgayThangNam { get; set; }
    [Display(Name = "Bậc lương ")]
    public int? IdBacLuong { get; set; }
    [Display(Name = "Hệ số lương")]
    public int? IdHeSoLuong { get; set; }

    public virtual DmBacLuong1? IdBacLuongNavigation { get; set; }

    public virtual TbCanBo? IdCanBoNavigation { get; set; }

    public virtual DmHeSoLuong? IdHeSoLuongNavigation { get; set; }

    public virtual DmTrinhDoDaoTao? IdTrinhDoDaoTaoNavigation { get; set; }
}

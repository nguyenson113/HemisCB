using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HemisCB.Models;

public partial class TbDonViThinhGiangCuaCanBo
{
    [Display(Name = "Mã hồ sơ đơn vị thỉnh giảng của cán bộ ")]
    public int IdDonViThinhGiangCuaCanBo { get; set; }
    [Display(Name = "Cán bộ ")]
    public int? IdCanBo { get; set; }
    [Display(Name = "Đơn vị thỉnh giảng")]
    public string? DonViThinhGiang { get; set; }
    [Display(Name = "Số hợp đồng thỉnh giảng ")]
    public string? SoHopDongThinhGiang { get; set; }
    [Display(Name = "Thời gian bắt đầu")]
    [DataType(DataType.Date)]
    //ĐỊNh dạng cho ngày/tháng/năm và cho phép cập nhật trong Edot 
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? ThoiGianBatDau { get; set; }
    [Display(Name = "Thời gian kết thúc")]
    [DataType(DataType.Date)]
    //ĐỊNh dạng cho ngày/tháng/năm và cho phép cập nhật trong Edot 
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? ThoiGianKetThuc { get; set; }
    [Display(Name = "Thâm niên giảng dạy")]
    public int? ThamNienGiangDay { get; set; }

    public virtual TbCanBo? IdCanBoNavigation { get; set; }
}

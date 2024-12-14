using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HemisCB.Models;

public partial class TbCanBoHuongDanThanhCongSinhVien
{
    [DisplayName("Mã cán bộ hướng dẫn")]
    public int IdCanBoHuongDanThanhCongSinhVien { get; set; }

    public int? IdCanBo { get; set; }
    [DisplayName("Mã sinh viên ")]
    public int? IdSinhVien { get; set; }
    [DisplayName("Trách nhiệm hướng dẫn ")]
    public string? TrachNhiemHuongDan { get; set; }
    [DisplayName("Thời gian bắt đầu ")]
    [DataType(DataType.Date)]
    //ĐỊNh dạng cho ngày/tháng/năm và cho phép cập nhật trong Edot 
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? ThoiGianBatDau { get; set; }
    [DisplayName("Thời gian kết thúc ")]
    [DataType(DataType.Date)]
    //ĐỊNh dạng cho ngày/tháng/năm và cho phép cập nhật trong Edot 
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? ThoiGianKetThuc { get; set; }

    public virtual TbCanBo? IdCanBoNavigation { get; set; }
}

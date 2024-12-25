using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HemisCB.Models.DM;
using HemisCB.Models.DM;

namespace HemisCB.Models;

public partial class TbHopDong
{
    [Display(Name = "Mã hồ sơ hợp đồng")]
    public int IdHopDong { get; set; }
    [Display(Name = "Cán bộ")]
    public int? IdCanBo { get; set; }
    [Display(Name = "Số hợp đồng")]
    public string? SoHopDong { get; set; }
    [Display(Name = "Loại hợp đồng")]
    public int? IdLoaiHopDong { get; set; }
    [Display(Name = "Số quyết định")]
    public string? SoQuyetDinh { get; set; }
    [Display(Name = "Ngày quyết định")]
    public DateOnly? NgayQuyetDinh { get; set; }
    [Display(Name = "Có giá trị từ")]
    [DataType(DataType.Date)]
    //ĐỊNh dạng cho ngày/tháng/năm và cho phép cập nhật trong Edot 
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? CoGiaTriTu { get; set; }
    [Display(Name = "Có giá trị đến")]
    [DataType(DataType.Date)]
    //ĐỊNh dạng cho ngày/tháng/năm và cho phép cập nhật trong Edot 
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? CoGiaTriDen { get; set; }
    [Display(Name = "Tình trạng hợp đồng")]
    public int? IdTinhTrangHopDong { get; set; }
    [Display(Name = "Làm việc toàn thời gian")]
    public bool? LamViecToanThoiGian { get; set; }

    public virtual TbCanBo? IdCanBoNavigation { get; set; }


    public virtual DmLoaiHopDong? IdLoaiHopDongNavigation { get; set; }

    public virtual DmTinhTrangHopDong? IdTinhTrangHopDongNavigation { get; set; }
}
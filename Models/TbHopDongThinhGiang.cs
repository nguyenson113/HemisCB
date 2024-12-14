using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HemisCB.Models.DM;
using HemisCB.Models.DM;

namespace HemisCB.Models;

public partial class TbHopDongThinhGiang
{
    [Display(Name = "ID Hợp đồng thỉnh giảng ")]
    public int IdHopDongThinhGiang { get; set; }
    [Display(Name = "ID Cán bộ ")]
    public int? IdCanBo { get; set; }
    [DisplayName(displayName: "Mã hợp đồng thỉnh giảng")]
    public string? MaHopDongThinhGiang { get; set; }

    [DisplayName(displayName: "Số sổ lao động")]
    public string? SoSoLaoDong { get; set; }

    [DisplayName(displayName: "Ngày cấp sổ lao động")]
    [DataType(DataType.Date)]
    //ĐỊNh dạng cho ngày/tháng/năm và cho phép cập nhật trong Edot 
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? NgayCapSoLaoDong { get; set; }

    [DisplayName(displayName: "Nơi cấp sổ lao động")]
    public string? NoiCapSoLaoDong { get; set; }

    [DisplayName(displayName: "Giá trị từ")]
    [DataType(DataType.Date)]
    //ĐỊNh dạng cho ngày/tháng/năm và cho phép cập nhật trong Edot 
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? CoGiaTriTu { get; set; }
    [DisplayName(displayName: "Giá trị đến")]
    [DataType(DataType.Date)]
    //ĐỊNh dạng cho ngày/tháng/năm và cho phép cập nhật trong Edot 
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? CoGiaTriDen { get; set; }

    [DisplayName(displayName: "Mã trạng thái hợp đồng")]
    public int? IdTrangThaiHopDong { get; set; }

    [DisplayName(displayName: "Tỷ lệ thời gian giảng dạy")]
    public int? TyLeThoiGianGiangDay { get; set; }

    public virtual TbCanBo? IdCanBoNavigation { get; set; }


    public virtual DmTrangThaiHopDong? IdTrangThaiHopDongNavigation { get; set; }
}

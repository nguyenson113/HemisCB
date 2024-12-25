using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HemisCB.Models.DM;
using HemisCB.Models.DM;

namespace HemisCB.Models;

public partial class TbGiaoVienQpan
{
    [DisplayName(displayName: "Mã hồ sơ giáo viên QPAN")]
    public int IdGiaoVienQpan { get; set; }
    [DisplayName(displayName: " Cán bộ")]
    public int? IdCanBo { get; set; }
    [DisplayName(displayName: "Năm bắt đầu biệt phái")]
    [DataType(DataType.Date)]
    //ĐỊNh dạng cho ngày/tháng/năm và cho phép cập nhật trong Edot 
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? NamBatDauBietPhai { get; set; }
    [DisplayName(displayName: "Số năm biệt phái")]
    [DataType(DataType.Date)]
    //ĐỊNh dạng cho ngày/tháng/năm và cho phép cập nhật trong Edot 
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? SoNamBietPhai { get; set; }
    [DisplayName(displayName: "Loại giảng viên quốc phòng")]

    public int? IdLoaiGiangVienQp { get; set; }
    [DisplayName(displayName: "Đào tạo GDQPAN")]

    public string? DaoTaoGdqpan { get; set; }
    [DisplayName(displayName: "Quân hàm")]
    public int? IdQuanHam { get; set; }
    [DisplayName(displayName: "Sở trường công tác")]
    public string? SoTruongCongTac { get; set; }


    public virtual TbCanBo? IdCanBoNavigation { get; set; }

    public virtual DmLoaiGiangVienQuocPhong? IdLoaiGiangVienQpNavigation { get; set; }

    public virtual DmQuanHam? IdQuanHamNavigation { get; set; }
}

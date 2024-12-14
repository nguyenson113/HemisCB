using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HemisCB.Models.DM;
using HemisCB.Models.DM;

namespace HemisCB.Models;

public partial class TbDanhGiaXepLoaiCanBo
{
    [DisplayName("ID Đánh giá xếp loại cán bộ")]
    public int IdDanhGiaXepLoaiCanBo { get; set; }

    [DisplayName("ID Cán bộ")]
    public int? IdCanBo { get; set; }

    [DisplayName(" Đánh giá")]
    public int? IdDanhGia { get; set; }

    [DisplayName("Năm đánh giá")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? NamDanhGia { get; set; }

    [DisplayName("Ngành được khen thưởng")]
    public string? NganhDuocKhenThuong { get; set; }

    public virtual TbCanBo? IdCanBoNavigation { get; set; }

    public virtual DmDanhGiaCongChucVienChuc? IdDanhGiaNavigation { get; set; }
}

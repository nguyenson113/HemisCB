using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HemisCB.Models.DM;
using HemisCB.Models.DM;

namespace HemisCB.Models;

public partial class TbDoiTuongChinhSachCanBo
{
    [Display(Name = "ID Đối tượng chính sách cán bộ  ")]
    public int IdDoiTuongChinhSachCanBo { get; set; }
    [Display(Name = "ID Cán bộ")]
    public int? IdCanBo { get; set; }
    [Display(Name = " Đối tượng chính sách ")]
    public int? IdDoiTuongChinhSach { get; set; }
    [Display(Name = "Từ ngày  ")]
    [DataType(DataType.Date)]
    //ĐỊNh dạng cho ngày/tháng/năm và cho phép cập nhật trong Edot 
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? TuNgay { get; set; }
    [Display(Name = "Đến ngày ")]
    [DataType(DataType.Date)]
    //ĐỊNh dạng cho ngày/tháng/năm và cho phép cập nhật trong Edot 
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly? DenNgay { get; set; }

    public virtual TbCanBo? IdCanBoNavigation { get; set; }

    public virtual DmDoiTuongChinhSach? IdDoiTuongChinhSachNavigation { get; set; }
}

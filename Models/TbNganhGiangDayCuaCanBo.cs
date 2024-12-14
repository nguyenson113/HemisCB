using System;
using System.Collections.Generic;
using System.ComponentModel;
using HemisCB.Models.DM;
using HemisCB.Models.DM;
namespace HemisCB.Models;

public partial class TbNganhGiangDayCuaCanBo
{
    [DisplayName(displayName: "ID Ngành giảng dạy của cán bộ")]
    public int IdNganhGiangDayCuaCanBo { get; set; }
    [DisplayName(displayName: "Cán bộ")]
    public int? IdCanBo { get; set; }
    [DisplayName(displayName: "Trình độ đào tạo")]
    public int? IdTrinhDoDaoTao { get; set; }
    [DisplayName(displayName: "Ngành ")]
    public int? IdNganh { get; set; }
    [DisplayName(displayName: "Ngành chính")]
    public bool? LaNganhChinh { get; set; }
    [DisplayName(displayName: "Đơn vị thỉnh giảng")]
    public string? DonViThinhGiang { get; set; }


    public virtual TbCanBo? IdCanBoNavigation { get; set; }

    public virtual DmNganhDaoTao? IdNganhNavigation { get; set; }

    public virtual DmTrinhDoDaoTao? IdTrinhDoDaoTaoNavigation { get; set; }
}
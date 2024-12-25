using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HemisCB.Models.DM;
using HemisCB.Models.DM;

namespace HemisCB.Models;

public partial class TbTrinhDoTiengDanToc
{
    [Display(Name = "Mã hồ sơ trình độ tiếng dân tộc")]

    public int IdTrinhDoTiengDanToc { get; set; }
    [Display(Name = " Cán bộ")]
    public int? IdCanBo { get; set; }
    [Display(Name = "Tiếng dân tộc")]
    public int? IdTiengDanToc { get; set; }
    [Display(Name = "Khung năng lực ngoại ngữ")]
    public int? IdKhungNangLucNgoaiNgu { get; set; }

    public virtual TbCanBo? IdCanBoNavigation { get; set; }

    public virtual DmKhungNangLucNgoaiNgu? IdKhungNangLucNgoaiNguNavigation { get; set; }

    public virtual DmTiengDanToc? IdTiengDanTocNavigation { get; set; }
}

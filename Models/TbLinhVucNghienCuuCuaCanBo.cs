using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HemisCB.Models.DM;
using HemisCB.Models.DM;

namespace HemisCB.Models;

public partial class TbLinhVucNghienCuuCuaCanBo
{
    [DisplayName(displayName: "ID Lĩnh vực nghiên cứu của cán bộ")]
    public int IdLinhVucNghienCuuCuaCanBo { get; set; }
    [DisplayName(displayName: "ID Cán bộ")]
    public int? IdCanBo { get; set; }
    [DisplayName(displayName: "Lĩnh vực nghiên cứu")]
    public int? IdLinhVucNghienCuu { get; set; }
    [DisplayName(displayName: "Là lĩnh vực nghiên cứu chuyên sâu")]
    public bool? LaLinhVucNghienCuuChuyenSau { get; set; }
    [DisplayName(displayName: "Số năm nghiên cứu")]
    public int? SoNamNghienCuu { get; set; }
    public virtual TbCanBo? IdCanBoNavigation { get; set; }
    public virtual DmLinhVucNghienCuu? IdLinhVucNghienCuuNavigation { get; set; }
}

using HemisCB.Models.DM;
using System;
using System.Collections.Generic;

namespace HemisCB.Models;

public partial class TbLinhVucNghienCuuCuaCanBo
{
    public int IdLinhVucNghienCuuCuaCanBo { get; set; }

    public int? IdCanBo { get; set; }

    public int? IdLinhVucNghienCuu { get; set; }

    public bool? LaLinhVucNghienCuuChuyenSau { get; set; }

    public int? SoNamNghienCuu { get; set; }

    public virtual TbCanBo? IdCanBoNavigation { get; set; }

    public virtual DmLinhVucNghienCuu? IdLinhVucNghienCuuNavigation { get; set; }
}

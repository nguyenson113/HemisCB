using HemisCB.Models.DM;
using System;
using System.Collections.Generic;

namespace HemisCB.Models;

public partial class TbNhomNganhDaoTao
{
    public int IdNhomNganhDaoTao { get; set; }

    public int? IdLinhVucDaoTao { get; set; }

    public int? IdNganhDaoTao { get; set; }

    public int? IdNhomNganh { get; set; }

    public virtual DmLinhVucDaoTao? IdLinhVucDaoTaoNavigation { get; set; }

    public virtual DmNganhDaoTao? IdNganhDaoTaoNavigation { get; set; }

    public virtual DmNhomNganh? IdNhomNganhNavigation { get; set; }
}

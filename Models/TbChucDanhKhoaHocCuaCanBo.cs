using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HemisCB.Models.DM;
using HemisCB.Models.DM;
using HemisCB.Models;


namespace HemisCB.Models;

public partial class TbChucDanhKhoaHocCuaCanBo
{
    [Display(Name = "Mã hồ sơ chức danh khoa học của cán bộ")]

    public int IdChucDanhKhoaHocCuaCanBo { get; set; }

    [Display(Name = "Cán bộ")]

    public int? IdCanBo { get; set; }

    [Display(Name = "Chức danh khoa học")]
    public int? IdChucDanhKhoaHoc { get; set; }

    [Display(Name = "Thẩm quyền quyết định")]
    public int? IdThamQuyenQuyetDinh { get; set; }

    [Display(Name = "Số quyết định")]
    public string? SoQuyetDinh { get; set; }

    [Display(Name = "Ngày quyết định")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    public DateOnly NgayQuyetDinh { get; set; }


    public virtual TbCanBo? IdCanBoNavigation { get; set; }

    public virtual DmChucDanhKhoaHoc? IdChucDanhKhoaHocNavigation { get; set; }


    public virtual DmLoaiQuyetDinh? IdThamQuyenQuyetDinhNavigation { get; set; }
}
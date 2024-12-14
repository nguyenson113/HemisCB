using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HemisCB.Models.DM;
using HemisCB.Models.DM;
using HemisCB.Models;

namespace HemisCB.Models;

public partial class TbCanBo
{
    [Display(Name = "Mã số ID cán bộ")]
    public int IdCanBo { get; set; }
    [Display(Name = "Mã người")]
    public int? IdNguoi { get; set; }
    [Display(Name = "Mã cán bộ")]
    public string? MaCanBo { get; set; }
    [Display(Name = "Chức vụ công tác")]
    public int? IdChucVuCongTac { get; set; }
    [Display(Name = "Số bảo hiểm xã hội")]
    public string? SoBaoHiemXaHoi { get; set; }
    [Display(Name = "Xã")]
    public int? IdXa { get; set; }
    [Display(Name = "Huyện")]
    public int? IdHuyen { get; set; }
    [Display(Name = "Tỉnh")]
    public int? IdTinh { get; set; }
    [Display(Name = "Email")]
    public string? Email { get; set; }
    [Display(Name = "Điện thoại")]
    public string? DienThoai { get; set; }
    [Display(Name = "Trạng thái làm việc")]
    public int? IdTrangThaiLamViec { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    [Display(Name = "Ngày chuyển trạng thái")]
    public DateOnly? NgayChuyenTrangThai { get; set; }
    [Display(Name = "Số quyết định hưu nghỉ việc")]
    public string? SoQuyetDinhHuuNghiViec { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    [Display(Name = "Ngày quyết định hưu nghỉ việc")]
    public DateOnly? NgayQuyetDinhHuuNghiViec { get; set; }
    [Display(Name = "Hình thức chuyển đến")]
    public string? HinhThucChuyenDen { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    [Display(Name = "Ngày kết thúc tạm nghỉ")]

    public DateOnly? NgayKetThucTamNghi { get; set; }
    [Display(Name = "Chức danh nghề nghiệp")]
    public int? IdChucDanhNgheNghiep { get; set; }
    [Display(Name = "Chức danh giảng viên")]
    public int? IdChucDanhGiangVien { get; set; }
    [Display(Name = "Chức danh nghiên cứu khoa học")]
    public int? IdChucDanhNghienCuuKhoaHoc { get; set; }
    [Display(Name = "Mã nghạch")]
    public int? IdNgach { get; set; }
    [Display(Name = "Cơ quan công tác")]
    public string? CoQuanCongTac { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
    [Display(Name = "Ngày tuyển dụng")]
    public DateOnly? NgayTuyenDung { get; set; }
    [Display(Name = "Chứng chỉ sư phạm giảng viên")]
    public bool? ChungChiSuPhamGiangVien { get; set; }
    [Display(Name = "Là công chức")]
    public bool? LaCongChuc { get; set; }
    [Display(Name = "Là viên chức")]
    public bool? LaVienChuc { get; set; }
    [Display(Name = "Có dạy môn Mác-Lênin")]
    public bool? CoDayMonMacLeNin { get; set; }
    [Display(Name = "Có dạy môn sư phạm")]
    public bool? CoDayMonSuPham { get; set; }
    [Display(Name = "Số giấy phép lao động")]

    public string? SoGiayPhepLaoDong { get; set; }
    [Display(Name = "Thâm niên công tác")]

    public int? ThamNienCongTac { get; set; }
    [Display(Name = "Tên doanh nghiệp")]

    public string? TenDoanhNghiep { get; set; }
    [Display(Name = "Năm kinh nghiệm giảng dạy")]

    public int? NamKinhNghiemGiangDay { get; set; }
    [Display(Name = "Giảng viên đáp ứng TT03")]

    public bool? GiangVienDapUngTt03 { get; set; }
    [Display(Name = "Id chức danh giảng viên")]

    public virtual DmChucDanhGiangVien? IdChucDanhGiangVienNavigation { get; set; }


    public virtual DmChucDanhNgheNghiep? IdChucDanhNgheNghiepNavigation { get; set; }


    public virtual DmChucDanhNckh? IdChucDanhNghienCuuKhoaHocNavigation { get; set; }


    public virtual DmChucVu? IdChucVuCongTacNavigation { get; set; }


    public virtual DmHuyen? IdHuyenNavigation { get; set; }


    public virtual DmNgach? IdNgachNavigation { get; set; }


    public virtual TbNguoi? IdNguoiNavigation { get; set; }


    public virtual DmTinh? IdTinhNavigation { get; set; }


    public virtual DmTrangThaiCanBo? IdTrangThaiLamViecNavigation { get; set; }


    public virtual DmXa? IdXaNavigation { get; set; }

    public virtual ICollection<TbCanBoHuongDanThanhCongSinhVien> TbCanBoHuongDanThanhCongSinhViens { get; set; } = new List<TbCanBoHuongDanThanhCongSinhVien>();

    public virtual ICollection<TbChucDanhKhoaHocCuaCanBo> TbChucDanhKhoaHocCuaCanBos { get; set; } = new List<TbChucDanhKhoaHocCuaCanBo>();

    public virtual ICollection<TbDanhGiaXepLoaiCanBo> TbDanhGiaXepLoaiCanBos { get; set; } = new List<TbDanhGiaXepLoaiCanBo>();

    public virtual ICollection<TbDanhHieuThiDuaGiaiThuongKhenThuongCanBo> TbDanhHieuThiDuaGiaiThuongKhenThuongCanBos { get; set; } = new List<TbDanhHieuThiDuaGiaiThuongKhenThuongCanBo>();

    public virtual ICollection<TbDienBienLuong> TbDienBienLuongs { get; set; } = new List<TbDienBienLuong>();

    public virtual ICollection<TbDoiTuongChinhSachCanBo> TbDoiTuongChinhSachCanBos { get; set; } = new List<TbDoiTuongChinhSachCanBo>();

    public virtual ICollection<TbDonViCongTacCuaCanBo> TbDonViCongTacCuaCanBos { get; set; } = new List<TbDonViCongTacCuaCanBo>();

    public virtual ICollection<TbDonViThinhGiangCuaCanBo> TbDonViThinhGiangCuaCanBos { get; set; } = new List<TbDonViThinhGiangCuaCanBo>();

    public virtual ICollection<TbGiangVienNn> TbGiangVienNns { get; set; } = new List<TbGiangVienNn>();

    public virtual ICollection<TbGiaoVienQpan> TbGiaoVienQpans { get; set; } = new List<TbGiaoVienQpan>();

    public virtual ICollection<TbGvduocCuDiDaoTao> TbGvduocCuDiDaoTaos { get; set; } = new List<TbGvduocCuDiDaoTao>();

    public virtual ICollection<TbHopDongThinhGiang> TbHopDongThinhGiangs { get; set; } = new List<TbHopDongThinhGiang>();

    public virtual ICollection<TbHopDong> TbHopDongs { get; set; } = new List<TbHopDong>();

    public virtual ICollection<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo> TbKhoaBoiDuongTapHuanThamGiaCuaCanBos { get; set; } = new List<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo>();

    public virtual ICollection<TbKyLuatCanBo> TbKyLuatCanBos { get; set; } = new List<TbKyLuatCanBo>();

    public virtual ICollection<TbLinhVucNghienCuuCuaCanBo> TbLinhVucNghienCuuCuaCanBos { get; set; } = new List<TbLinhVucNghienCuuCuaCanBo>();

    public virtual ICollection<TbNganhDungTenGiangDay> TbNganhDungTenGiangDays { get; set; } = new List<TbNganhDungTenGiangDay>();

    public virtual ICollection<TbNganhGiangDayCuaCanBo> TbNganhGiangDayCuaCanBos { get; set; } = new List<TbNganhGiangDayCuaCanBo>();

    public virtual ICollection<TbPhuCap> TbPhuCaps { get; set; } = new List<TbPhuCap>();

    public virtual ICollection<TbQuaTrinhCongTacCuaCanBo> TbQuaTrinhCongTacCuaCanBos { get; set; } = new List<TbQuaTrinhCongTacCuaCanBo>();

    public virtual ICollection<TbQuaTrinhDaoTaoCuaCanBo> TbQuaTrinhDaoTaoCuaCanBos { get; set; } = new List<TbQuaTrinhDaoTaoCuaCanBo>();

    public virtual ICollection<TbThanhPhanThamGiaDoanCongTac> TbThanhPhanThamGiaDoanCongTacs { get; set; } = new List<TbThanhPhanThamGiaDoanCongTac>();

    public virtual ICollection<TbThongTinHocTapNghienCuuSinh> TbThongTinHocTapNghienCuuSinhIdNguoiHuongDanChinhNavigations { get; set; } = new List<TbThongTinHocTapNghienCuuSinh>();

    public virtual ICollection<TbThongTinHocTapNghienCuuSinh> TbThongTinHocTapNghienCuuSinhIdNguoiHuongDanPhuNavigations { get; set; } = new List<TbThongTinHocTapNghienCuuSinh>();

    public virtual ICollection<TbTrinhDoTiengDanToc> TbTrinhDoTiengDanTocs { get; set; } = new List<TbTrinhDoTiengDanToc>();
}

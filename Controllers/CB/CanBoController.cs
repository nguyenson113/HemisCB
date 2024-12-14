using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HemisCB.Models;
using HemisCB.API;
using HemisCB.Models.DM;

namespace HemisCB.Controllers.CB
{
    public class CanBoController : Controller
    {
        private readonly ApiServices ApiServices_;

        public CanBoController(ApiServices services)
        {
            ApiServices_ = services;
        }

        //============================TẠO DANH SÁCH LẤY DỮ LIÊU TỪ APIHemis=================================
        private async Task<List<TbCanBo>> TbCanBos()
        {
            List<TbCanBo> tbCanBos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");

            List<DmChucDanhGiangVien> dmchucDanhGiangViens = await ApiServices_.GetAll<DmChucDanhGiangVien>("/api/dm/ChucDanhGiangVien");
            List<DmChucDanhNgheNghiep> dmchucDanhNgheNghieps = await ApiServices_.GetAll<DmChucDanhNgheNghiep>("/api/dm/ChucDanhNgheNghiep");
            List<DmChucDanhNckh> dmchucDanhNCKHs = await ApiServices_.GetAll<DmChucDanhNckh>("/api/dm/ChucDanhNCKH");
            List<DmChucVu> dmchucVus = await ApiServices_.GetAll<DmChucVu>("/api/dm/ChucVu");

            List<DmHuyen> dmHuyens = await ApiServices_.GetAll<DmHuyen>("/api/dm/CanBo");
            List<DmXa> dmXas = await ApiServices_.GetAll<DmXa>("/api/dm/Xa");
            List<DmTinh> dmTinhs = await ApiServices_.GetAll<DmTinh>("/api/dm/Tinh");

            List<DmNgach> dmNgachs = await ApiServices_.GetAll<DmNgach>("/api/dm/Ngach");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            List<DmTrangThaiCanBo> dmtrangThaiCanBos = await ApiServices_.GetAll<DmTrangThaiCanBo>("/api/dm/TrangThaiCanBo");
       
            tbCanBos.ForEach(item => {
                item.IdChucDanhGiangVienNavigation = dmchucDanhGiangViens.FirstOrDefault(x => x.IdChucDanhGiangVien == item.IdChucDanhGiangVien);
                item.IdChucDanhNgheNghiepNavigation = dmchucDanhNgheNghieps.FirstOrDefault(x => x.IdChucDanhNgheNghiep == item.IdChucDanhNgheNghiep);
                item.IdChucDanhNghienCuuKhoaHocNavigation = dmchucDanhNCKHs.FirstOrDefault(x => x.IdChucDanhNghienCuuKhoaHoc == item.IdChucDanhNghienCuuKhoaHoc);
                item.IdChucVuCongTacNavigation = dmchucVus.FirstOrDefault(x => x.IdChucVu == item.IdChucVuCongTac);

                item.IdHuyenNavigation = dmHuyens.FirstOrDefault(x => x.IdHuyen == item.IdHuyen);
                item.IdXaNavigation = dmXas.FirstOrDefault(x => x.IdXa == item.IdXa);
                item.IdTinhNavigation = dmTinhs.FirstOrDefault(x => x.IdTinh == item.IdTinh);
                item.IdTrangThaiLamViecNavigation = dmtrangThaiCanBos.FirstOrDefault(x => x.IdTrangThaiCanBo == item.IdTrangThaiLamViec);
                item.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdNguoi);
                item.IdNgachNavigation = dmNgachs.FirstOrDefault(x => x.IdNgach == item.IdNgach);
            });
            return tbCanBos;
        }

        // GET: CanBo
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbCanBo> getall = await TbCanBos();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: CanBo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Kiểm tra nếu id là null
            if (id == null)
            {
                return NotFound();
            }

            // Tìm quá trình công tác theo id
            var tbCanBos = await TbCanBos();
            var tbCanBo = tbCanBos.FirstOrDefault(m => m.IdCanBo == id);

            // Kiểm tra nếu không tìm thấy
            if (tbCanBo == null)
            {
                return NotFound();
            }

            // Trả về view với thông tin chi tiết
            return View(tbCanBo);
        }


        // GET: CanBo/Create
     

        public async Task<IActionResult> Create()
        {
            // Tạo danh sách lựa chọn cho IdCanBo, IdChucDanhGiangVien, IdChucVu
            ViewData["IdChucDanhGiangVien"] = new SelectList(await ApiServices_.GetAll<DmChucDanhGiangVien>("/api/dm/ChucDanhGiangVien"), "IdChucDanhGiangVien", "ChucDanhGiangVien");
            ViewData["IdChucDanhNgheNghiep"] = new SelectList(await ApiServices_.GetAll<DmChucDanhNgheNghiep>("/api/dm/ChucDanhNgheNghiep"), "IdChucDanhNgheNghiep", "ChucDanhNgheNghiep");
            ViewData["IdChucDanhNghienCuuKhoaHoc"] = new SelectList(await ApiServices_.GetAll<DmChucDanhNckh>("/api/dm/ChucDanhNCKH"), "IdChucDanhNghienCuuKhoaHoc", "ChucDanhNghienCuuKhoaHoc");
           
            ViewData["IdChucVuCongTac"] = new SelectList(await ApiServices_.GetAll<DmChucVu>("/api/dm/ChucVu"), "IdChucVu", "ChucVu");

            ViewData["IdHuyen"] = new SelectList(await ApiServices_.GetAll<DmHuyen>("/api/dm/Huyen"), "IdHuyen", "TenHuyen");
            ViewData["IdNgach"] = new SelectList(await ApiServices_.GetAll<DmNgach>("/api/dm/Ngach"), "IdNgach", "Ngach");
            ViewData["IdNguoi"] = new SelectList(await ApiServices_.GetAll<TbNguoi>("/api/Nguoi"), "IdNguoi", "IdNguoi");
            ViewData["IdTrangThaiLamViec"] = new SelectList(await ApiServices_.GetAll<DmTrangThaiCanBo>("/api/dm/TrangThaiCanBo"), "IdTrangThaiCanBo", "TrangThaiCanBo");
            ViewData["IdXa"] = new SelectList(await ApiServices_.GetAll<DmXa>("/api/dm/Xa"), "IdXa", "TenXa");
            ViewData["IdTinh"] = new SelectList(await ApiServices_.GetAll<DmTinh>("/api/dm/Tinh"), "IdTinh", "TenTinh");
            return View();
        }

        // POST: CanBo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCanBo,IdNguoi,MaCanBo,IdChucVuCongTac,SoBaoHiemXaHoi,IdXa,IdHuyen,IdTinh,Email,DienThoai,IdTrangThaiLamViec,NgayChuyenTrangThai,SoQuyetDinhHuuNghiViec,NgayQuyetDinhHuuNghiViec,HinhThucChuyenDen,NgayKetThucTamNghi,IdChucDanhNgheNghiep,IdChucDanhGiangVien,IdChucDanhNghienCuuKhoaHoc,IdNgach,CoQuanCongTac,NgayTuyenDung,ChungChiSuPhamGiangVien,LaCongChuc,LaVienChuc,CoDayMonMacLeNin,CoDayMonSuPham,SoGiayPhepLaoDong,ThamNienCongTac,TenDoanhNghiep,NamKinhNghiemGiangDay,GiangVienDapUngTt03")] TbCanBo tbCanBo)
        {
            if (await TbCanBoExists(tbCanBo.IdCanBo)) ModelState.AddModelError("IdCanBo", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbCanBo>("/api/cb/CanBo", tbCanBo);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdChucDanhGiangVien"] = new SelectList(await ApiServices_.GetAll<DmChucDanhGiangVien>("/api/dm/ChucDanhGiangVien"), "IdChucDanhGiangVien", "IdChucDanhGiangVien", tbCanBo.IdChucDanhGiangVien);
            ViewData["IdChucDanhNgheNghiep"] = new SelectList(await ApiServices_.GetAll<DmChucDanhNgheNghiep>("/api/dm/ChucDanhNgheNghiep"), "IdChucDanhNgheNghiep", "IdChucDanhNgheNghiep", tbCanBo.IdChucDanhNgheNghiep);
            ViewData["IdChucDanhNghienCuuKhoaHoc"] = new SelectList(await ApiServices_.GetAll<DmChucDanhNckh>("/api/dm/ChucDanhNCKH"), "IdChucDanhNghienCuuKhoaHoc", "IdChucDanhNghienCuuKhoaHoc", tbCanBo.IdChucDanhNghienCuuKhoaHoc);
            ViewData["IdChucVuCongTac"] = new SelectList(await ApiServices_.GetAll<DmChucVu>("/api/dm/ChucVu"), "IdChucVu", "IdChucVu", tbCanBo.IdChucVuCongTac);
            ViewData["IdHuyen"] = new SelectList(await ApiServices_.GetAll<DmHuyen>("/api/dm/Huyen"), "IdHuyen", "IdHuyen", tbCanBo.IdHuyen);
            ViewData["IdNgach"] = new SelectList(await ApiServices_.GetAll<DmNgach>("/api/dm/Ngach"), "IdNgach", "IdNgach", tbCanBo.IdNgach);
            ViewData["IdNguoi"] = new SelectList(await ApiServices_.GetAll<TbNguoi>("/api/Nguoi"), "IdNguoi", "IdNguoi", tbCanBo.IdNguoi);
            ViewData["IdTinh"] = new SelectList(await ApiServices_.GetAll<DmTinh>("/api/dm/Tinh"), "IdTinh", "IdTinh", tbCanBo.IdTinh);
            ViewData["IdTrangThaiLamViec"] = new SelectList(await ApiServices_.GetAll<DmTrangThaiCanBo>("/api/dm/TrangThaiCanBo"), "IdTrangThaiCanBo", "IdTrangThaiCanBo", tbCanBo.IdTrangThaiLamViec);
            ViewData["IdXa"] = new SelectList(await ApiServices_.GetAll<DmXa>("/api/dm/Xa"), "IdXa", "IdXa", tbCanBo.IdXa);
            return View(tbCanBo);
        }

        // GET: CanBo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbCanBo = await ApiServices_.GetId<TbCanBo>("/api/cb/CanBo", id ?? 0);
            if (tbCanBo == null)
            {
                return NotFound();
            }
            ViewData["IdChucDanhGiangVien"] = new SelectList(await ApiServices_.GetAll<DmChucDanhGiangVien>("/api/dm/ChucDanhGiangVien"), "IdChucDanhGiangVien", "IdChucDanhGiangVien", tbCanBo.IdChucDanhGiangVien);
            ViewData["IdChucDanhNgheNghiep"] = new SelectList(await ApiServices_.GetAll<DmChucDanhNgheNghiep>("/api/dm/ChucDanhNgheNghiep"), "IdChucDanhNgheNghiep", "IdChucDanhNgheNghiep", tbCanBo.IdChucDanhNgheNghiep);
            ViewData["IdChucDanhNghienCuuKhoaHoc"] = new SelectList(await ApiServices_.GetAll<DmChucDanhNckh>("/api/dm/ChucDanhNCKH"), "IdChucDanhNghienCuuKhoaHoc", "IdChucDanhNghienCuuKhoaHoc", tbCanBo.IdChucDanhNghienCuuKhoaHoc);
            ViewData["IdChucVuCongTac"] = new SelectList(await ApiServices_.GetAll<DmChucVu>("/api/dm/ChucVu"), "IdChucVu", "IdChucVu", tbCanBo.IdChucVuCongTac);
            ViewData["IdHuyen"] = new SelectList(await ApiServices_.GetAll<DmHuyen>("/api/dm/Huyen"), "IdHuyen", "IdHuyen", tbCanBo.IdHuyen);
            ViewData["IdNgach"] = new SelectList(await ApiServices_.GetAll<DmNgach>("/api/dm/Ngach"), "IdNgach", "IdNgach", tbCanBo.IdNgach);
            ViewData["IdNguoi"] = new SelectList(await ApiServices_.GetAll<TbNguoi>("/api/Nguoi"), "IdNguoi", "IdNguoi", tbCanBo.IdNguoi);
            ViewData["IdTinh"] = new SelectList(await ApiServices_.GetAll<DmTinh>("/api/dm/Tinh"), "IdTinh", "IdTinh", tbCanBo.IdTinh);
            ViewData["IdTrangThaiLamViec"] = new SelectList(await ApiServices_.GetAll<DmTrangThaiCanBo>("/api/dm/TrangThaiCanBo"), "IdTrangThaiCanBo", "IdTrangThaiCanBo", tbCanBo.IdTrangThaiLamViec);
            ViewData["IdXa"] = new SelectList(await ApiServices_.GetAll<DmXa>("/api/dm/Xa"), "IdXa", "IdXa", tbCanBo.IdXa);
            return View(tbCanBo);
        }

        // POST: CanBo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCanBo,IdNguoi,MaCanBo,IdChucVuCongTac,SoBaoHiemXaHoi,IdXa,IdHuyen,IdTinh,Email,DienThoai,IdTrangThaiLamViec,NgayChuyenTrangThai,SoQuyetDinhHuuNghiViec,NgayQuyetDinhHuuNghiViec,HinhThucChuyenDen,NgayKetThucTamNghi,IdChucDanhNgheNghiep,IdChucDanhGiangVien,IdChucDanhNghienCuuKhoaHoc,IdNgach,CoQuanCongTac,NgayTuyenDung,ChungChiSuPhamGiangVien,LaCongChuc,LaVienChuc,CoDayMonMacLeNin,CoDayMonSuPham,SoGiayPhepLaoDong,ThamNienCongTac,TenDoanhNghiep,NamKinhNghiemGiangDay,GiangVienDapUngTt03")] TbCanBo tbCanBo)
        {
            if (id != tbCanBo.IdCanBo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbCanBo>("/api/cb/CanBo", id, tbCanBo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbCanBoExists(tbCanBo.IdCanBo) == false)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdChucDanhGiangVien"] = new SelectList(await ApiServices_.GetAll<DmChucDanhGiangVien>("/api/dm/ChucDanhGiangVien"), "IdChucDanhGiangVien", "IdChucDanhGiangVien", tbCanBo.IdChucDanhGiangVien);
            ViewData["IdChucDanhNgheNghiep"] = new SelectList(await ApiServices_.GetAll<DmChucDanhNgheNghiep>("/api/dm/ChucDanhNgheNghiep"), "IdChucDanhNgheNghiep", "IdChucDanhNgheNghiep", tbCanBo.IdChucDanhNgheNghiep);
            ViewData["IdChucDanhNghienCuuKhoaHoc"] = new SelectList(await ApiServices_.GetAll<DmChucDanhNckh>("/api/dm/ChucDanhNCKH"), "IdChucDanhNghienCuuKhoaHoc", "IdChucDanhNghienCuuKhoaHoc", tbCanBo.IdChucDanhNghienCuuKhoaHoc);
            ViewData["IdChucVuCongTac"] = new SelectList(await ApiServices_.GetAll<DmChucVu>("/api/dm/ChucVu"), "IdChucVu", "IdChucVu", tbCanBo.IdChucVuCongTac);
            ViewData["IdHuyen"] = new SelectList(await ApiServices_.GetAll<DmHuyen>("/api/dm/Huyen"), "IdHuyen", "IdHuyen", tbCanBo.IdHuyen);
            ViewData["IdNgach"] = new SelectList(await ApiServices_.GetAll<DmNgach>("/api/dm/Ngach"), "IdNgach", "IdNgach", tbCanBo.IdNgach);
            ViewData["IdNguoi"] = new SelectList(await ApiServices_.GetAll<TbNguoi>("/api/Nguoi"), "IdNguoi", "IdNguoi", tbCanBo.IdNguoi);
            ViewData["IdTinh"] = new SelectList(await ApiServices_.GetAll<DmTinh>("/api/dm/Tinh"), "IdTinh", "IdTinh", tbCanBo.IdTinh);
            ViewData["IdTrangThaiLamViec"] = new SelectList(await ApiServices_.GetAll<DmTrangThaiCanBo>("/api/dm/TrangThaiCanBo"), "IdTrangThaiCanBo", "IdTrangThaiCanBo", tbCanBo.IdTrangThaiLamViec);
            ViewData["IdXa"] = new SelectList(await ApiServices_.GetAll<DmXa>("/api/dm/Xa"), "IdXa", "IdXa", tbCanBo.IdXa);
            return View(tbCanBo);
        }

        // GET: CanBo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbCanBos = await ApiServices_.GetAll<TbQuaTrinhDaoTaoCuaCanBo>("/api/cb/CanBo");
            var tbCanBo = tbCanBos.FirstOrDefault(m => m.IdCanBo == id);
            if (tbCanBo == null)
            {
                return NotFound();
            }

            return View(tbCanBo);
        }


        // POST: CanBo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbCanBo>("/api/cb/CanBo", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        private async Task<bool> TbCanBoExists(int id)
        {
            var tbCanBos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            return tbCanBos.Any(e => e.IdCanBo == id);
        }
    }
}

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HemisCB.Models;
using HemisCB.Models.DM;
using System.Text;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using HemisCB.API;
using HemisCB.Models.DM;

namespace HemisCB.Controllers.CB
{
    public class NguoiController : Controller
    {
        private readonly ApiServices ApiServices_;

        //Nhận HemisContext truyền từ service
        public NguoiController(ApiServices services)
        {
            ApiServices_ = services;
        }

        //============================TẠO DANH SÁCH LẤY DỮ LIÊU TỪ APIHemis=================================
        private async Task<List<TbNguoi>> TbNguois()
        {

            //Tạo danh sách CanBo 
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");

            //Tạo danh sách lấy dữ liệu các biến từ APIHemis 
            List<DmChucDanhKhoaHoc> dmChucDanhKhoaHocs = await ApiServices_.GetAll<DmChucDanhKhoaHoc>("/api/dm/ChucDanhKhoaHoc");
            List<DmNganhDaoTao> dmNganhDaoTaos = await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao");
            List<DmTonGiao> dmTonGiaos = await ApiServices_.GetAll<DmTonGiao>("/api/dm/TonGiao");
            List<DmDanToc> dmDanTocs = await ApiServices_.GetAll<DmDanToc>("/api/dm/DanToc");

            List<DmHoGiaDinhChinhSach> dmHoGiaDinhChinhSachs = await ApiServices_.GetAll<DmHoGiaDinhChinhSach>("/api/dm/HoGiaDinhChinhSach");
            List<DmGioiTinh> dmGioiTinhs = await ApiServices_.GetAll<DmGioiTinh>("/api/dm/GioiTinh");
            List<DmKhungNangLucNgoaiNgu> dmKhungNangLucNgoaiNgus = await ApiServices_.GetAll<DmKhungNangLucNgoaiNgu>("/api/dm/KhungNangLucNgoaiNgu");
            List<DmNgoaiNgu> dmNgoaiNgus = await ApiServices_.GetAll<DmNgoaiNgu>("/api/dm/NgoaiNgu");

            List<DmQuocTich> dmQuocTichs = await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich");
            List<DmHangThuongBinh> dmHangThuongBinhs = await ApiServices_.GetAll<DmHangThuongBinh>("/api/dm/HangThuongBinh");
            List<DmTrinhDoDaoTao> dmTrinhDoDaoTaos = await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao");
            List<DmTrinhDoLyLuanChinhTri> dmTrinhDoLyLuanChinhTris = await ApiServices_.GetAll<DmTrinhDoLyLuanChinhTri>("/api/dm/TrinhDoLyLuanChinhTri");

            List<DmTrinhDoQuanLyNhaNuoc> dmTrinhDoQuanLyNhaNuocs = await ApiServices_.GetAll<DmTrinhDoQuanLyNhaNuoc>("/api/dm/TrinhDoQuanLyNhaNuoc");
            List<DmTrinhDoTinHoc> dmTrinhDoTinHocs = await ApiServices_.GetAll<DmTrinhDoTinHoc>("/api/dm/TrinhDoTinHoc");
           

            tbNguois.ForEach(item => {
                item.IdChucDanhKhoaHocNavigation = dmChucDanhKhoaHocs.FirstOrDefault(x => x.IdChucDanhKhoaHoc == item.IdChucDanhKhoaHoc);
                item.IdChuyenMonDaoTaoNavigation = dmNganhDaoTaos.FirstOrDefault(x => x.IdNganhDaoTao == item.IdChuyenMonDaoTao);
                item.IdTonGiaoNavigation = dmTonGiaos.FirstOrDefault(x => x.IdTonGiao == item.IdTonGiao);
                item.IdDanTocNavigation = dmDanTocs.FirstOrDefault(x => x.IdDanToc == item.IdDanToc);

                item.IdGiaDinhChinhSachNavigation = dmHoGiaDinhChinhSachs.FirstOrDefault(x => x.IdHoGiaDinhChinhSach == item.IdGiaDinhChinhSach);
                item.IdGioiTinhNavigation = dmGioiTinhs.FirstOrDefault(x => x.IdGioiTinh == item.IdGioiTinh);
                item.IdKhungNangLucNgoaiNgucNavigation = dmKhungNangLucNgoaiNgus.FirstOrDefault(x => x.IdKhungNangLucNgoaiNgu == item.IdKhungNangLucNgoaiNguc);
                item.IdNgoaiNguNavigation = dmNgoaiNgus.FirstOrDefault(x => x.IdNgoaiNgu == item.IdNgoaiNgu);

                item.IdQuocTichNavigation = dmQuocTichs.FirstOrDefault(x => x.IdQuocTich == item.IdQuocTich);
                item.IdThuongBinhHangNavigation = dmHangThuongBinhs.FirstOrDefault(x => x.IdHangThuongBinh == item.IdThuongBinhHang);
                item.IdTrinhDoDaoTaoNavigation = dmTrinhDoDaoTaos.FirstOrDefault(x => x.IdTrinhDoDaoTao == item.IdTrinhDoDaoTao);
                item.IdTrinhDoLyLuanChinhTriNavigation = dmTrinhDoLyLuanChinhTris.FirstOrDefault(x => x.IdTrinhDoLyLuanChinhTri == item.IdTrinhDoLyLuanChinhTri);

                item.IdTrinhDoQuanLyNhaNuocNavigation = dmTrinhDoQuanLyNhaNuocs.FirstOrDefault(x => x.IdTrinhDoQuanLyNhaNuoc == item.IdTrinhDoQuanLyNhaNuoc);
                item.IdTrinhDoTinHocNavigation = dmTrinhDoTinHocs.FirstOrDefault(x => x.IdTrinhDoTinHoc == item.IdTrinhDoTinHoc);
            
               
            });
            return tbNguois;
        }

        public async Task<IActionResult> Statistics()
        {
            List<TbNguoi> getall = await TbNguois();
            return View(getall);
        }

        //GET: /Nguoi | /Nguoi/Index
        // Thu thập các dữ liệu cần thiết của table TbNguoi để trả về hiển thị trên trang index
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbNguoi> getall = await TbNguois();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        //GET: /Nguoi/Details
        // Thu thập thông tin chi tiết của người với Id cụ thể để trả về hiện thị ở Details
        public async Task<IActionResult> Details(int? id)
        {
            // Kiểm tra nếu id là null
            if (id == null)
            {
                return NotFound();
            }

            // Tìm quá trình công tác theo id
            var tbNguois = await TbNguois();
            var tbNguoi = tbNguois.FirstOrDefault(m => m.IdNguoi == id);

            // Kiểm tra nếu không tìm thấy
            if (tbNguoi == null)
            {
                return NotFound();
            }

            // Trả về view với thông tin chi tiết
            return View(tbNguoi);
        }


        // GET: Nguoi/Create
        public async Task<IActionResult> Create()
        {
            // Tạo danh sách lựa chọn cho IdCanBo, IdChucDanhGiangVien, IdChucVu
            ViewData["IdChucDanhKhoaHoc"] = new SelectList(await ApiServices_.GetAll<DmChucDanhKhoaHoc>("/api/dm/ChucDanhKhoaHoc"), "IdChucDanhKhoaHoc", "ChucDanhKhoaHoc");
            ViewData["IdChuyenMonDaoTao"] = new SelectList(await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao"), "IdNganhDaoTao", "NganhDaoTao");
            ViewData["IdTonGiao"] = new SelectList(await ApiServices_.GetAll<DmTonGiao>("/api/dm/TonGiao"), "IdTonGiao", "TonGiao");
            ViewData["IdDanToc"] = new SelectList(await ApiServices_.GetAll<DmDanToc>("/api/dm/DanToc"), "IdDanToc", "DanToc");
            ViewData["IdGiaDinhChinhSach"] = new SelectList(await ApiServices_.GetAll<DmHoGiaDinhChinhSach>("/api/dm/HoGiaDinhChinhSach"), "IdHoGiaDinhChinhSach", "HoGiaDinhChinhSach");
            ViewData["IdGioiTinh"] = new SelectList(await ApiServices_.GetAll<DmGioiTinh>("/api/dm/GioiTinh"), "IdGioiTinh", "GioiTinh");
            ViewData["IdKhungNangLucNgoaiNguc"] = new SelectList(await ApiServices_.GetAll<DmKhungNangLucNgoaiNgu>("/api/dm/KhungNangLucNgoaiNgu"), "IdKhungNangLucNgoaiNgu", "TenKhungNangLucNgoaiNgu");
            ViewData["IdNgoaiNgu"] = new SelectList(await ApiServices_.GetAll<DmNgoaiNgu>("/api/dm/NgoaiNgu"), "IdNgoaiNgu", "NgoaiNgu");
            ViewData["IdQuocTich"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc");
            ViewData["IdThuongBinhHang"] = new SelectList(await ApiServices_.GetAll<DmHangThuongBinh>("/api/dm/HangThuongBinh"), "IdHangThuongBinh", "HangThuongBinh");
            ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao");
            ViewData["IdTrinhDoLyLuanChinhTri"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoLyLuanChinhTri>("/api/dm/TrinhDoLyLuanChinhTri"), "IdTrinhDoLyLuanChinhTri", "TenTrinhDoLyLuanChinhTri");
            ViewData["IdTrinhDoQuanLyNhaNuoc"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoQuanLyNhaNuoc>("/api/dm/TrinhDoQuanLyNhaNuoc"), "IdTrinhDoQuanLyNhaNuoc", "TrinhDoQuanLyNhaNuoc");
            ViewData["IdTrinhDoTinHoc"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoTinHoc>("/api/dm/TrinhDoTinHoc"), "IdTrinhDoTinHoc", "TrinhDoTinHoc");
            return View();
        }



        //POST: /Nguoi/Create
        [HttpPost]//Để action Khớp với POST HTTP 
        [ValidateAntiForgeryToken]//Ngăn chặn Cross-Site Request Forgery qua Token verification (Đối chiếu với token được gửi bởi client mà trước đó được inject ở Form qua TagHelper) 
        public async Task<IActionResult> Create([Bind("IdNguoi,Ho,Ten,IdQuocTich,SoCccd,NgayCapCccd,NoiCapCccd,NgaySinh,IdGioiTinh,IdDanToc,IdTonGiao,NgayVaoDoan,NgayVaoDang,NgayVaoDangChinhThuc,NgayNhapNgu,NgayXuatNgu,IdThuongBinhHang,IdGiaDinhChinhSach,IdChucDanhKhoaHoc,IdTrinhDoDaoTao,IdChuyenMonDaoTao,IdNgoaiNgu,IdKhungNangLucNgoaiNguc,IdTrinhDoLyLuanChinhTri,IdTrinhDoQuanLyNhaNuoc,IdTrinhDoTinHoc")] TbNguoi tbNguoi)
        {
            try
            {
                //Kiểm tra xem đã tồn tại IdNguoi chưa nếu tồn tại thêm ModelError cho IdNguoi\
              
                if (await TbNguoiExists(tbNguoi.IdNguoi)) ModelState.AddModelError("IdNguoi", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbCanBo>("/api/Nguoi", tbNguoi);
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdChucDanhKhoaHoc"] = new SelectList(await ApiServices_.GetAll<DmChucDanhKhoaHoc>("/api/dm/ChucDanhKhoaHoc"), "IdChucDanhKhoaHoc", "ChucDanhKhoaHoc", tbNguoi.IdChucDanhKhoaHoc);
                ViewData["IdChuyenMonDaoTao"] = new SelectList(await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao"), "IdNganhDaoTao", "NganhDaoTao", tbNguoi.IdChuyenMonDaoTao);
                ViewData["IdTonGiao"] = new SelectList(await ApiServices_.GetAll<DmTonGiao>("/api/dm/TonGiao"), "IdTonGiao", "TonGiao", tbNguoi.IdDanToc);
                ViewData["IdDanToc"] = new SelectList(await ApiServices_.GetAll<DmDanToc>("/api/dm/DanToc"), "IdDanToc", "DanToc", tbNguoi.IdDanToc);
                ViewData["IdGiaDinhChinhSach"] = new SelectList(await ApiServices_.GetAll<DmHoGiaDinhChinhSach>("/api/dm/HoGiaDinhChinhSach"), "IdHoGiaDinhChinhSach", "HoGiaDinhChinhSach", tbNguoi.IdGiaDinhChinhSach);
                ViewData["IdGioiTinh"] = new SelectList(await ApiServices_.GetAll<DmGioiTinh>("/api/dm/GioiTinh"), "IdGioiTinh", "GioiTinh", tbNguoi.IdGioiTinh);
                ViewData["IdKhungNangLucNgoaiNguc"] = new SelectList(await ApiServices_.GetAll<DmKhungNangLucNgoaiNgu>("/api/dm/KhungNangLucNgoaiNgu"), "IdKhungNangLucNgoaiNgu", "TenKhungNangLucNgoaiNgu", tbNguoi.IdKhungNangLucNgoaiNguc);
                ViewData["IdNgoaiNgu"] = new SelectList(await ApiServices_.GetAll<DmNgoaiNgu>("/api/dm/NgoaiNgu"), "IdNgoaiNgu", "NgoaiNgu", tbNguoi.IdNgoaiNgu);
                ViewData["IdQuocTich"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc", tbNguoi.IdQuocTich);
                ViewData["IdThuongBinhHang"] = new SelectList(await ApiServices_.GetAll<DmHangThuongBinh>("/api/dm/HangThuongBinh"), "IdHangThuongBinh", "HangThuongBinh", tbNguoi.IdThuongBinhHang);
                ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao", tbNguoi.IdTrinhDoDaoTao);
                ViewData["IdTrinhDoLyLuanChinhTri"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoLyLuanChinhTri>("/api/dm/TrinhDoLyLuanChinhTri"), "IdTrinhDoLyLuanChinhTri", "TenTrinhDoLyLuanChinhTri", tbNguoi.IdTrinhDoLyLuanChinhTri);
                ViewData["IdTrinhDoQuanLyNhaNuoc"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoQuanLyNhaNuoc>("/api/dm/TrinhDoQuanLyNhaNuoc"), "IdTrinhDoQuanLyNhaNuoc", "TrinhDoQuanLyNhaNuoc", tbNguoi.IdTrinhDoQuanLyNhaNuoc);
                ViewData["IdTrinhDoTinHoc"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoTinHoc>("/api/dm/TrinhDoTinHoc"), "IdTrinhDoTinHoc", "TrinhDoTinHoc", tbNguoi.IdTrinhDoTinHoc);
                return View(tbNguoi);
            }
            catch (Exception ex)
            {
                export_message(ex.Message + "\n" + (ex.InnerException == null ? "" : ex.InnerException.ToString()));
                return BadRequest();
            }
        }

        // GET: Nguoi/Edit/5
        //Chỉnh sửa thông tin 
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                //Tìm model người khớp với Id được cung cấp
                var tbNguoi = await ApiServices_.GetId<TbNguoi>("/api/Nguoi", id ?? 0);
                if (tbNguoi == null)
                {
                    return NotFound();
                }
                //Nếu tìm thấy, khởi tạo các giá trị các giá trị cần thiết và trả về view
                ViewData["IdChucDanhKhoaHoc"] = new SelectList(await ApiServices_.GetAll<DmChucDanhKhoaHoc>("/api/dm/ChucDanhKhoaHoc"), "IdChucDanhKhoaHoc", "ChucDanhKhoaHoc", tbNguoi.IdChucDanhKhoaHoc);
                ViewData["IdChuyenMonDaoTao"] = new SelectList(await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao"), "IdNganhDaoTao", "NganhDaoTao", tbNguoi.IdChuyenMonDaoTao);
                ViewData["IdTonGiao"] = new SelectList(await ApiServices_.GetAll<DmTonGiao>("/api/dm/TonGiao"), "IdTonGiao", "TonGiao", tbNguoi.IdDanToc);
                ViewData["IdDanToc"] = new SelectList(await ApiServices_.GetAll<DmDanToc>("/api/dm/DanToc"), "IdDanToc", "DanToc", tbNguoi.IdDanToc);
                ViewData["IdGiaDinhChinhSach"] = new SelectList(await ApiServices_.GetAll<DmHoGiaDinhChinhSach>("/api/dm/HoGiaDinhChinhSach"), "IdHoGiaDinhChinhSach", "HoGiaDinhChinhSach", tbNguoi.IdGiaDinhChinhSach);
                ViewData["IdGioiTinh"] = new SelectList(await ApiServices_.GetAll<DmGioiTinh>("/api/dm/GioiTinh"), "IdGioiTinh", "GioiTinh", tbNguoi.IdGioiTinh);
                ViewData["IdKhungNangLucNgoaiNguc"] = new SelectList(await ApiServices_.GetAll<DmKhungNangLucNgoaiNgu>("/api/dm/KhungNangLucNgoaiNgu"), "IdKhungNangLucNgoaiNgu", "TenKhungNangLucNgoaiNgu", tbNguoi.IdKhungNangLucNgoaiNguc);
                ViewData["IdNgoaiNgu"] = new SelectList(await ApiServices_.GetAll<DmNgoaiNgu>("/api/dm/NgoaiNgu"), "IdNgoaiNgu", "NgoaiNgu", tbNguoi.IdNgoaiNgu);
                ViewData["IdQuocTich"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc", tbNguoi.IdQuocTich);
                ViewData["IdThuongBinhHang"] = new SelectList(await ApiServices_.GetAll<DmHangThuongBinh>("/api/dm/HangThuongBinh"), "IdHangThuongBinh", "HangThuongBinh", tbNguoi.IdThuongBinhHang);
                ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao", tbNguoi.IdTrinhDoDaoTao);
                ViewData["IdTrinhDoLyLuanChinhTri"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoLyLuanChinhTri>("/api/dm/TrinhDoLyLuanChinhTri"), "IdTrinhDoLyLuanChinhTri", "TenTrinhDoLyLuanChinhTri", tbNguoi.IdTrinhDoLyLuanChinhTri);
                ViewData["IdTrinhDoQuanLyNhaNuoc"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoQuanLyNhaNuoc>("/api/dm/TrinhDoQuanLyNhaNuoc"), "IdTrinhDoQuanLyNhaNuoc", "TrinhDoQuanLyNhaNuoc", tbNguoi.IdTrinhDoQuanLyNhaNuoc);
                ViewData["IdTrinhDoTinHoc"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoTinHoc>("/api/dm/TrinhDoTinHoc"), "IdTrinhDoTinHoc", "TrinhDoTinHoc", tbNguoi.IdTrinhDoTinHoc);
                return View(tbNguoi);
            }
            catch (Exception ex)
            {
                export_message(ex.Message + "\n" + (ex.InnerException == null ? "" : ex.InnerException.ToString()));
                return BadRequest();
            }
        }

        // POST: Nguoi/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNguoi,Ho,Ten,IdQuocTich,SoCccd,NgayCapCccd,NoiCapCccd,NgaySinh,IdGioiTinh,IdDanToc,IdTonGiao,NgayVaoDoan,NgayVaoDang,NgayVaoDangChinhThuc,NgayNhapNgu,NgayXuatNgu,IdThuongBinhHang,IdGiaDinhChinhSach,IdChucDanhKhoaHoc,IdTrinhDoDaoTao,IdChuyenMonDaoTao,IdNgoaiNgu,IdKhungNangLucNgoaiNguc,IdTrinhDoLyLuanChinhTri,IdTrinhDoQuanLyNhaNuoc,IdTrinhDoTinHoc")] TbNguoi tbNguoi)
        {
            try
            {
                if (id != tbNguoi.IdNguoi)
                {
                    return NotFound();
                }
                //check_null(tbNguoi);
                if (ModelState.IsValid)
                {
                    //Kiểm tra quá trình update
                    try
                    {
                        await ApiServices_.Update<TbNguoi>("/api/Nguoi", id, tbNguoi);
                    }
                    catch (DbUpdateConcurrencyException)
                    {//Nếu QUERY thực thi ở Database không thay đổi giá trị nào thì trả về DbUpdateConcurrencyException
                     //Lỗi xảy ra chủ yếu do table cần thực thi UPDATE ở database đã bị thay đổi sau khi QUERY được gửi đi
                     //và trước khi QUERY UPDATE được thực hiện
                     //Khi gửi QUERY bên cạnh điều kiện ID EF sẽ thêm điều kiện Version để so sánh version khi gửi và khi thực thi 
                        if (await TbNguoiExists(tbNguoi.IdNguoi) == false)
                        {
                            return NotFound();
                        }
                        else
                        {
                            return BadRequest();
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdChucDanhKhoaHoc"] = new SelectList(await ApiServices_.GetAll<DmChucDanhKhoaHoc>("/api/dm/ChucDanhKhoaHoc"), "IdChucDanhKhoaHoc", "ChucDanhKhoaHoc", tbNguoi.IdChucDanhKhoaHoc);
                ViewData["IdChuyenMonDaoTao"] = new SelectList(await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao"), "IdNganhDaoTao", "NganhDaoTao", tbNguoi.IdChuyenMonDaoTao);
                ViewData["IdTonGiao"] = new SelectList(await ApiServices_.GetAll<DmTonGiao>("/api/dm/TonGiao"), "IdTonGiao", "TonGiao", tbNguoi.IdDanToc);
                ViewData["IdDanToc"] = new SelectList(await ApiServices_.GetAll<DmDanToc>("/api/dm/DanToc"), "IdDanToc", "DanToc", tbNguoi.IdDanToc);
                ViewData["IdGiaDinhChinhSach"] = new SelectList(await ApiServices_.GetAll<DmHoGiaDinhChinhSach>("/api/dm/HoGiaDinhChinhSach"), "IdHoGiaDinhChinhSach", "HoGiaDinhChinhSach", tbNguoi.IdGiaDinhChinhSach);
                ViewData["IdGioiTinh"] = new SelectList(await ApiServices_.GetAll<DmGioiTinh>("/api/dm/GioiTinh"), "IdGioiTinh", "GioiTinh", tbNguoi.IdGioiTinh);
                ViewData["IdKhungNangLucNgoaiNguc"] = new SelectList(await ApiServices_.GetAll<DmKhungNangLucNgoaiNgu>("/api/dm/KhungNangLucNgoaiNgu"), "IdKhungNangLucNgoaiNgu", "TenKhungNangLucNgoaiNgu", tbNguoi.IdKhungNangLucNgoaiNguc);
                ViewData["IdNgoaiNgu"] = new SelectList(await ApiServices_.GetAll<DmNgoaiNgu>("/api/dm/NgoaiNgu"), "IdNgoaiNgu", "NgoaiNgu", tbNguoi.IdNgoaiNgu);
                ViewData["IdQuocTich"] = new SelectList(await ApiServices_.GetAll<DmQuocTich>("/api/dm/QuocTich"), "IdQuocTich", "TenNuoc", tbNguoi.IdQuocTich);
                ViewData["IdThuongBinhHang"] = new SelectList(await ApiServices_.GetAll<DmHangThuongBinh>("/api/dm/HangThuongBinh"), "IdHangThuongBinh", "HangThuongBinh", tbNguoi.IdThuongBinhHang);
                ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao", tbNguoi.IdTrinhDoDaoTao);
                ViewData["IdTrinhDoLyLuanChinhTri"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoLyLuanChinhTri>("/api/dm/TrinhDoLyLuanChinhTri"), "IdTrinhDoLyLuanChinhTri", "TenTrinhDoLyLuanChinhTri", tbNguoi.IdTrinhDoLyLuanChinhTri);
                ViewData["IdTrinhDoQuanLyNhaNuoc"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoQuanLyNhaNuoc>("/api/dm/TrinhDoQuanLyNhaNuoc"), "IdTrinhDoQuanLyNhaNuoc", "TrinhDoQuanLyNhaNuoc", tbNguoi.IdTrinhDoQuanLyNhaNuoc);
                ViewData["IdTrinhDoTinHoc"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoTinHoc>("/api/dm/TrinhDoTinHoc"), "IdTrinhDoTinHoc", "TrinhDoTinHoc", tbNguoi.IdTrinhDoTinHoc);
                return View(tbNguoi);
            }
            catch (Exception ex)
            {
                export_message(ex.Message + "\n" + (ex.InnerException == null ? "" : ex.InnerException.ToString()));
                return BadRequest();
            }
        }

        // GET: Nguoi/Delete/5
        //Xóa một người khỏi database
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
                var tbNguoi = tbNguois.FirstOrDefault(m => m.IdNguoi == id);
                if (tbNguoi == null)
                {
                    return NotFound();
                }

                return View(tbNguoi);
            }
            catch (Exception ex)
            {
                export_message(ex.Message + "\n" + (ex.InnerException == null ? "" : ex.InnerException.ToString()));
                return BadRequest();
            }
        }

        // POST: Nguoi/Delete/5
        [HttpPost, ActionName("Delete")]//ActionName dùng để định dạng tên action
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbNguoi>("/api/Nguoi", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        //Kiểm tra xem có tồn tại id hay không nếu có return true ngược lại return false
        private async Task<bool> TbNguoiExists(int id)
        {
            var tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            return tbNguois.Any(e => e.IdNguoi == id);
        }
        //Kiểm tra giá trị

        //private void check_null(TbNguoi tbNguoi)
        //{
        //    if (tbNguoi.Ho == null) ModelState.AddModelError("Ho", "Vui lòng nhập họ!");
        //    if (tbNguoi.Ten == null) ModelState.AddModelError("Ten", "Vui lòng nhập tên!");
        //    if (tbNguoi.IdQuocTich == null) ModelState.AddModelError("IdQuocTich", "Không được bỏ trống!");
        //    if (tbNguoi.IdDanToc == null) ModelState.AddModelError("IdDanToc", "Không được bỏ trống!");
        //    if (tbNguoi.IdChucDanhKhoaHoc == null) ModelState.AddModelError("IdChucDanhKhoaHoc", "Không được bỏ trống!");
        //    if (tbNguoi.IdChuyenMonDaoTao == null) ModelState.AddModelError("IdChuyenMonDaoTao", "Không được bỏ trống!");
        //    if (tbNguoi.IdTonGiao == null) ModelState.AddModelError("IdTonGiao", "Không được bỏ trống!");
        //    if (tbNguoi.IdGioiTinh == null) ModelState.AddModelError("IdGioiTinh", "Không được bỏ trống!");
        //    if (tbNguoi.IdKhungNangLucNgoaiNguc == null) ModelState.AddModelError("IdKhungNangLucNgoaiNguc", "Không được bỏ trống!");
        //    if (tbNguoi.IdNgoaiNgu == null) ModelState.AddModelError("IdNgoaiNgu", "Không được bỏ trống!");
        //    if (tbNguoi.IdTrinhDoDaoTao == null) ModelState.AddModelError("IdTrinhDoDaoTao", "Không được bỏ trống!");
        //    if (tbNguoi.IdTrinhDoLyLuanChinhTri == null) ModelState.AddModelError("IdTrinhDoLyLuanChinhTri", "Không được bỏ trống!");
        //    if (tbNguoi.IdTrinhDoQuanLyNhaNuoc == null) ModelState.AddModelError("IdTrinhDoQuanLyNhaNuoc", "Không được bỏ trống!");
        //    if (tbNguoi.IdTrinhDoTinHoc == null) ModelState.AddModelError("IdTrinhDoTinHoc", "Không được bỏ trống!");
        //    if (tbNguoi.IdThuongBinhHang == null) ModelState.AddModelError("IdThuongBinhHang", "Không được bỏ trống!");
        //    if (tbNguoi.IdGiaDinhChinhSach == null) ModelState.AddModelError("IdGiaDinhChinhSach", "Không được bỏ trống!");
        //}

        //Tạo các select_list
       
        //In lỗi ra file
        private void export_message(string Message)
        {
            //Thêm tên để setup cho dễ
            string ten_folder_view = "Nguoi";
            // In lỗi vào file
            //Tạo UTF-8 encoding để encode ký tự về UTF-8
            UTF8Encoding unicode = new UTF8Encoding();
            //Tên của file để in lỗi
            string filename = Path.Combine(Environment.CurrentDirectory, $"Views/{ten_folder_view}/error.txt");
            //Tạo dãy byte để in vào filestream
            byte[] bytes = unicode.GetBytes(Message);
            //Mở file
            using (FileStream file = new FileStream(filename, FileMode.OpenOrCreate))
            {
                //Xóa nội dung của file
                file.SetLength(0);
                //In lỗi vào file
                file.Write(bytes, 0, bytes.Length);
                //Đóng file
                file.Close();
            }


        }



        //Import Excel 
        public IActionResult Excel(string json)
        {
            try
            {
                List<List<string>> data = JsonConvert.DeserializeObject<List<List<string>>>(json);
                Console.WriteLine(JsonConvert.SerializeObject(data));
                return Accepted(Json(new { msg = JsonConvert.SerializeObject(data) }));
            }
            catch (Exception ex)
            {
                return BadRequest(Json(new { msg = "Lỗi nè mấy má !!!!!!!!" }));
            }

        }
    }
}

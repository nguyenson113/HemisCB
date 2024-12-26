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
using Newtonsoft.Json;
using System.Globalization;

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

            //Tạo danh sách CanBo 
            List<TbCanBo> tbCanBos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");

            //Tạo danh sách lấy dữ liệu các biến từ APIHemis 
            //Tạo danh sách lấy dữ liệu từ APIHemis từ danh mục DmChucDanhGiangVien 
            List<DmChucDanhGiangVien> dmchucDanhGiangViens = await ApiServices_.GetAll<DmChucDanhGiangVien>("/api/dm/ChucDanhGiangVien");
            //Tạo danh sách lấy dữ liệu từ APIHemis từ danh mục DmChucDanhNgheNghiep 
            List<DmChucDanhNgheNghiep> dmchucDanhNgheNghieps = await ApiServices_.GetAll<DmChucDanhNgheNghiep>("/api/dm/ChucDanhNgheNghiep");
            //Tạo danh sách lấy dữ liệu từ APIHemis từ danh mục DmNckh  
            List<DmChucDanhNckh> dmchucDanhNCKHs = await ApiServices_.GetAll<DmChucDanhNckh>("/api/dm/ChucDanhNCKH");
            //Tạo danh sách lấy dữ liệu từ APIHemis từ danh mục DmChucVu 
            List<DmChucVu> dmchucVus = await ApiServices_.GetAll<DmChucVu>("/api/dm/ChucVu");
            //Tạo danh sách lấy dữ liệu từ APIHemis từ danh mục DmHuyen
            List<DmHuyen> dmHuyens = await ApiServices_.GetAll<DmHuyen>("/api/dm/Huyen");
            //Tạo danh sách lấy dữ liệu từ APIHemis từ danh mục DmXa 
            List<DmXa> dmXas = await ApiServices_.GetAll<DmXa>("/api/dm/Xa");
            //Tạo danh sách lấy dữ liệu từ APIHemis từ danh mục DmTinh 
            List<DmTinh> dmTinhs = await ApiServices_.GetAll<DmTinh>("/api/dm/Tinh");
            //Tạo danh sách lấy dữ liệu từ APIHemis từ danh mục DmNgach 
            List<DmNgach> dmNgachs = await ApiServices_.GetAll<DmNgach>("/api/dm/Ngach");
            //Tạo danh sách lấy dữ liệu từ APIHemis từ danh mục TbNguoi 
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            //Tạo danh sách lấy dữ liệu từ APIHemis từ danh mục DmTrangThaiCanBo 
            List<DmTrangThaiCanBo> dmtrangThaiCanBos = await ApiServices_.GetAll<DmTrangThaiCanBo>("/api/dm/TrangThaiCanBo");
            tbCanBos.ForEach(item => {
                //Trả dữ liệu lấy từ APIHemis vào các biến được sử dụng trong bảng
               
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

        public async Task<IActionResult> Statistics()
        {
            List<TbCanBo> getall = await TbCanBos();
            return View(getall);
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
                return BadRequest();//Trả về HTTP 400 nếu có lỗi xảy ra 
            }
        }

        // GET: CanBo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
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
            catch (Exception ex)
            {
                return BadRequest();//Trả về HTTP 400 nếu có lỗi xảy ra 

            }
        }


        // GET: CanBo/Create
     

        public async Task<IActionResult> Create()
        {
            //Tạo danh sách lựa chọn cho IdChucDanhGiangVien 
            ViewData["IdChucDanhGiangVien"] = new SelectList(await ApiServices_.GetAll<DmChucDanhGiangVien>("/api/dm/ChucDanhGiangVien"), "IdChucDanhGiangVien", "ChucDanhGiangVien");
            //Tạo danh sách lựa chọn cho IdChucDanhNgheNghiep
            ViewData["IdChucDanhNgheNghiep"] = new SelectList(await ApiServices_.GetAll<DmChucDanhNgheNghiep>("/api/dm/ChucDanhNgheNghiep"), "IdChucDanhNgheNghiep", "ChucDanhNgheNghiep");
            //Tạo danh sách lựa chọn cho IdChucDanhNGhienCuuKhoaHoc
            ViewData["IdChucDanhNghienCuuKhoaHoc"] = new SelectList(await ApiServices_.GetAll<DmChucDanhNckh>("/api/dm/ChucDanhNCKH"), "IdChucDanhNghienCuuKhoaHoc", "ChucDanhNghienCuuKhoaHoc");
            //Tạo danh sách lựa chọn cho IdChucVuCongTac
            ViewData["IdChucVuCongTac"] = new SelectList(await ApiServices_.GetAll<DmChucVu>("/api/dm/ChucVu"), "IdChucVu", "ChucVu");
            //Tạo danh sách lựa chọn cho IdHuyen
            ViewData["IdHuyen"] = new SelectList(await ApiServices_.GetAll<DmHuyen>("/api/dm/Huyen"), "IdHuyen", "TenHuyen");
            //Tạo danh sách lựa chọn cho IdNgach 
            ViewData["IdNgach"] = new SelectList(await ApiServices_.GetAll<DmNgach>("/api/dm/Ngach"), "IdNgach", "Ngach");
            //Tạo danh sách lựa chọn cho IdNguoi 
            ViewData["IdNguoi"] = new SelectList(await ApiServices_.GetAll<TbNguoi>("/api/Nguoi"), "IdNguoi", "IdNguoi");
            //Tạo danh sách lựa chọn cho IdTrangThaiLamViec 
            ViewData["IdTrangThaiLamViec"] = new SelectList(await ApiServices_.GetAll<DmTrangThaiCanBo>("/api/dm/TrangThaiCanBo"), "IdTrangThaiCanBo", "TrangThaiCanBo");
            //Tạo danh sách lựa chọn cho IdXa 
            ViewData["IdXa"] = new SelectList(await ApiServices_.GetAll<DmXa>("/api/dm/Xa"), "IdXa", "TenXa");
            //Tạo danh sách lựa chọn cho IdTinh
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
            try
            {
                //Áp dụng throw và hiển thị ra theo BadRequest(bat_loi.Message);
                //throw new Exception("Lỗi rồi bạn ơi! Mã quản lí nhiệm vụ bạn vừa nhập đã tồn tại!!!Nhập lại nhé!");
                if (await TbCanBoExists(tbCanBo.IdCanBo)) ModelState.AddModelError("IdCanBo", "ID này đã tồn tại!");
                if (ModelState.IsValid)
                {
                    await ApiServices_.Create<TbCanBo>("/api/cb/CanBo", tbCanBo);
                    return RedirectToAction(nameof(Index));
                }
                //Tạo danh sách lựa chọn cho IdChucDanhGiangVien và lưu dữ liệu vào biến 
                ViewData["IdChucDanhGiangVien"] = new SelectList(await ApiServices_.GetAll<DmChucDanhGiangVien>("/api/dm/ChucDanhGiangVien"), "IdChucDanhGiangVien", "ChucDanhGiangVien", tbCanBo.IdChucDanhGiangVien);
                //Tạo danh sách lựa chọn cho IdChucDanhNgheNghiep và lưu dữ liệu vào biến
                ViewData["IdChucDanhNgheNghiep"] = new SelectList(await ApiServices_.GetAll<DmChucDanhNgheNghiep>("/api/dm/ChucDanhNgheNghiep"), "IdChucDanhNgheNghiep", "ChucDanhNgheNghiep", tbCanBo.IdChucDanhNgheNghiep);
                //Tạo danh sách lựa chọn cho IdChucDanhNgheNghiep và lưu dữ liệu vào biến 
                ViewData["IdChucDanhNghienCuuKhoaHoc"] = new SelectList(await ApiServices_.GetAll<DmChucDanhNckh>("/api/dm/ChucDanhNCKH"), "IdChucDanhNghienCuuKhoaHoc", "ChucDanhNghienCuuKhoaHoc", tbCanBo.IdChucDanhNghienCuuKhoaHoc);
                //Tạo danh sách lựa chọn cho IdChucVuCongTac và lưu dữ liệu vào biến 
                ViewData["IdChucVuCongTac"] = new SelectList(await ApiServices_.GetAll<DmChucVu>("/api/dm/ChucVu"), "IdChucVu", "ChucVu", tbCanBo.IdChucVuCongTac);
                //Tạo danh sách lựa chọn cho IdHuyen và lưu dữ liệu vào biến 
                ViewData["IdHuyen"] = new SelectList(await ApiServices_.GetAll<DmHuyen>("/api/dm/Huyen"), "IdHuyen", "TenHuyen", tbCanBo.IdHuyen);
                //Tạo danh sách lựa chọn cho IdNgach và lưu dữ liệu vào biến
                ViewData["IdNgach"] = new SelectList(await ApiServices_.GetAll<DmNgach>("/api/dm/Ngach"), "IdNgach", "Ngach", tbCanBo.IdNgach);
                //Tạo danh sách lựa chọn cho IdNguoi và lưu dữ liệu vào biến
                ViewData["IdNguoi"] = new SelectList(await ApiServices_.GetAll<TbNguoi>("/api/Nguoi"), "IdNguoi", "IdNguoi", tbCanBo.IdNguoi);
                //Tạo danh sách lựa chọn cho IdTinh và lưu dữ liệu vào biến 
                ViewData["IdTinh"] = new SelectList(await ApiServices_.GetAll<DmTinh>("/api/dm/Tinh"), "IdTinh", "TenTinh", tbCanBo.IdTinh);
                //Tạo danh sách lựa chọn cho IdTrangThaiLamViec và lưu dữ liệu vào biến 
                ViewData["IdTrangThaiLamViec"] = new SelectList(await ApiServices_.GetAll<DmTrangThaiCanBo>("/api/dm/TrangThaiCanBo"), "IdTrangThaiCanBo", "TrangThaiCanBo", tbCanBo.IdTrangThaiLamViec);
                //Tạo danh sách lựa chọn cho IdXa và lưu dữ liệu vào biến
                ViewData["IdXa"] = new SelectList(await ApiServices_.GetAll<DmXa>("/api/dm/Xa"), "IdXa", "TenXa", tbCanBo.IdXa);
                return View(tbCanBo);
            }
            catch (Exception ex)
            {
                return BadRequest();
                ////Trả về thông báo hiển thị 
                //return BadRequest(ex.Message);

            }
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
            //Tạo danh sách lựa chọn cho IdChucDanhGiangVien và lưu dữ liệu vào biến 
            ViewData["IdChucDanhGiangVien"] = new SelectList(await ApiServices_.GetAll<DmChucDanhGiangVien>("/api/dm/ChucDanhGiangVien"), "IdChucDanhGiangVien", "ChucDanhGiangVien", tbCanBo.IdChucDanhGiangVien);
            //Tạo danh sách lựa chọn cho IdChucDanhNgheNghiep và lưu dữ liệu vào biến
            ViewData["IdChucDanhNgheNghiep"] = new SelectList(await ApiServices_.GetAll<DmChucDanhNgheNghiep>("/api/dm/ChucDanhNgheNghiep"), "IdChucDanhNgheNghiep", "ChucDanhNgheNghiep", tbCanBo.IdChucDanhNgheNghiep);
            //Tạo danh sách lựa chọn cho IdChucDanhNghienCuuKhoaHoc  và lưu dữ liệu vào biến 
            ViewData["IdChucDanhNghienCuuKhoaHoc"] = new SelectList(await ApiServices_.GetAll<DmChucDanhNckh>("/api/dm/ChucDanhNCKH"), "IdChucDanhNghienCuuKhoaHoc", "ChucDanhNghienCuuKhoaHoc", tbCanBo.IdChucDanhNghienCuuKhoaHoc);
              //Tạo danh sách lựa chọn cho IdHuyen và lưu dữ liệu vào biến 
            ViewData["IdChucVuCongTac"] = new SelectList(await ApiServices_.GetAll<DmChucVu>("/api/dm/ChucVu"), "IdChucVu", "ChucVu", tbCanBo.IdChucVuCongTac);
            //Tạo danh sách lựa chọn cho IdHuyen và lưu dữ liệu vào biến 
            ViewData["IdHuyen"] = new SelectList(await ApiServices_.GetAll<DmHuyen>("/api/dm/Huyen"), "IdHuyen", "TenHuyen", tbCanBo.IdHuyen);
            //Tạo danh sách lựa chọn cho IdNgach và lưu dữ liệu vào biến 
            ViewData["IdNgach"] = new SelectList(await ApiServices_.GetAll<DmNgach>("/api/dm/Ngach"), "IdNgach", "Ngach", tbCanBo.IdNgach);
            //Tạo danh sách lựa chọn cho IdNguoi và lưu dữ liệu vào biến 
            ViewData["IdNguoi"] = new SelectList(await ApiServices_.GetAll<TbNguoi>("/api/Nguoi"), "IdNguoi", "IdNguoi", tbCanBo.IdNguoi);
            //Tạo danh sách lựa chọn cho IdTinh  và lưu dữ liệu vào biến 
            ViewData["IdTinh"] = new SelectList(await ApiServices_.GetAll<DmTinh>("/api/dm/Tinh"), "IdTinh", "TenTinh", tbCanBo.IdTinh);
            //Tạo danh sách lựa chọn cho IdTrangThaiLamViec  và lưu dữ liệu vào biến 
            ViewData["IdTrangThaiLamViec"] = new SelectList(await ApiServices_.GetAll<DmTrangThaiCanBo>("/api/dm/TrangThaiCanBo"), "IdTrangThaiCanBo", "TrangThaiCanBo", tbCanBo.IdTrangThaiLamViec);
            //Tạo danh sách lựa chọn cho IdXa và lưu dữ liệu vào biến 
            ViewData["IdXa"] = new SelectList(await ApiServices_.GetAll<DmXa>("/api/dm/Xa"), "IdXa", "TenXa", tbCanBo.IdXa);
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
            ViewData["IdChucDanhGiangVien"] = new SelectList(await ApiServices_.GetAll<DmChucDanhGiangVien>("/api/dm/ChucDanhGiangVien"), "IdChucDanhGiangVien", "ChucDanhGiangVien", tbCanBo.IdChucDanhGiangVien);
            ViewData["IdChucDanhNgheNghiep"] = new SelectList(await ApiServices_.GetAll<DmChucDanhNgheNghiep>("/api/dm/ChucDanhNgheNghiep"), "IdChucDanhNgheNghiep", "ChucDanhNgheNghiep", tbCanBo.IdChucDanhNgheNghiep);
            ViewData["IdChucDanhNghienCuuKhoaHoc"] = new SelectList(await ApiServices_.GetAll<DmChucDanhNckh>("/api/dm/ChucDanhNCKH"), "IdChucDanhNghienCuuKhoaHoc", "ChucDanhNghienCuuKhoaHoc", tbCanBo.IdChucDanhNghienCuuKhoaHoc);
            ViewData["IdChucVuCongTac"] = new SelectList(await ApiServices_.GetAll<DmChucVu>("/api/dm/ChucVu"), "IdChucVu", "ChucVu", tbCanBo.IdChucVuCongTac);
            ViewData["IdHuyen"] = new SelectList(await ApiServices_.GetAll<DmHuyen>("/api/dm/Huyen"), "IdHuyen", "TenHuyen", tbCanBo.IdHuyen);
            ViewData["IdNgach"] = new SelectList(await ApiServices_.GetAll<DmNgach>("/api/dm/Ngach"), "IdNgach", "Ngach", tbCanBo.IdNgach);
            ViewData["IdNguoi"] = new SelectList(await ApiServices_.GetAll<TbNguoi>("/api/Nguoi"), "IdNguoi", "IdNguoi", tbCanBo.IdNguoi);
            ViewData["IdTinh"] = new SelectList(await ApiServices_.GetAll<DmTinh>("/api/dm/Tinh"), "IdTinh", "TenTinh", tbCanBo.IdTinh);
            ViewData["IdTrangThaiLamViec"] = new SelectList(await ApiServices_.GetAll<DmTrangThaiCanBo>("/api/dm/TrangThaiCanBo"), "IdTrangThaiCanBo", "TrangThaiCanBo", tbCanBo.IdTrangThaiLamViec);
            ViewData["IdXa"] = new SelectList(await ApiServices_.GetAll<DmXa>("/api/dm/Xa"), "IdXa", "TenXa", tbCanBo.IdXa);
            return View(tbCanBo);
        }

        // GET: CanBo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var tbCanBos = await TbCanBos();
                var tbCanBo = tbCanBos.FirstOrDefault(m => m.IdCanBo == id);
                if (tbCanBo == null)
                {
                    return NotFound();
                }

                return View(tbCanBo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); //Trả về báo lỗi nếu có 
            }
            
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


        //Import Excel 
        public async Task<IActionResult> Receive(string json)
        {
            try
            {
                // Khai báo thông báo mặc định
                var message = "Không phát hiện lỗi";
                // Giải mã dữ liệu JSON từ client
                List<List<string>> data = JsonConvert.DeserializeObject<List<List<string>>>(json);

                List<TbCanBo> lst = new List<TbCanBo>();

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();
                List<TbCanBo> TbCanBos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    TbCanBo model = new TbCanBo();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (lst.Any(t => t.IdCanBo == id) || TbCanBos.Any(t => t.IdCanBo == id)); // Kiểm tra id có tồn tại không

                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdCanBo = id; // Gán ID
                    model.NgayChuyenTrangThai = DateOnly.ParseExact(item[1], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.CoQuanCongTac = item[0];
                    model.IdChucVuCongTac = ParseInt(item[2]);
                    model.IdTinh = ParseInt(item[3]);
                    model.IdTrangThaiLamViec = ParseInt(item[4]);
                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu (giả sử có một phương thức tạo đối tượng trong DB)
                foreach (var item in lst)
                {
                    await CreateTbCanBo(item); // Giả sử có phương thức tạo dữ liệu vào DB
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về thông báo lỗi
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbCanBo(TbCanBo item)
        {
            await ApiServices_.Create<TbCanBo>("/api/cb/CanBo", item);
        }

        private int? ParseInt(string v)
        {
            if (int.TryParse(v, out int result)) // Nếu chuỗi có thể chuyển thành int
            {
                return result; // Trả về giá trị int
            }
            else
            {
                return null; // Nếu không thể chuyển thành int, trả về null
            }
        }

    }
}

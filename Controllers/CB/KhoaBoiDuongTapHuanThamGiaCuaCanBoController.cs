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
    public class KhoaBoiDuongTapHuanThamGiaCuaCanBoController : Controller
    {
        private readonly ApiServices ApiServices_;

        public KhoaBoiDuongTapHuanThamGiaCuaCanBoController(ApiServices services)
        {
            ApiServices_ = services;
        }

        //============================TẠO DANH SÁCH LẤY API========================
        private async Task<List<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo>> TbKhoaBoiDuongTapHuanThamGiaCuaCanBos()
        {
            List<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo> tbKhoaBoiDuongTapHuanThamGiaCuaCanBos = await ApiServices_.GetAll<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo>("/api/cb/KhoaBoiDuongTapHuanThamGiaCuaCanBo");
            List<DmLoaiBoiDuong> dmloaiBoiDuongs = await ApiServices_.GetAll<DmLoaiBoiDuong>("/api/dm/LoaiBoiDuong");
            List<DmNguonKinhPhi> dmnguonKinhPhis = await ApiServices_.GetAll<DmNguonKinhPhi>("/api/dm/NguonKinhPhi");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            tbKhoaBoiDuongTapHuanThamGiaCuaCanBos.ForEach(item => {
                item.IdLoaiBoiDuongNavigation = dmloaiBoiDuongs.FirstOrDefault(x => x.IdLoaiBoiDuong == item.IdLoaiBoiDuong);
                item.IdNguonKinhPhiNavigation = dmnguonKinhPhis.FirstOrDefault(x => x.IdNguonKinhPhi == item.IdNguonKinhPhi);
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                if (item.IdCanBoNavigation != null)
                    item.IdCanBoNavigation.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdCanBoNavigation.IdNguoi);
            });
            return tbKhoaBoiDuongTapHuanThamGiaCuaCanBos;
        }

        private async Task<List<TbCanBo>> TbCanBos()
        {
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            tbcanbos.ForEach(item => {
                item.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdNguoi);

            });

            return tbcanbos;
        }


        public async Task<IActionResult> Statistics()
        {
            List<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo> getall = await TbKhoaBoiDuongTapHuanThamGiaCuaCanBos();
            return View(getall);
        }


        // GET: KhoaBoiDuongTapHuanThamGiaCuaCanBo
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo> getall = await TbKhoaBoiDuongTapHuanThamGiaCuaCanBos();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: KhoaBoiDuongTapHuanThamGiaCuaCanBo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbKhoaBoiDuongTapHuanThamGiaCuaCanBos = await TbKhoaBoiDuongTapHuanThamGiaCuaCanBos();
                var tbKhoaBoiDuongTapHuanThamGiaCuaCanBo = tbKhoaBoiDuongTapHuanThamGiaCuaCanBos.FirstOrDefault(m => m.IdKhoaBoiDuongTapHuanThamGiaCuaCanBo == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbKhoaBoiDuongTapHuanThamGiaCuaCanBo == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbKhoaBoiDuongTapHuanThamGiaCuaCanBo);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        // GET: KhoaBoiDuongTapHuanThamGiaCuaCanBo/Create
        //sửa các selectlist 
       

        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name");
                ViewData["IdLoaiBoiDuong"] = new SelectList(await ApiServices_.GetAll<DmLoaiBoiDuong>("/api/dm/LoaiBoiDuong"), "IdLoaiBoiDuong", "LoaiBoiDuong");
                ViewData["IdNguonKinhPhi"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhi>("/api/dm/NguonKinhPhi"), "IdNguonKinhPhi", "NguonKinhPhi");
              
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }


        // POST: KhoaBoiDuongTapHuanThamGiaCuaCanBo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdKhoaBoiDuongTapHuanThamGiaCuaCanBo,IdCanBo,TenKhoaBoiDuongTapHuan,DonViToChuc,IdLoaiBoiDuong,DiaDiemToChuc,ThoiGianBatDau,ThoiGianKetThuc,IdNguonKinhPhi,ChungChi,NgayCap")] TbKhoaBoiDuongTapHuanThamGiaCuaCanBo tbKhoaBoiDuongTapHuanThamGiaCuaCanBo)
        {
            if (await TbKhoaBoiDuongTapHuanThamGiaCuaCanBoExists(tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdKhoaBoiDuongTapHuanThamGiaCuaCanBo)) ModelState.AddModelError("IdKhoaBoiDuongTapHuanThamGiaCuaCanBo", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo>("/api/cb/KhoaBoiDuongTapHuanThamGiaCuaCanBo", tbKhoaBoiDuongTapHuanThamGiaCuaCanBo);
                return RedirectToAction(nameof(Index));
            }
            try
            {
               
                ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdCanBo);
                ViewData["IdLoaiBoiDuong"] = new SelectList(await ApiServices_.GetAll<DmLoaiBoiDuong>("/api/dm/LoaiBoiDuong"), "IdLoaiBoiDuong", "LoaiBoiDuong", tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdLoaiBoiDuong);
                ViewData["IdNguonKinhPhi"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhi>("/api/dm/NguonKinhPhi"), "IdNguonKinhPhi", "NguonKinhPhi", tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdNguonKinhPhi);
                return View(tbKhoaBoiDuongTapHuanThamGiaCuaCanBo);
            }catch(Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: KhoaBoiDuongTapHuanThamGiaCuaCanBo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var tbKhoaBoiDuongTapHuanThamGiaCuaCanBo = await ApiServices_.GetId<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo>("/api/cb/KhoaBoiDuongTapHuanThamGiaCuaCanBo", id ?? 0);
                if (tbKhoaBoiDuongTapHuanThamGiaCuaCanBo == null)
                {
                    return NotFound();
                }
                ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name", tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdCanBo);
                ViewData["IdLoaiBoiDuong"] = new SelectList(await ApiServices_.GetAll<DmLoaiBoiDuong>("/api/dm/LoaiBoiDuong"), "IdLoaiBoiDuong", "LoaiBoiDuong", tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdLoaiBoiDuong);
                ViewData["IdNguonKinhPhi"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhi>("/api/dm/NguonKinhPhi"), "IdNguonKinhPhi", "NguonKinhPhi", tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdNguonKinhPhi);
                return View(tbKhoaBoiDuongTapHuanThamGiaCuaCanBo);
            }catch(Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: KhoaBoiDuongTapHuanThamGiaCuaCanBo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdKhoaBoiDuongTapHuanThamGiaCuaCanBo,IdCanBo,TenKhoaBoiDuongTapHuan,DonViToChuc,IdLoaiBoiDuong,DiaDiemToChuc,ThoiGianBatDau,ThoiGianKetThuc,IdNguonKinhPhi,ChungChi,NgayCap")] TbKhoaBoiDuongTapHuanThamGiaCuaCanBo tbKhoaBoiDuongTapHuanThamGiaCuaCanBo)
        {
            try
            {
                if (id != tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdKhoaBoiDuongTapHuanThamGiaCuaCanBo)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo>("/api/cb/KhoaBoiDuongTapHuanThamGiaCuaCanBo", id, tbKhoaBoiDuongTapHuanThamGiaCuaCanBo);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbKhoaBoiDuongTapHuanThamGiaCuaCanBoExists(tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdKhoaBoiDuongTapHuanThamGiaCuaCanBo) == false)
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
                ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdCanBo);
                ViewData["IdLoaiBoiDuong"] = new SelectList(await ApiServices_.GetAll<DmLoaiBoiDuong>("/api/dm/LoaiBoiDuong"), "IdLoaiBoiDuong", "LoaiBoiDuong", tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdLoaiBoiDuong);
                ViewData["IdNguonKinhPhi"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhi>("/api/dm/NguonKinhPhi"), "IdNguonKinhPhi", "NguonKinhPhi", tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdNguonKinhPhi);
                return View(tbKhoaBoiDuongTapHuanThamGiaCuaCanBo);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
            }

        // GET: KhoaBoiDuongTapHuanThamGiaCuaCanBo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbKhoaBoiDuongTapHuanThamGiaCuaCanBos = await TbKhoaBoiDuongTapHuanThamGiaCuaCanBos();
            var tbKhoaBoiDuongTapHuanThamGiaCuaCanBo = tbKhoaBoiDuongTapHuanThamGiaCuaCanBos.FirstOrDefault(m => m.IdKhoaBoiDuongTapHuanThamGiaCuaCanBo == id);
            if (tbKhoaBoiDuongTapHuanThamGiaCuaCanBo == null)
            {
                return NotFound();
            }

            return View(tbKhoaBoiDuongTapHuanThamGiaCuaCanBo);
        }

        // POST: KhoaBoiDuongTapHuanThamGiaCuaCanBo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo>("/api/cb/KhoaBoiDuongTapHuanThamGiaCuaCanBo", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        private async Task<bool> TbKhoaBoiDuongTapHuanThamGiaCuaCanBoExists(int id)
        {
            var tbKhoaBoiDuongTapHuanThamGiaCuaCanBos = await ApiServices_.GetAll<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo>("/api/cb/KhoaBoiDuongTapHuanThamGiaCuaCanBo");
            return tbKhoaBoiDuongTapHuanThamGiaCuaCanBos.Any(e => e.IdKhoaBoiDuongTapHuanThamGiaCuaCanBo == id);
        }


        //===========================================================Import Excel===================================================
        public async Task<IActionResult> Receive(string json)
        {
            try
            {
                // Khai báo thông báo mặc định
                var message = "Không phát hiện lỗi";
                // Giải mã dữ liệu JSON từ client
                List<List<string>> data = JsonConvert.DeserializeObject<List<List<string>>>(json);

                List<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo> lst = new List<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo>();

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();
                List<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo> TbKhoaBoiDuongTapHuanThamGiaCuaCanBos = await ApiServices_.GetAll<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo>("/api/cb/KhoaBoiDuongTapHuanThamGiaCuaCanBo");
                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    TbKhoaBoiDuongTapHuanThamGiaCuaCanBo model = new TbKhoaBoiDuongTapHuanThamGiaCuaCanBo();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (lst.Any(t => t.IdKhoaBoiDuongTapHuanThamGiaCuaCanBo == id) || TbKhoaBoiDuongTapHuanThamGiaCuaCanBos.Any(t => t.IdKhoaBoiDuongTapHuanThamGiaCuaCanBo == id)); // Kiểm tra id có tồn tại không

                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdKhoaBoiDuongTapHuanThamGiaCuaCanBo = id; // Gán ID
                    model.TenKhoaBoiDuongTapHuan = item[0];
                    model.DonViToChuc = item[1];
                    model.DiaDiemToChuc = item[2];
                  
                    model.ThoiGianBatDau = DateOnly.ParseExact(item[3], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.ThoiGianKetThuc = DateOnly.ParseExact(item[4], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.ChungChi = item[5];
                    model.NgayCap = DateOnly.ParseExact(item[6], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.IdCanBo = ParseInt(item[7]);
                    model.IdLoaiBoiDuong= ParseInt(item[8]);
                    model.IdNguonKinhPhi = ParseInt(item[9]);
                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu (giả sử có một phương thức tạo đối tượng trong DB)
                foreach (var item in lst)
                {
                    await CreateTbKhoaBoiDuongTapHuanThamGiaCuaCanBo(item); // Giả sử có phương thức tạo dữ liệu vào DB
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về thông báo lỗi
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbKhoaBoiDuongTapHuanThamGiaCuaCanBo(TbKhoaBoiDuongTapHuanThamGiaCuaCanBo item)
        {
            await ApiServices_.Create<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo>("/api/cb/KhoaBoiDuongTapHuanThamGiaCuaCanBo", item);
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

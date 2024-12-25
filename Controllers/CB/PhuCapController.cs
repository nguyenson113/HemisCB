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
    public class PhuCapController : Controller
    {
        private readonly ApiServices ApiServices_;

        public PhuCapController(ApiServices services)
        {
            ApiServices_ = services;
        }


        //============================TẠO DANH SÁCH LẤY API========================
        private async Task<List<TbPhuCap>> TbPhuCaps()
        {
            List<TbPhuCap> tbPhuCaps = await ApiServices_.GetAll<TbPhuCap>("/api/CSGD/PhuCap");
            List<DmBacLuong1> dmbacLuong1s = await ApiServices_.GetAll<DmBacLuong1>("/api/dm/BacLuong");
            List<DmHeSoLuong> dmheSoLuongs = await ApiServices_.GetAll<DmHeSoLuong>("/api/dm/HeSoLuong");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            tbPhuCaps.ForEach(item => {
                //item.IdBacLuongNavigation = dmbacLuong1s.FirstOrDefault(x => x.IdBacLuong == item.IdBacLuong);
                item.IdHeSoLuongNavigation = dmheSoLuongs.FirstOrDefault(x => x.IdHeSoLuong == item.IdHeSoLuong);
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdCanBoNavigation.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdCanBoNavigation.IdNguoi);
            });
            return tbPhuCaps;
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
            List<TbPhuCap> getall = await TbPhuCaps();
            return View(getall);
        }



        // GET: PhuCap
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbPhuCap> getall = await TbPhuCaps();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        // GET: PhuCap/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbPhuCaps = await TbPhuCaps();
                var tbPhuCap = tbPhuCaps.FirstOrDefault(m => m.IdPhuCap == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbPhuCap == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbPhuCap);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        // GET: PhuCap/Create
   

        public async Task<IActionResult> Create()
        {
          
            //ViewData["IdBacLuong"] = new SelectList(await ApiServices_.GetAll<DmBacLuong>("/api/dm/BacLuong"), "IdBacLuong", "BacLuong");
            ViewData["IdHeSoLuong"] = new SelectList(await ApiServices_.GetAll<DmHeSoLuong>("/api/dm/HeSoLuong"), "IdHeSoLuong", "HeSoLuong");
            ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name");
            return View();
        }

        // POST: PhuCap/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPhuCap,IdCanBo,PhuCapThuHutNghe,PhuCapThamNien,PhuCapUuDaiNghe,PhuCapChucVu,PhuCapDocHai,PhuCapKhac,IdBacLuong,PhanTramVuotKhung,IdHeSoLuong,NgayThangNamHuongLuong")] TbPhuCap tbPhuCap)
        {

            if (await TbPhuCapExists(tbPhuCap.IdPhuCap)) ModelState.AddModelError("IdPhuCap", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbPhuCap>("/api/CSGD/PhuCap", tbPhuCap);
                return RedirectToAction(nameof(Index));
            }
            {
                //ViewData["IdBacLuong"] = new SelectList(await ApiServices_.GetAll<DmBacLuong>("/api/dm/BacLuong"), "IdBacLuong", "BacLuong", tbPhuCap.IdBacLuong);
                ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbPhuCap.IdCanBo);
                ViewData["IdHeSoLuong"] = new SelectList(await ApiServices_.GetAll<DmHeSoLuong>("/api/dm/HeSoLuong"), "IdHeSoLuong", "HeSoLuong", tbPhuCap.IdHeSoLuong);
                return View(tbPhuCap);
            }

            
        }
        // GET: PhuCap/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbPhuCap = await ApiServices_.GetId<TbPhuCap>("/api/CSGD/PhuCap", id ?? 0);
            if (tbPhuCap == null)
            {
                return NotFound();
            }
            //ViewData["IdBacLuong"] = new SelectList(await ApiServices_.GetAll<DmBacLuong>("/api/dm/BacLuong"), "IdBacLuong", "BacLuong", tbPhuCap.IdBacLuong);
            ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name", tbPhuCap.IdCanBo);
            ViewData["IdHeSoLuong"] = new SelectList(await ApiServices_.GetAll<DmHeSoLuong>("/api/dm/HeSoLuong"), "IdHeSoLuong", "HeSoLuong", tbPhuCap.IdHeSoLuong);
            return View(tbPhuCap);
        }

        // POST: PhuCap/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdPhuCap,IdCanBo,PhuCapThuHutNghe,PhuCapThamNien,PhuCapUuDaiNghe,PhuCapChucVu,PhuCapDocHai,PhuCapKhac,IdBacLuong,PhanTramVuotKhung,IdHeSoLuong,NgayThangNamHuongLuong")] TbPhuCap tbPhuCap)
        {
            if (id != tbPhuCap.IdPhuCap)
            {
                return NotFound();
            }

            // Kiểm tra xem ID đã tồn tại hay chưa (trừ ID hiện tại)
            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbPhuCap>("/api/CSGD/PhuCap", id, tbPhuCap);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbPhuCapExists(tbPhuCap.IdPhuCap) == false)
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
            //ViewData["IdBacLuong"] = new SelectList(await ApiServices_.GetAll<DmBacLuong>("/api/dm/BacLuong"), "IdBacLuong", "BacLuong", tbPhuCap.IdBacLuong);
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbPhuCap.IdCanBo);
            ViewData["IdHeSoLuong"] = new SelectList(await ApiServices_.GetAll<DmHeSoLuong>("/api/dm/HeSoLuong"), "IdHeSoLuong", "HeSoLuong", tbPhuCap.IdHeSoLuong);
            return View(tbPhuCap);
        }

        // GET: PhuCap/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbPhuCaps = await TbPhuCaps();
            var tbPhuCap = tbPhuCaps.FirstOrDefault(m => m.IdPhuCap == id);
            if (tbPhuCap == null)
            {
                return NotFound();
            }

            return View(tbPhuCap);
        }

        // POST: PhuCap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbPhuCap>("/api/CSGD/PhuCap", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<bool> TbPhuCapExists(int id)
        {
            var tbPhuCaps = await ApiServices_.GetAll<TbPhuCap>("/api/CSGD/PhuCap");
            return tbPhuCaps.Any(e => e.IdPhuCap == id);
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

                List<TbPhuCap> lst = new List<TbPhuCap>();

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();
                List<TbPhuCap> TbPhuCaps = await ApiServices_.GetAll<TbPhuCap>("/api/cb/PhuCap");
                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    TbPhuCap model = new TbPhuCap();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (lst.Any(t => t.IdPhuCap == id) || TbPhuCaps.Any(t => t.IdPhuCap == id)); // Kiểm tra id có tồn tại không

                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdPhuCap = id; // Gán ID
                    model.NgayThangNamHuongLuong = DateOnly.ParseExact(item[2], "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    model.PhuCapThuHutNghe = ParseInt(item[1]);
                    model.IdCanBo = ParseInt(item[0]);
                    model.PhuCapThamNien = ParseInt(item[3]);
                    model.IdHeSoLuong = ParseInt(item[4]);
                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu (giả sử có một phương thức tạo đối tượng trong DB)
                foreach (var item in lst)
                {
                    await CreateTbPhuCap(item); // Giả sử có phương thức tạo dữ liệu vào DB
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về thông báo lỗi
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbPhuCap(TbPhuCap item)
        {
            await ApiServices_.Create<TbPhuCap>("/api/cb/PhuCap", item);
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
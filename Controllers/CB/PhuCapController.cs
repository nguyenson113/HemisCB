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
                item.IdBacLuongNavigation = dmbacLuong1s.FirstOrDefault(x => x.IdBacLuong == item.IdBacLuong);
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
          
            ViewData["IdBacLuong"] = new SelectList(await ApiServices_.GetAll<DmBacLuong>("/api/dm/BacLuong"), "IdBacLuong", "BacLuong");
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
                ViewData["IdBacLuong"] = new SelectList(await ApiServices_.GetAll<DmBacLuong>("/api/dm/BacLuong"), "IdBacLuong", "BacLuong", tbPhuCap.IdBacLuong);
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
            ViewData["IdBacLuong"] = new SelectList(await ApiServices_.GetAll<DmBacLuong>("/api/dm/BacLuong"), "IdBacLuong", "BacLuong", tbPhuCap.IdBacLuong);
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbPhuCap.IdCanBo);
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
            ViewData["IdBacLuong"] = new SelectList(await ApiServices_.GetAll<DmBacLuong>("/api/dm/BacLuong"), "IdBacLuong", "BacLuong", tbPhuCap.IdBacLuong);
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbPhuCap.IdCanBo);
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
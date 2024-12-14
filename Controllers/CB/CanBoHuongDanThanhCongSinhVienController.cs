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
    public class CanBoHuongDanThanhCongSinhVienController : Controller
    {
        private readonly ApiServices ApiServices_;

        public CanBoHuongDanThanhCongSinhVienController(ApiServices services)
        {
            ApiServices_ = services;
        }

        //================================== TẠO LIST LẤY DỮ LIỆU TỪ API ==============================
        private async Task<List<TbCanBoHuongDanThanhCongSinhVien>> TbCanBoHuongDanThanhCongSinhViens()
        {
            List<TbCanBoHuongDanThanhCongSinhVien> tbCanBoHuongDanThanhCongSinhViens = await ApiServices_.GetAll<TbCanBoHuongDanThanhCongSinhVien>("/api/cb/CanBoHuongDanThanhCongSinhVien");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
           
            tbCanBoHuongDanThanhCongSinhViens.ForEach(item => {
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
            });
            return tbCanBoHuongDanThanhCongSinhViens;
        }

        // GET: CanBoHuongDanThanhCongSinhVien
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbCanBoHuongDanThanhCongSinhVien> getall = await TbCanBoHuongDanThanhCongSinhViens();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: CanBoHuongDanThanhCongSinhVien/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbCanBoHuongDanThanhCongSinhViens = await TbCanBoHuongDanThanhCongSinhViens();
                var tbCanBoHuongDanThanhCongSinhVien = tbCanBoHuongDanThanhCongSinhViens.FirstOrDefault(m => m.IdCanBoHuongDanThanhCongSinhVien == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbCanBoHuongDanThanhCongSinhVien == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbCanBoHuongDanThanhCongSinhVien);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: CanBoHuongDanThanhCongSinhVien/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo");
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: CanBoHuongDanThanhCongSinhVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCanBoHuongDanThanhCongSinhVien,IdCanBo,IdSinhVien,TrachNhiemHuongDan,ThoiGianBatDau,ThoiGianKetThuc")] TbCanBoHuongDanThanhCongSinhVien tbCanBoHuongDanThanhCongSinhVien)
        {
            if (await TbCanBoHuongDanThanhCongSinhVienExists(tbCanBoHuongDanThanhCongSinhVien.IdCanBoHuongDanThanhCongSinhVien)) ModelState.AddModelError("IdCanBoHuongDanThanhCongSinhVien", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbCanBoHuongDanThanhCongSinhVien>("/api/cb/CanBoHuongDanThanhCongSinhVien", tbCanBoHuongDanThanhCongSinhVien);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbCanBoHuongDanThanhCongSinhVien.IdCanBo);
            return View(tbCanBoHuongDanThanhCongSinhVien);
        }

        // GET: CanBoHuongDanThanhCongSinhVien/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbCanBoHuongDanThanhCongSinhVien = await ApiServices_.GetId<TbCanBoHuongDanThanhCongSinhVien>("/api/cb/CanBoHuongDanThanhCongSinhVien", id ?? 0); 
            if (tbCanBoHuongDanThanhCongSinhVien == null)
            {
                return NotFound();
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbCanBoHuongDanThanhCongSinhVien.IdCanBo);
            return View(tbCanBoHuongDanThanhCongSinhVien);
        }

        // POST: CanBoHuongDanThanhCongSinhVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCanBoHuongDanThanhCongSinhVien,IdCanBo,IdSinhVien,TrachNhiemHuongDan,ThoiGianBatDau,ThoiGianKetThuc")] TbCanBoHuongDanThanhCongSinhVien tbCanBoHuongDanThanhCongSinhVien)
        {
            if (id != tbCanBoHuongDanThanhCongSinhVien.IdCanBoHuongDanThanhCongSinhVien)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbCanBoHuongDanThanhCongSinhVien>("/api/cb/CanBoHuongDanThanhCongSinhVien", id, tbCanBoHuongDanThanhCongSinhVien);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbCanBoHuongDanThanhCongSinhVienExists(tbCanBoHuongDanThanhCongSinhVien.IdCanBoHuongDanThanhCongSinhVien) == false)
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
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbCanBoHuongDanThanhCongSinhVien.IdCanBo);
            return View(tbCanBoHuongDanThanhCongSinhVien);
        }

        // GET: CanBoHuongDanThanhCongSinhVien/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }
                var tbCanBoHuongDanThanhCongSinhViens = await ApiServices_.GetAll<TbCanBoHuongDanThanhCongSinhVien>("/api/cb/CanBoHuongDanThanhCongSinhVien");
                var tbCanBoHuongDanThanhCongSinhVien = tbCanBoHuongDanThanhCongSinhViens.FirstOrDefault(m => m.IdCanBoHuongDanThanhCongSinhVien == id);
                if (tbCanBoHuongDanThanhCongSinhVien == null)
                {
                    return NotFound();
                }

                return View(tbCanBoHuongDanThanhCongSinhVien);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: CanBoHuongDanThanhCongSinhVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbCanBoHuongDanThanhCongSinhVien>("/api/cb/CanBoHuongDanThanhCongSinhVien", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        private async Task<bool> TbCanBoHuongDanThanhCongSinhVienExists(int id)
        {
            var tbCanBoHuongDanThanhCongSinhViens = await ApiServices_.GetAll<TbCanBoHuongDanThanhCongSinhVien>("/api/cb/CanBoHuongDanThanhCongSinhVien");
            return tbCanBoHuongDanThanhCongSinhViens.Any(e => e.IdCanBoHuongDanThanhCongSinhVien == id);
        }
    }
}

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
    public class DanhGiaXepLoaiCanBoController : Controller
    {
        private readonly ApiServices ApiServices_;

        public DanhGiaXepLoaiCanBoController(ApiServices services)
        {
            ApiServices_ = services;
        }

        //======================== TẠO LIST LẤY DỮ LIỆU TỪ API ============================
        private async Task<List<TbDanhGiaXepLoaiCanBo>> TbDanhGiaXepLoaiCanBos()
        {
            List<TbDanhGiaXepLoaiCanBo> tbDanhGiaXepLoaiCanBos = await ApiServices_.GetAll<TbDanhGiaXepLoaiCanBo>("/api/cb/DanhGiaXepLoaiCanBo");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<DmDanhGiaCongChucVienChuc> dmdanhGiaCongChucVienChucs = await ApiServices_.GetAll<DmDanhGiaCongChucVienChuc>("/api/dm/DanhGiaCongChucVienChuc");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            tbDanhGiaXepLoaiCanBos.ForEach(item => {
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdDanhGiaNavigation = dmdanhGiaCongChucVienChucs.FirstOrDefault(x => x.IdDanhGiaCongChucVienChuc == item.IdDanhGia);
                item.IdCanBoNavigation.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdCanBoNavigation.IdNguoi);
            });
            return tbDanhGiaXepLoaiCanBos;
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
            List<TbDanhGiaXepLoaiCanBo> getall = await TbDanhGiaXepLoaiCanBos();
            return View(getall);
        }

        // GET: DanhGiaXepLoaiCanBo


        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbDanhGiaXepLoaiCanBo> getall = await TbDanhGiaXepLoaiCanBos();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: DanhGiaXepLoaiCanBo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbDanhGiaXepLoaiCanBos = await TbDanhGiaXepLoaiCanBos();
                var tbDanhGiaXepLoaiCanBo = tbDanhGiaXepLoaiCanBos.FirstOrDefault(m => m.IdDanhGiaXepLoaiCanBo == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbDanhGiaXepLoaiCanBo == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbDanhGiaXepLoaiCanBo);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: DanhGiaXepLoaiCanBo/Create
       

        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name");
                ViewData["IdDanhGia"] = new SelectList(await ApiServices_.GetAll<DmDanhGiaCongChucVienChuc>("/api/dm/DanhGiaCongChucVienChuc"), "IdDanhGiaCongChucVienChuc", "DanhGiaCongChucVienChuc");
                
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: DanhGiaXepLoaiCanBo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDanhGiaXepLoaiCanBo,IdCanBo,IdDanhGia,NamDanhGia,NganhDuocKhenThuong")] TbDanhGiaXepLoaiCanBo tbDanhGiaXepLoaiCanBo)
        {
            if (await TbDanhGiaXepLoaiCanBoExists(tbDanhGiaXepLoaiCanBo.IdDanhGiaXepLoaiCanBo)) ModelState.AddModelError("IdDanhGiaXepLoaiCanBo", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbDanhGiaXepLoaiCanBo>("/api/cb/DanhGiaXepLoaiCanBo", tbDanhGiaXepLoaiCanBo);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdDanhGia"] = new SelectList(await ApiServices_.GetAll<DmDanhGiaCongChucVienChuc>("/api/dm/DanhGiaCongChucVienChuc"), "IdDanhGiaCongChucVienChuc", "DanhGiaCongChucVienChuc", tbDanhGiaXepLoaiCanBo.IdDanhGia);
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbDanhGiaXepLoaiCanBo.IdCanBo);
            return View(tbDanhGiaXepLoaiCanBo);
        }

        // GET: DanhGiaXepLoaiCanBo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbDanhGiaXepLoaiCanBo = await ApiServices_.GetId<TbDanhGiaXepLoaiCanBo>("/api/cb/DanhGiaXepLoaiCanBo", id ?? 0);
            if (tbDanhGiaXepLoaiCanBo == null)
            {
                return NotFound();
            }
            ViewData["IdDanhGia"] = new SelectList(await ApiServices_.GetAll<DmDanhGiaCongChucVienChuc>("/api/dm/DanhGiaCongChucVienChuc"), "IdDanhGiaCongChucVienChuc", "DanhGiaCongChucVienChuc", tbDanhGiaXepLoaiCanBo.IdDanhGia);
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbDanhGiaXepLoaiCanBo.IdCanBo);
            return View(tbDanhGiaXepLoaiCanBo);
        }

        // POST: DanhGiaXepLoaiCanBo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDanhGiaXepLoaiCanBo,IdCanBo,IdDanhGia,NamDanhGia,NganhDuocKhenThuong")] TbDanhGiaXepLoaiCanBo tbDanhGiaXepLoaiCanBo)
        {
            if (id != tbDanhGiaXepLoaiCanBo.IdDanhGiaXepLoaiCanBo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbDanhGiaXepLoaiCanBo>("/api/cb/DanhGiaXepLoaiCanBo", id, tbDanhGiaXepLoaiCanBo);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbDanhGiaXepLoaiCanBoExists(tbDanhGiaXepLoaiCanBo.IdDanhGiaXepLoaiCanBo) == false)

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
            ViewData["IdDanhGia"] = new SelectList(await ApiServices_.GetAll<DmDanhGiaCongChucVienChuc>("/api/dm/DanhGiaCongChucVienChuc"), "IdDanhGiaCongChucVienChuc", "DanhGiaCongChucVienChuc", tbDanhGiaXepLoaiCanBo.IdDanhGia);
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbDanhGiaXepLoaiCanBo.IdCanBo);
            return View(tbDanhGiaXepLoaiCanBo);
        }

        // GET: DanhGiaXepLoaiCanBo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbDanhGiaXepLoaiCanBos = await ApiServices_.GetAll<TbDanhGiaXepLoaiCanBo>("/api/cb/DanhGiaXepLoaiCanBo");
            var tbDanhGiaXepLoaiCanBo = tbDanhGiaXepLoaiCanBos.FirstOrDefault(m => m.IdDanhGiaXepLoaiCanBo == id);
            if (tbDanhGiaXepLoaiCanBo == null)
            {
                return NotFound();
            }

            return View(tbDanhGiaXepLoaiCanBo);
        }

        // POST: DanhGiaXepLoaiCanBo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbDanhGiaXepLoaiCanBo>("/api/cb/DanhGiaXepLoaiCanBo", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<bool> TbDanhGiaXepLoaiCanBoExists(int id)
        {
            var tbDanhGiaXepLoaiCanBos = await ApiServices_.GetAll<TbDanhGiaXepLoaiCanBo>("/api/cb/DanhGiaXepLoaiCanBo");
            return tbDanhGiaXepLoaiCanBos.Any(e => e.IdDanhGiaXepLoaiCanBo == id);
        }
    }
}

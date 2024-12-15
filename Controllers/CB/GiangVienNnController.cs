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
    public class GiangVienNnController : Controller
    {
        private readonly ApiServices ApiServices_;

        public GiangVienNnController(ApiServices services)
        {
            ApiServices_ = services;
        }

        //==========================================TẠO DANH SÁCH THÔNG TIN TỪ API===========================
        private async Task<List<TbGiangVienNn>> TbGiangVienNns()
        {
            List<TbGiangVienNn> tbGiangVienNns = await ApiServices_.GetAll<TbGiangVienNn>("/api/cb/GiangVienNn");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<DmNoiDungHoatDongTaiVietNam> dmnoiDungHoatDongTaiVietNams = await ApiServices_.GetAll<DmNoiDungHoatDongTaiVietNam>("/api/dm/NoiDungHoatDongTaiVietNam");
            tbGiangVienNns.ForEach(item => {
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdNoiDungHoatDongTaiVietNamNavigation = dmnoiDungHoatDongTaiVietNams.FirstOrDefault(x => x.IdNoiDungHoatDongTaiVietNam == item.IdNoiDungHoatDongTaiVietNam);
            });
            return tbGiangVienNns;
        }

        public async Task<IActionResult> Statistics()
        {
            List<TbGiangVienNn> getall = await TbGiangVienNns();
            return View(getall);
        }


        // GET: TbGiangVienNns
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbGiangVienNn> getall = await TbGiangVienNns();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: TbGiangVienNns/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbGiangVienNns = await TbGiangVienNns();
                var tbGiangVienNn = tbGiangVienNns.FirstOrDefault(m => m.IdGvnn == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbGiangVienNn == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbGiangVienNn);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: TbGiangVienNns/Create
      
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo");
                ViewData["IdNoiDungHoatDongTaiVietNam"] = new SelectList(await ApiServices_.GetAll<DmNoiDungHoatDongTaiVietNam>("/api/dm/NoiDungHoatDongTaiVietNam"), "IdNoiDungHoatDongTaiVietNam", "NoiDungHoatDongTaiVietNam");
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: TbGiangVienNns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGvnn,IdCanBo,CoQuanChuQuanOnuocNgoai,IdNoiDungHoatDongTaiVietNam")] TbGiangVienNn tbGiangVienNn)
        {
            if (await TbGiangVienNnExists(tbGiangVienNn.IdGvnn)) ModelState.AddModelError("IdGvnn", "ID này đã tồn tại!");

            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbGiangVienNn>("/api/cb/GiangVienNn", tbGiangVienNn);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbGiangVienNn.IdCanBo);
            ViewData["IdNoiDungHoatDongTaiVietNam"] = new SelectList(await ApiServices_.GetAll<DmNoiDungHoatDongTaiVietNam>("/api/dm/NoiDungHoatDongTaiVietNam"), "IdNoiDungHoatDongTaiVietNam", "NoiDungHoatDongTaiVietNam", tbGiangVienNn.IdNoiDungHoatDongTaiVietNam);
            return View(tbGiangVienNn);
        }

        // GET: TbGiangVienNns/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbGiangVienNn = await ApiServices_.GetId<TbGiangVienNn>("/api/cb/GiangVienNn", id ?? 0);
            if (tbGiangVienNn == null)
            {
                return NotFound();
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbGiangVienNn.IdCanBo);
            ViewData["IdNoiDungHoatDongTaiVietNam"] = new SelectList(await ApiServices_.GetAll<DmNoiDungHoatDongTaiVietNam>("/api/dm/NoiDungHoatDongTaiVietNam"), "IdNoiDungHoatDongTaiVietNam", "NoiDungHoatDongTaiVietNam", tbGiangVienNn.IdNoiDungHoatDongTaiVietNam);
            return View(tbGiangVienNn);
        }

        // POST: TbGiangVienNns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdGvnn,IdCanBo,CoQuanChuQuanOnuocNgoai,IdNoiDungHoatDongTaiVietNam")] TbGiangVienNn tbGiangVienNn)
        {
            if (id != tbGiangVienNn.IdGvnn)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbGiangVienNn>("/api/cb/GiangVienNn", id, tbGiangVienNn);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbGiangVienNnExists(tbGiangVienNn.IdGvnn) == false)
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
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbGiangVienNn.IdCanBo);
            ViewData["IdNoiDungHoatDongTaiVietNam"] = new SelectList(await ApiServices_.GetAll<DmNoiDungHoatDongTaiVietNam>("/api/dm/NoiDungHoatDongTaiVietNam"), "IdNoiDungHoatDongTaiVietNam", "NoiDungHoatDongTaiVietNam", tbGiangVienNn.IdNoiDungHoatDongTaiVietNam);
            return View(tbGiangVienNn);
        }

        // GET: TbGiangVienNns/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tbGiangVienNns = await ApiServices_.GetAll<TbGiangVienNn>("/api/cb/GiangVienNn");
            var tbGiangVienNn = tbGiangVienNns.FirstOrDefault(m => m.IdGvnn == id);
            if (tbGiangVienNn == null)
            {
                return NotFound();
            }

            return View(tbGiangVienNn);
        }

        // POST: TbGiangVienNns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbGiangVienNn>("/api/cb/GiangVienNn", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<bool> TbGiangVienNnExists(int id)
        {
            var tbGiangVienNns = await ApiServices_.GetAll<TbGiangVienNn>("/api/cb/GiangVienNn");
            return tbGiangVienNns.Any(e => e.IdGvnn == id);
        }
    }
}

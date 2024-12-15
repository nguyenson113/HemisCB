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
    public class DoiTuongChinhSachCanBoController : Controller
    {
        private readonly ApiServices ApiServices_;

        public DoiTuongChinhSachCanBoController(ApiServices services)
        {
            ApiServices_ = services;
        }

        //============================TẠO DANH SÁCH LẤY API========================
        private async Task<List<TbDoiTuongChinhSachCanBo>> TbDoiTuongChinhSachCanBos()
        {
            List<TbDoiTuongChinhSachCanBo> tbDoiTuongChinhSachCanBos = await ApiServices_.GetAll<TbDoiTuongChinhSachCanBo>("/api/cb/DoiTuongChinhSachCanBo");
            List<DmDoiTuongChinhSach> dmdoiTuongChinhSachs = await ApiServices_.GetAll<DmDoiTuongChinhSach>("/api/dm/DoiTuongChinhSach");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            tbDoiTuongChinhSachCanBos.ForEach(item => {
                item.IdDoiTuongChinhSachNavigation = dmdoiTuongChinhSachs.FirstOrDefault(x => x.IdDoiTuongChinhSach == item.IdDoiTuongChinhSach);
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
            });
            return tbDoiTuongChinhSachCanBos;
        }

        public async Task<IActionResult> Statistics()
        {
            List<TbDoiTuongChinhSachCanBo> getall = await TbDoiTuongChinhSachCanBos();
            return View(getall);
        }


        // GET: DoiTuongChinhSachCanBo
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbDoiTuongChinhSachCanBo> getall = await TbDoiTuongChinhSachCanBos();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }


        // GET: DoiTuongChinhSachCanBo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Kiểm tra nếu id là null
            if (id == null)
            {
                return NotFound();
            }

            // Tìm quá trình công tác theo id
            var tbDoiTuongChinhSachCanBos = await TbDoiTuongChinhSachCanBos();
            var tbDoiTuongChinhSachCanBo = tbDoiTuongChinhSachCanBos.FirstOrDefault(m => m.IdDoiTuongChinhSachCanBo == id);

            // Kiểm tra nếu không tìm thấy
            if (tbDoiTuongChinhSachCanBo == null)
            {
                return NotFound();
            }

            // Trả về view với thông tin chi tiết
            return View(tbDoiTuongChinhSachCanBo);
        }


        // GET: DoiTuongChinhSachCanBo/Create
        public async Task<IActionResult> Create()
        {
            ViewData["IdDoiTuongChinhSach"] = new SelectList(await ApiServices_.GetAll<DmDoiTuongChinhSach>("/api/dm/DoiTuongChinhSach"), "IdDoiTuongChinhSach", "DoiTuongChinhSach");
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo");
            return View();
        }

        // POST: DoiTuongChinhSachCanBo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDoiTuongChinhSachCanBo,IdCanBo,IdDoiTuongChinhSach,TuNgay,DenNgay")] TbDoiTuongChinhSachCanBo tbDoiTuongChinhSachCanBo)
        {
            if (await TbDoiTuongChinhSachCanBoExists(tbDoiTuongChinhSachCanBo.IdDoiTuongChinhSachCanBo)) ModelState.AddModelError("IdDoiTuongChinhSachCanBo", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbDoiTuongChinhSachCanBo>("/api/cb/DoiTuongChinhSachCanBo", tbDoiTuongChinhSachCanBo);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbDoiTuongChinhSachCanBo.IdCanBo);
            ViewData["IdDoiTuongChinhSach"] = new SelectList(await ApiServices_.GetAll<DmDoiTuongChinhSach>("/api/dm/DoiTuongChinhSach"), "IdDoiTuongChinhSach", "DoiTuongChinhSach", tbDoiTuongChinhSachCanBo.IdDoiTuongChinhSach);
            return View(tbDoiTuongChinhSachCanBo);
        }

        // GET: DoiTuongChinhSachCanBo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbDoiTuongChinhSachCanBo = await ApiServices_.GetId<TbDoiTuongChinhSachCanBo>("/api/cb/DoiTuongChinhSachCanBo", id ?? 0);
            if (tbDoiTuongChinhSachCanBo == null)
            {
                return NotFound();
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbDoiTuongChinhSachCanBo.IdCanBo);
            ViewData["IdDoiTuongChinhSach"] = new SelectList(await ApiServices_.GetAll<DmDoiTuongChinhSach>("/api/dm/DoiTuongChinhSach"), "IdDoiTuongChinhSach", "DoiTuongChinhSach", tbDoiTuongChinhSachCanBo.IdDoiTuongChinhSach);
            return View(tbDoiTuongChinhSachCanBo);
        }

        // POST: DoiTuongChinhSachCanBo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDoiTuongChinhSachCanBo,IdCanBo,IdDoiTuongChinhSach,TuNgay,DenNgay")] TbDoiTuongChinhSachCanBo tbDoiTuongChinhSachCanBo)
        {
            if (id != tbDoiTuongChinhSachCanBo.IdDoiTuongChinhSachCanBo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbDoiTuongChinhSachCanBo>("/api/cb/DoiTuongChinhSachCanBo", id, tbDoiTuongChinhSachCanBo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbDoiTuongChinhSachCanBoExists(tbDoiTuongChinhSachCanBo.IdDoiTuongChinhSachCanBo) == false)
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
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbDoiTuongChinhSachCanBo.IdCanBo);
            ViewData["IdDoiTuongChinhSach"] = new SelectList(await ApiServices_.GetAll<DmDoiTuongChinhSach>("/api/dm/DoiTuongChinhSach"), "IdDoiTuongChinhSach", "DoiTuongChinhSach", tbDoiTuongChinhSachCanBo.IdDoiTuongChinhSach);
            return View(tbDoiTuongChinhSachCanBo);
        }

        // GET: DoiTuongChinhSachCanBo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbDoiTuongChinhSachCanBos = await ApiServices_.GetAll<TbDoiTuongChinhSachCanBo>("/api/cb/DoiTuongChinhSachCanBo");
            var tbDoiTuongChinhSachCanBo = tbDoiTuongChinhSachCanBos.FirstOrDefault(m => m.IdDoiTuongChinhSachCanBo == id);
            if (tbDoiTuongChinhSachCanBo == null)
            {
                return NotFound();
            }

            return View(tbDoiTuongChinhSachCanBo);
        }

        // POST: DoiTuongChinhSachCanBo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbDoiTuongChinhSachCanBo>("/api/cb/DoiTuongChinhSachCanBo", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<bool> TbDoiTuongChinhSachCanBoExists(int id)
        {
            var tbDoiTuongChinhSachCanBos = await ApiServices_.GetAll<TbDoiTuongChinhSachCanBo>("/api/cb/DoiTuongChinhSachCanBo");
            return tbDoiTuongChinhSachCanBos.Any(e => e.IdDoiTuongChinhSachCanBo == id);
        }
    }
}

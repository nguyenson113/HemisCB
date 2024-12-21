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
        public class DanhHieuThiDuaGiaiThuongKhenThuongCanBoController : Controller
        {
        private readonly ApiServices ApiServices_;

        public DanhHieuThiDuaGiaiThuongKhenThuongCanBoController(ApiServices services)
            {
            ApiServices_ = services;
        }

        // GET: DanhHieuThiDuaGiaiThuongKhenThuongCanBo
        //=============================================== TẠO LIST DANH SÁCH LẤY DỮ LIỆU API =====================================================================================
        private async Task<List<TbDanhHieuThiDuaGiaiThuongKhenThuongCanBo>> TbDanhHieuThiDuaGiaiThuongKhenThuongCanBos()
        {
            List<TbDanhHieuThiDuaGiaiThuongKhenThuongCanBo> tbDanhHieuThiDuaGiaiThuongKhenThuongCanBos = await ApiServices_.GetAll<TbDanhHieuThiDuaGiaiThuongKhenThuongCanBo>("/api/cb/DanhHieuThiDuaGiaiThuongKhenThuongCanBo");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<DmCapKhenThuong> dmcapKhenThuongs = await ApiServices_.GetAll<DmCapKhenThuong>("/api/dm/CapKhenThuong");
            List<DmLoaiDanhHieuThiDuaGiaiThuongKhenThuong> dmloaiDanhHieuThiDuaGiaiThuongKhenThuongs = await ApiServices_.GetAll<DmLoaiDanhHieuThiDuaGiaiThuongKhenThuong>("/api/dm/LoaiDanhHieuThiDuaGiaiThuongKhenThuong");
            List<DmPhuongThucKhenThuong> dmphuongThucKhenThuongs = await ApiServices_.GetAll<DmPhuongThucKhenThuong>("/api/dm/PhuongThucKhenThuong");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            List<DmThiDuaGiaiThuongKhenThuong> dmthiDuaGiaiThuongKhenThuongs = await ApiServices_.GetAll<DmThiDuaGiaiThuongKhenThuong>("/api/dm/ThiDuaGiaiThuongKhenThuong");
            tbDanhHieuThiDuaGiaiThuongKhenThuongCanBos.ForEach(item => {
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdCapKhenThuongNavigation = dmcapKhenThuongs.FirstOrDefault(x => x.IdCapKhenThuong == item.IdCapKhenThuong);
                item.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuongNavigation = dmloaiDanhHieuThiDuaGiaiThuongKhenThuongs.FirstOrDefault(x => x.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong == item.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong);
                item.IdPhuongThucKhenThuongNavigation = dmphuongThucKhenThuongs.FirstOrDefault(x => x.IdPhuongThucKhenThuong == item.IdPhuongThucKhenThuong);
                item.IdCanBoNavigation.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdCanBoNavigation.IdNguoi);
                item.IdThiDuaGiaiThuongKhenThuongNavigation = dmthiDuaGiaiThuongKhenThuongs.FirstOrDefault(x => x.IdThiDuaGiaiThuongKhenThuong == item.IdThiDuaGiaiThuongKhenThuong);
            });
            return tbDanhHieuThiDuaGiaiThuongKhenThuongCanBos;
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
            List<TbDanhHieuThiDuaGiaiThuongKhenThuongCanBo> getall = await TbDanhHieuThiDuaGiaiThuongKhenThuongCanBos();
            return View(getall);
        }



        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbDanhHieuThiDuaGiaiThuongKhenThuongCanBo> getall = await TbDanhHieuThiDuaGiaiThuongKhenThuongCanBos();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        public async Task<IActionResult> Delete(int? id)
        {
            // Kiểm tra nếu id là null
            if (id == null)
            {
                return NotFound();
            }

            var tbDanhHieuThiDuaGiaiThuongKhenThuongCanBos = await TbDanhHieuThiDuaGiaiThuongKhenThuongCanBos();
            var tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo = tbDanhHieuThiDuaGiaiThuongKhenThuongCanBos.FirstOrDefault(m => m.IdDanhHieuThiDuaGiaiThuongKhenThuongCanBo == id);
            if (tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo == null)
            {
                return NotFound();
            }

            // Trả về view với thông tin để xác nhận xóa
            return View(tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo);
        }

        // GET: DanhHieuThiDuaGiaiThuongKhenThuongCanBo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbDanhHieuThiDuaGiaiThuongKhenThuongCanBos = await TbDanhHieuThiDuaGiaiThuongKhenThuongCanBos();
                var tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo = tbDanhHieuThiDuaGiaiThuongKhenThuongCanBos.FirstOrDefault(m => m.IdDanhHieuThiDuaGiaiThuongKhenThuongCanBo == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        // GET: DanhHieuThiDuaGiaiThuongKhenThuongCanBo/Create
        public async Task<IActionResult> Create()
        {
            ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name");
            ViewData["IdCapKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmCapKhenThuong>("/api/dm/CapKhenThuong"), "IdCapKhenThuong", "CapKhenThuong");
            ViewData["IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmLoaiDanhHieuThiDuaGiaiThuongKhenThuong>("/api/dm/LoaiDanhHieuThiDuaGiaiThuongKhenThuong"), "IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong", "LoaiDanhHieuThiDuaGiaiThuongKhenThuong");
            ViewData["IdPhuongThucKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmPhuongThucKhenThuong>("/api/dm/PhuongThucKhenThuong"), "IdPhuongThucKhenThuong", "PhuongThucKhenThuong");
            ViewData["IdThiDuaGiaiThuongKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmThiDuaGiaiThuongKhenThuong>("/api/dm/ThiDuaGiaiThuongKhenThuong"), "IdThiDuaGiaiThuongKhenThuong", "ThiDuaGiaiThuongKhenThuong");
            return View();
        }



        // POST: DanhHieuThiDuaGiaiThuongKhenThuongCanBo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDanhHieuThiDuaGiaiThuongKhenThuongCanBo,IdCanBo,LoaiDanhHieuThiDuaGiaiThuongKhenThuong,ThiDuaGiaiThuongKhenThuong,SoQuyetDinh,PhuongThucKhenThuong,NamKhenThuong,CapKhenThuong")] TbDanhHieuThiDuaGiaiThuongKhenThuongCanBo tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo)
        {
            if (await TbDanhHieuThiDuaGiaiThuongKhenThuongCanBoExists(tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo.IdDanhHieuThiDuaGiaiThuongKhenThuongCanBo)) ModelState.AddModelError("IdDanhHieuThiDuaGiaiThuongKhenThuongCanBo", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbDanhHieuThiDuaGiaiThuongKhenThuongCanBo>("/api/cb/DanhHieuThiDuaGiaiThuongKhenThuongCanBo", tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo.IdCanBo);
            ViewData["IdCapKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmCapKhenThuong>("/api/dm/CapKhenThuong"), "IdCapKhenThuong", "CapKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo.IdCapKhenThuong);
            ViewData["IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmLoaiDanhHieuThiDuaGiaiThuongKhenThuong>("/api/dm/LoaiDanhHieuThiDuaGiaiThuongKhenThuong"), "IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong", "LoaiDanhHieuThiDuaGiaiThuongKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong);
            ViewData["IdPhuongThucKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmPhuongThucKhenThuong>("/api/dm/PhuongThucKhenThuong"), "IdPhuongThucKhenThuong", "PhuongThucKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo.IdPhuongThucKhenThuong);
            ViewData["IdThiDuaGiaiThuongKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmThiDuaGiaiThuongKhenThuong>("/api/dm/ThiDuaGiaiThuongKhenThuong"), "IdThiDuaGiaiThuongKhenThuong", "ThiDuaGiaiThuongKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo.IdThiDuaGiaiThuongKhenThuong);
            return View(tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo);
        }

        // GET: DanhHieuThiDuaGiaiThuongKhenThuongCanBo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo = await ApiServices_.GetId<TbDanhHieuThiDuaGiaiThuongKhenThuongCanBo>("/api/cb/DanhHieuThiDuaGiaiThuongKhenThuongCanBo", id ?? 0);
            if (tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo == null)
            {
                return NotFound();
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo.IdCanBo);
            ViewData["IdCapKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmCapKhenThuong>("/api/dm/CapKhenThuong"), "IdCapKhenThuong", "CapKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo.IdCapKhenThuong);
            ViewData["IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmLoaiDanhHieuThiDuaGiaiThuongKhenThuong>("/api/dm/LoaiDanhHieuThiDuaGiaiThuongKhenThuong"), "IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong", "LoaiDanhHieuThiDuaGiaiThuongKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong);
            ViewData["IdPhuongThucKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmPhuongThucKhenThuong>("/api/dm/PhuongThucKhenThuong"), "IdPhuongThucKhenThuong", "PhuongThucKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo.IdPhuongThucKhenThuong);
            ViewData["IdThiDuaGiaiThuongKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmThiDuaGiaiThuongKhenThuong>("/api/dm/ThiDuaGiaiThuongKhenThuong"), "IdThiDuaGiaiThuongKhenThuong", "ThiDuaGiaiThuongKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo.IdThiDuaGiaiThuongKhenThuong);
            return View(tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo);
        }

        // POST: DanhHieuThiDuaGiaiThuongKhenThuongCanBo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDanhHieuThiDuaGiaiThuongKhenThuongCanBo,IdCanBo,IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong,IdThiDuaGiaiThuongKhenThuong,SoQuyetDinh,IdPhuongThucKhenThuong,NamKhenThuong,IdCapKhenThuong")] TbDanhHieuThiDuaGiaiThuongKhenThuongCanBo tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo)
        {
            if (id != tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo.IdDanhHieuThiDuaGiaiThuongKhenThuongCanBo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbDanhHieuThiDuaGiaiThuongKhenThuongCanBo>("/api/cb/DanhHieuThiDuaGiaiThuongKhenThuongCanBo", id, tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbDanhHieuThiDuaGiaiThuongKhenThuongCanBoExists(tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo.IdDanhHieuThiDuaGiaiThuongKhenThuongCanBo) == false)
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
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo.IdCanBo);
            ViewData["IdCapKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmCapKhenThuong>("/api/dm/CapKhenThuong"), "IdCapKhenThuong", "CapKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo.IdCapKhenThuong);
            ViewData["IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmLoaiDanhHieuThiDuaGiaiThuongKhenThuong>("/api/dm/LoaiDanhHieuThiDuaGiaiThuongKhenThuong"), "IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong", "LoaiDanhHieuThiDuaGiaiThuongKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuong);
            ViewData["IdPhuongThucKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmPhuongThucKhenThuong>("/api/dm/PhuongThucKhenThuong"), "IdPhuongThucKhenThuong", "PhuongThucKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo.IdPhuongThucKhenThuong);
            ViewData["IdThiDuaGiaiThuongKhenThuong"] = new SelectList(await ApiServices_.GetAll<DmThiDuaGiaiThuongKhenThuong>("/api/dm/ThiDuaGiaiThuongKhenThuong"), "IdThiDuaGiaiThuongKhenThuong", "ThiDuaGiaiThuongKhenThuong", tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo.IdThiDuaGiaiThuongKhenThuong);
            return View(tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo);
        }

        //// GET: DanhHieuThiDuaGiaiThuongKhenThuongCanBo/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo = await _context.TbDanhHieuThiDuaGiaiThuongKhenThuongCanBos
        //        .Include(t => t.IdCanBoNavigation)
        //        .Include(t => t.IdCapKhenThuongNavigation)
        //        .Include(t => t.IdLoaiDanhHieuThiDuaGiaiThuongKhenThuongNavigation)
        //        .Include(t => t.IdPhuongThucKhenThuongNavigation)
        //        .Include(t => t.IdThiDuaGiaiThuongKhenThuongNavigation)
        //        .FirstOrDefaultAsync(m => m.IdDanhHieuThiDuaGiaiThuongKhenThuongCanBo == id);
        //    if (tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo);
        //}

        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo = await _context.TbDanhHieuThiDuaGiaiThuongKhenThuongCanBos.FindAsync(id);
        //    if (tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo != null)
        //    {
        //        _context.TbDanhHieuThiDuaGiaiThuongKhenThuongCanBos.Remove(tbDanhHieuThiDuaGiaiThuongKhenThuongCanBo);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbDanhHieuThiDuaGiaiThuongKhenThuongCanBo>("/api/cb/DanhHieuThiDuaGiaiThuongKhenThuongCanBo", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<bool> TbDanhHieuThiDuaGiaiThuongKhenThuongCanBoExists(int id)
        {
            var tbDanhHieuThiDuaGiaiThuongKhenThuongCanBos = await ApiServices_.GetAll<TbDanhHieuThiDuaGiaiThuongKhenThuongCanBo>("/api/cb/DanhHieuThiDuaGiaiThuongKhenThuongCanBo");
            return tbDanhHieuThiDuaGiaiThuongKhenThuongCanBos.Any(e => e.IdDanhHieuThiDuaGiaiThuongKhenThuongCanBo == id);
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
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
    public class ChucDanhKhoaHocCuaCanBoController : Controller
    {
        private readonly ApiServices ApiServices_;

        public ChucDanhKhoaHocCuaCanBoController(ApiServices services)
        {
            ApiServices_ = services;
        }

        //=================================================== TẠO LIST DANH SÁCH ĐỂ LẤY DỮ LIỆU TỪ APIHEMIS ===============================

        // GET: ChucDanhKhoaHocCuaCanBo
        private async Task<List<TbChucDanhKhoaHocCuaCanBo>> TbChucDanhKhoaHocCuaCanBos()
        {
            List<TbChucDanhKhoaHocCuaCanBo> tbChucDanhKhoaHocCuaCanBos = await ApiServices_.GetAll<TbChucDanhKhoaHocCuaCanBo>("/api/cb/ChucDanhKhoaHocCuaCanBo");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<DmChucDanhKhoaHoc> dmChucDanhKhoaHocs = await ApiServices_.GetAll<DmChucDanhKhoaHoc>("/api/dm/ChucDanhKhoaHoc");
            List<DmLoaiQuyetDinh> dmLoaiQuyetDinhs = await ApiServices_.GetAll<DmLoaiQuyetDinh>("/api/dm/LoaiQuyetDinh");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            tbChucDanhKhoaHocCuaCanBos.ForEach(item =>
            {
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdChucDanhKhoaHocNavigation = dmChucDanhKhoaHocs.FirstOrDefault(x => x.IdChucDanhKhoaHoc == item.IdChucDanhKhoaHoc);
                item.IdThamQuyenQuyetDinhNavigation = dmLoaiQuyetDinhs.FirstOrDefault(x => x.IdLoaiQuyetDinh == item.IdThamQuyenQuyetDinh);
                item.IdCanBoNavigation.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdCanBoNavigation.IdNguoi);
            });
            return tbChucDanhKhoaHocCuaCanBos;
        }

        private async Task<List<TbCanBo>> TbCanBos()
        {
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            tbcanbos.ForEach(item =>
            {
                item.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdNguoi);

            });

            return tbcanbos;
        }

        public async Task<IActionResult> Statistics()
        {
            List<TbChucDanhKhoaHocCuaCanBo> getall = await TbChucDanhKhoaHocCuaCanBos();
            return View(getall);
        }

        // GET: DanhGiaXepLoaiCanBo


        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbChucDanhKhoaHocCuaCanBo> getall = await TbChucDanhKhoaHocCuaCanBos();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: ChucDanhKhoaHocCuaCanBo/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbChucDanhKhoaHocCuaCanBos = await TbChucDanhKhoaHocCuaCanBos();
                var tbChucDanhKhoaHocCuaCanBo = tbChucDanhKhoaHocCuaCanBos.FirstOrDefault(m => m.IdChucDanhKhoaHocCuaCanBo == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbChucDanhKhoaHocCuaCanBo == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbChucDanhKhoaHocCuaCanBo);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        // GET: ChucDanhKhoaHocCuaCanBo/Create

        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name");
                ViewData["IdChucDanhKhoaHoc"] = new SelectList(await ApiServices_.GetAll<DmChucDanhKhoaHoc>("/api/dm/ChucDanhKhoaHoc"), "IdChucDanhKhoaHoc", "ChucDanhKhoaHoc");
                ViewData["IdThamQuyenQuyetDinh"] = new SelectList(await ApiServices_.GetAll<DmLoaiQuyetDinh>("/api/dm/LoaiQuyetDinh"), "IdLoaiQuyetDinh", "LoaiQuyetDinh");
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: ChucDanhKhoaHocCuaCanBo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdChucDanhKhoaHocCuaCanBo,IdCanBo,IdChucDanhKhoaHoc,IdThamQuyenQuyetDinh,SoQuyetDinh,NgayQuyetDinh")] TbChucDanhKhoaHocCuaCanBo tbChucDanhKhoaHocCuaCanBo)
        {
            if (await TbChucDanhKhoaHocCuaCanBoExists(tbChucDanhKhoaHocCuaCanBo.IdChucDanhKhoaHocCuaCanBo)) ModelState.AddModelError("IdChucDanhKhoaHocCuaCanBo", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbChucDanhKhoaHocCuaCanBo>("/api/cb/ChucDanhKhoaHocCuaCanBo", tbChucDanhKhoaHocCuaCanBo);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbChucDanhKhoaHocCuaCanBo.IdCanBo);
            ViewData["IdChucDanhKhoaHoc"] = new SelectList(await ApiServices_.GetAll<DmChucDanhKhoaHoc>("/api/dm/ChucDanhKhoaHoc"), "IdChucDanhKhoaHoc", "ChucDanhKhoaHoc", tbChucDanhKhoaHocCuaCanBo.IdChucDanhKhoaHoc);
            ViewData["IdThamQuyenQuyetDinh"] = new SelectList(await ApiServices_.GetAll<DmLoaiQuyetDinh>("/api/dm/LoaiQuyetDinh"), "IdLoaiQuyetDinh", "LoaiQuyetDinh", tbChucDanhKhoaHocCuaCanBo.IdThamQuyenQuyetDinh);
            return View(tbChucDanhKhoaHocCuaCanBo);
        }

        // GET: ChucDanhKhoaHocCuaCanBo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbChucDanhKhoaHocCuaCanBo = await ApiServices_.GetId<TbChucDanhKhoaHocCuaCanBo>("/api/cb/ChucDanhKhoaHocCuaCanBo", id ?? 0);
            if (tbChucDanhKhoaHocCuaCanBo == null)
            {
                return NotFound();
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbChucDanhKhoaHocCuaCanBo.IdCanBo);
            ViewData["IdChucDanhKhoaHoc"] = new SelectList(await ApiServices_.GetAll<DmChucDanhKhoaHoc>("/api/dm/ChucDanhKhoaHoc"), "IdChucDanhKhoaHoc", "ChucDanhKhoaHoc", tbChucDanhKhoaHocCuaCanBo.IdChucDanhKhoaHoc);
            ViewData["IdThamQuyenQuyetDinh"] = new SelectList(await ApiServices_.GetAll<DmLoaiQuyetDinh>("/api/dm/LoaiQuyetDinh"), "IdLoaiQuyetDinh", "LoaiQuyetDinh", tbChucDanhKhoaHocCuaCanBo.IdThamQuyenQuyetDinh);
            return View(tbChucDanhKhoaHocCuaCanBo);
        }

        // POST: ChucDanhKhoaHocCuaCanBo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdChucDanhKhoaHocCuaCanBo,IdCanBo,IdChucDanhKhoaHoc,IdThamQuyenQuyetDinh,SoQuyetDinh,NgayQuyetDinh")] TbChucDanhKhoaHocCuaCanBo tbChucDanhKhoaHocCuaCanBo)
        {
            if (id != tbChucDanhKhoaHocCuaCanBo.IdChucDanhKhoaHocCuaCanBo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbChucDanhKhoaHocCuaCanBo>("/api/cb/ChucDanhKhoaHocCuaCanBo", id, tbChucDanhKhoaHocCuaCanBo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbChucDanhKhoaHocCuaCanBoExists(tbChucDanhKhoaHocCuaCanBo.IdChucDanhKhoaHocCuaCanBo) == false)
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
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbChucDanhKhoaHocCuaCanBo.IdCanBo);
            ViewData["IdChucDanhKhoaHoc"] = new SelectList(await ApiServices_.GetAll<DmChucDanhKhoaHoc>("/api/dm/ChucDanhKhoaHoc"), "IdChucDanhKhoaHoc", "ChucDanhKhoaHoc", tbChucDanhKhoaHocCuaCanBo.IdChucDanhKhoaHoc);
            ViewData["IdThamQuyenQuyetDinh"] = new SelectList(await ApiServices_.GetAll<DmLoaiQuyetDinh>("/api/dm/LoaiQuyetDinh"), "IdLoaiQuyetDinh", "LoaiQuyetDinh", tbChucDanhKhoaHocCuaCanBo.IdThamQuyenQuyetDinh);
            return View(tbChucDanhKhoaHocCuaCanBo);
        }

        // GET: ChucDanhKhoaHocCuaCanBo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbChucDanhKhoaHocCuaCanBos = await ApiServices_.GetAll<TbChucDanhKhoaHocCuaCanBo>("/api/cb/ChucDanhKhoaHocCuaCanBo");
            var tbChucDanhKhoaHocCuaCanBo = tbChucDanhKhoaHocCuaCanBos.FirstOrDefault(m => m.IdChucDanhKhoaHocCuaCanBo == id);
            if (tbChucDanhKhoaHocCuaCanBo == null)
            {
                return NotFound();
            }

            return View(tbChucDanhKhoaHocCuaCanBo);
        }

        // POST: ChucDanhKhoaHocCuaCanBo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbChucDanhKhoaHocCuaCanBo>("/api/cb/ChucDanhKhoaHocCuaCanBo", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<bool> TbChucDanhKhoaHocCuaCanBoExists(int id)
        {
            var tbChucDanhKhoaHocCuaCanBos = await ApiServices_.GetAll<TbChucDanhKhoaHocCuaCanBo>("/api/cb/ChucDanhKhoaHocCuaCanBo");
            return tbChucDanhKhoaHocCuaCanBos.Any(e => e.IdChucDanhKhoaHocCuaCanBo == id);
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
    }
}



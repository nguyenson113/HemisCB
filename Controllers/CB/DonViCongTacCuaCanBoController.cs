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
    public class DonViCongTacCuaCanBoController : Controller
    {
        private readonly ApiServices ApiServices_;

        public DonViCongTacCuaCanBoController(ApiServices services)
        {
            ApiServices_ = services;
        }


        //==========================================TẠO DANH SÁCH THÔNG TIN TỪ API===========================
        private async Task<List<TbDonViCongTacCuaCanBo>> TbDonViCongTacCuaCanBos()
        {
            List<TbDonViCongTacCuaCanBo> tbDonViCongTacCuaCanBos = await ApiServices_.GetAll<TbDonViCongTacCuaCanBo>("/api/cb/DonViCongTacCuaCanBo");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<DmChucVu> dmchucVus = await ApiServices_.GetAll<DmChucVu>("/api/dm/ChucVu");
            List<DmHinhThucBoNhiem> dmhinhThucBoNhiems = await ApiServices_.GetAll<DmHinhThucBoNhiem>("/api/dm/HinhThucBoNhiem");
           
            tbDonViCongTacCuaCanBos.ForEach(item => {
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdChucVuNavigation = dmchucVus.FirstOrDefault(x => x.IdChucVu == item.IdChucVu);
                item.IdHinhThucBoNhiemNavigation = dmhinhThucBoNhiems.FirstOrDefault(x => x.IdHinhThucBoNhiem == item.IdHinhThucBoNhiem);
                
            });
            return tbDonViCongTacCuaCanBos;
        }

        public async Task<IActionResult> Statistics()
        {
            List<TbDonViCongTacCuaCanBo> getall = await TbDonViCongTacCuaCanBos();
            return View(getall);
        }

        // GET: DonViCongTacCuaCanBo
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbDonViCongTacCuaCanBo> getall = await TbDonViCongTacCuaCanBos();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        // GET: DonViCongTacCuaCanBo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbDonViCongTacCuaCanBos = await TbDonViCongTacCuaCanBos();
                var tbDonViCongTacCuaCanBo = tbDonViCongTacCuaCanBos.FirstOrDefault(m => m.IdDvct == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbDonViCongTacCuaCanBo == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbDonViCongTacCuaCanBo);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        // GET: DonViCongTacCuaCanBo/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo");
                ViewData["IdChucVu"] = new SelectList(await ApiServices_.GetAll<DmChucVu>("/api/dm/ChucVu"), "IdChucVu", "ChucVu");
                ViewData["IdHinhThucBoNhiem"] = new SelectList(await ApiServices_.GetAll<DmHinhThucBoNhiem>("/api/dm/HinhThucBoNhiem"), "IdHinhThucBoNhiem", "HinhThucBoNhiem");
               
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }



        // POST: DonViCongTacCuaCanBo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDvct,IdCanBo,MaPhongBanDonVi,IdChucVu,IdHinhThucBoNhiem,SoQuyetDinh,NgayQuyetDinh,LaDonViChinh,LaDonViGiangDay,ThoiGianCoHieuLuc,ThoiGianHetHieuLuc")] TbDonViCongTacCuaCanBo tbDonViCongTacCuaCanBo)
        {
            if (await TbDonViCongTacCuaCanBoExists(tbDonViCongTacCuaCanBo.IdDvct)) ModelState.AddModelError("IdDvct", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbDonViCongTacCuaCanBo>("/api/cb/DonViCongTacCuaCanBo", tbDonViCongTacCuaCanBo);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbDonViCongTacCuaCanBo.IdCanBo);
            ViewData["IdChucVu"] = new SelectList(await ApiServices_.GetAll<DmChucVu>("/api/dm/ChucVu"), "IdChucVu", "ChucVu", tbDonViCongTacCuaCanBo.IdCanBo);
            ViewData["IdHinhThucBoNhiem"] = new SelectList(await ApiServices_.GetAll<DmHinhThucBoNhiem>("/api/dm/HinhThucBoNhiem"), "IdHinhThucBoNhiem", "HinhThucBoNhiem", tbDonViCongTacCuaCanBo.IdHinhThucBoNhiem);
            return View(tbDonViCongTacCuaCanBo);
        }

        // GET: DonViCongTacCuaCanBo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbDonViCongTacCuaCanBo = await ApiServices_.GetId<TbDonViCongTacCuaCanBo>("/api/cb/DonViCongTacCuaCanBo", id ?? 0);
            if (tbDonViCongTacCuaCanBo == null)
            {
                return NotFound();
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbDonViCongTacCuaCanBo.IdCanBo);
            ViewData["IdChucVu"] = new SelectList(await ApiServices_.GetAll<DmChucVu>("/api/dm/ChucVu"), "IdChucVu", "ChucVu", tbDonViCongTacCuaCanBo.IdCanBo);
            ViewData["IdHinhThucBoNhiem"] = new SelectList(await ApiServices_.GetAll<DmHinhThucBoNhiem>("/api/dm/HinhThucBoNhiem"), "IdHinhThucBoNhiem", "HinhThucBoNhiem", tbDonViCongTacCuaCanBo.IdHinhThucBoNhiem);
            return View(tbDonViCongTacCuaCanBo);
        }

        // POST: DonViCongTacCuaCanBo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDvct,IdCanBo,MaPhongBanDonVi,IdChucVu,IdHinhThucBoNhiem,SoQuyetDinh,NgayQuyetDinh,LaDonViChinh,LaDonViGiangDay,ThoiGianCoHieuLuc,ThoiGianHetHieuLuc")] TbDonViCongTacCuaCanBo tbDonViCongTacCuaCanBo)
        {
            if (id != tbDonViCongTacCuaCanBo.IdDvct)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbDonViCongTacCuaCanBo>("/api/cb/DonViCongTacCuaCanBo", id, tbDonViCongTacCuaCanBo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbDonViCongTacCuaCanBoExists(tbDonViCongTacCuaCanBo.IdDvct) == false)
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
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbDonViCongTacCuaCanBo.IdCanBo);
            ViewData["IdChucVu"] = new SelectList(await ApiServices_.GetAll<DmChucVu>("/api/dm/ChucVu"), "IdChucVu", "ChucVu", tbDonViCongTacCuaCanBo.IdCanBo);
            ViewData["IdHinhThucBoNhiem"] = new SelectList(await ApiServices_.GetAll<DmHinhThucBoNhiem>("/api/dm/HinhThucBoNhiem"), "IdHinhThucBoNhiem", "HinhThucBoNhiem", tbDonViCongTacCuaCanBo.IdHinhThucBoNhiem);
            return View(tbDonViCongTacCuaCanBo);
        }

        // GET: DonViCongTacCuaCanBo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbDonViCongTacCuaCanBos = await ApiServices_.GetAll<TbDonViCongTacCuaCanBo>("/api/cb/DonViCongTacCuaCanBo");
            var tbDonViCongTacCuaCanBo = tbDonViCongTacCuaCanBos.FirstOrDefault(m => m.IdDvct == id);
            if (tbDonViCongTacCuaCanBo == null)
            {
                return NotFound();
            }

            return View(tbDonViCongTacCuaCanBo);
        }

        // POST: DonViCongTacCuaCanBo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbDonViCongTacCuaCanBo>("/api/cb/DonViCongTacCuaCanBo", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<bool> TbDonViCongTacCuaCanBoExists(int id)
        {
            var tbDonViCongTacCuaCanBos = await ApiServices_.GetAll<TbDonViCongTacCuaCanBo>("/api/cb/DonViCongTacCuaCanBo");
            return tbDonViCongTacCuaCanBos.Any(e => e.IdDvct == id);
        }
    }
}

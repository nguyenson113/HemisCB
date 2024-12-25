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
    public class HopDongThinhGiangController : Controller
    {
        private readonly ApiServices ApiServices_;

        public HopDongThinhGiangController(ApiServices services)
        {
            ApiServices_ = services;
        }
        //==========================================TẠO DANH SÁCH THÔNG TIN TỪ API===========================
        private async Task<List<TbHopDongThinhGiang>> TbHopDongThinhGiangs()
        {
            List<TbHopDongThinhGiang> tbHopDongThinhGiangs = await ApiServices_.GetAll<TbHopDongThinhGiang>("/api/cb/HopDongThinhGiang");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<DmTrangThaiHopDong> dmtrangThaiHopDongs = await ApiServices_.GetAll<DmTrangThaiHopDong>("/api/dm/TrangThaiHopDong");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            tbHopDongThinhGiangs.ForEach(item => {
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdTrangThaiHopDongNavigation = dmtrangThaiHopDongs.FirstOrDefault(x => x.IdTrangThaiHopDong == item.IdTrangThaiHopDong);
                item.IdCanBoNavigation.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdCanBoNavigation.IdNguoi);
            });
            return tbHopDongThinhGiangs;
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
            List<TbHopDongThinhGiang> getall = await TbHopDongThinhGiangs();
            return View(getall);
        }

        // GET: HopDongThinhGiang
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbHopDongThinhGiang> getall = await TbHopDongThinhGiangs();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: HopDongThinhGiang/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbHopDongThinhGiangs = await TbHopDongThinhGiangs();
                var tbHopDongThinhGiang = tbHopDongThinhGiangs.FirstOrDefault(m => m.IdHopDongThinhGiang == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbHopDongThinhGiang == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbHopDongThinhGiang);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: HopDongThinhGiang/Create
       

        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo");
                ViewData["IdTrangThaiHopDong"] = new SelectList(await ApiServices_.GetAll<DmTrangThaiHopDong>("/api/dm/TrangThaiHopDong"), "IdTrangThaiHopDong", "TrangThaiHopDong");
              
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: HopDongThinhGiang/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHopDongThinhGiang,IdCanBo,MaHopDongThinhGiang,SoSoLaoDong,NgayCapSoLaoDong,NoiCapSoLaoDong,CoGiaTriTu,CoGiaTriDen,IdTrangThaiHopDong,TyLeThoiGianGiangDay")] TbHopDongThinhGiang tbHopDongThinhGiang)
        {
            if (await TbHopDongThinhGiangExists(tbHopDongThinhGiang.IdHopDongThinhGiang)) ModelState.AddModelError("IdHopDongThinhGiang", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbHopDongThinhGiang>("/api/cb/HopDongThinhGiang", tbHopDongThinhGiang);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbHopDongThinhGiang.IdCanBo);
            ViewData["IdTrangThaiHopDong"] = new SelectList(await ApiServices_.GetAll<DmTrangThaiHopDong>("/api/dm/TrangThaiHopDong"), "IdTrangThaiHopDong", "TrangThaiHopDong", tbHopDongThinhGiang.IdTrangThaiHopDong);
            return View(tbHopDongThinhGiang);
        }

        // GET: HopDongThinhGiang/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var tbHopDongThinhGiang = await ApiServices_.GetId<TbHopDongThinhGiang>("/api/cb/HopDongThinhGiang", id ?? 0);
            if (tbHopDongThinhGiang == null)
            {
                return NotFound();
            }
            ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name", tbHopDongThinhGiang.IdCanBo);
            ViewData["IdTrangThaiHopDong"] = new SelectList(await ApiServices_.GetAll<DmTrangThaiHopDong>("/api/dm/TrangThaiHopDong"), "IdTrangThaiHopDong", "TrangThaiHopDong", tbHopDongThinhGiang.IdTrangThaiHopDong);
            return View(tbHopDongThinhGiang);
        }

        // POST: HopDongThinhGiang/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdHopDongThinhGiang,IdCanBo,MaHopDongThinhGiang,SoSoLaoDong,NgayCapSoLaoDong,NoiCapSoLaoDong,CoGiaTriTu,CoGiaTriDen,IdTrangThaiHopDong,TyLeThoiGianGiangDay")] TbHopDongThinhGiang tbHopDongThinhGiang)
        {
            if (id != tbHopDongThinhGiang.IdHopDongThinhGiang)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbHopDongThinhGiang>("/api/cb/HopDongThinhGiang", id, tbHopDongThinhGiang);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbHopDongThinhGiangExists(tbHopDongThinhGiang.IdHopDongThinhGiang) == false)
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
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbHopDongThinhGiang.IdCanBo);
            ViewData["IdTrangThaiHopDong"] = new SelectList(await ApiServices_.GetAll<DmTrangThaiHopDong>("/api/dm/TrangThaiHopDong"), "IdTrangThaiHopDong", "TrangThaiHopDong", tbHopDongThinhGiang.IdTrangThaiHopDong);
            return View(tbHopDongThinhGiang);
        }

        // GET: HopDongThinhGiang/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbHopDongThinhGiangs = await TbHopDongThinhGiangs();
            var tbHopDongThinhGiang = tbHopDongThinhGiangs.FirstOrDefault(m => m.IdHopDongThinhGiang == id);
            if (tbHopDongThinhGiang == null)
            {
                return NotFound();
            }

            return View(tbHopDongThinhGiang);
        }

        // POST: HopDongThinhGiang/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbHopDongThinhGiang>("/api/cb/HopDongThinhGiang", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<bool> TbHopDongThinhGiangExists(int id)
        {
            var tbHopDongThinhGiangs = await ApiServices_.GetAll<TbHopDongThinhGiang>("/api/cb/HopDongThinhGiang");
            return tbHopDongThinhGiangs.Any(e => e.IdHopDongThinhGiang == id);
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

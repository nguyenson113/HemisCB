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
    public class HopDongController : Controller
    {
        private readonly ApiServices ApiServices_;

        public HopDongController(ApiServices services)
        {
            ApiServices_ = services;
        }


        //==========================================TẠO DANH SÁCH THÔNG TIN TỪ API===========================
        private async Task<List<TbHopDong>> TbHopDongs()
        {
            List<TbHopDong> tbHopDongs = await ApiServices_.GetAll<TbHopDong>("/api/cb/HopDong");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<DmLoaiHopDong> dmloaiHopDongs = await ApiServices_.GetAll<DmLoaiHopDong>("/api/dm/LoaiHopDong");
            List<DmTinhTrangHopDong> dmtinhTrangHopDongs = await ApiServices_.GetAll<DmTinhTrangHopDong>("/api/dm/TinhTrangHopDong");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            tbHopDongs.ForEach(item => {
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdLoaiHopDongNavigation = dmloaiHopDongs.FirstOrDefault(x => x.IdLoaiHopDong == item.IdLoaiHopDong);
                item.IdTinhTrangHopDongNavigation = dmtinhTrangHopDongs.FirstOrDefault(x => x.IdTinhTrangHopDong == item.IdTinhTrangHopDong);
                item.IdCanBoNavigation.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdCanBoNavigation.IdNguoi);
            });
            return tbHopDongs;
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
            List<TbHopDong> getall = await TbHopDongs();
            return View(getall);
        }


        // GET: HopDong
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbHopDong> getall = await TbHopDongs();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: HopDong/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbHopDongs = await TbHopDongs();
                var tbHopDong = tbHopDongs.FirstOrDefault(m => m.IdHopDong == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbHopDong == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbHopDong);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        // GET: HopDong/Create
       

        public async Task<IActionResult> Create()
        {
            // Tạo danh sách lựa chọn cho IdCanBo, IdChucDanhGiangVien, IdChucVu
            ViewData["IdLoaiHopDong"] = new SelectList(await ApiServices_.GetAll<DmLoaiHopDong>("/api/dm/LoaiHopDong"), "IdLoaiHopDong", "LoaiHopDong");
            ViewData["IdTinhTrangHopDong"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangHopDong>("/api/dm/TinhTrangHopDong"), "IdTinhTrangHopDong", "TinhTrangHopDong");
            ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name");
            return View();
        }

        // POST: HopDong/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHopDong,IdCanBo,SoHopDong,IdLoaiHopDong,SoQuyetDinh,NgayQuyetDinh,CoGiaTriTu,CoGiaTriDen,IdTinhTrangHopDong,LamViecToanThoiGian")] TbHopDong tbHopDong)
        {
            if (await TbHopDongExists(tbHopDong.IdHopDong)) ModelState.AddModelError("IdHopDong", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbHopDong>("/api/cb/HopDong", tbHopDong);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbHopDong.IdCanBo);
            ViewData["IdLoaiHopDong"] = new SelectList(await ApiServices_.GetAll<DmLoaiHopDong>("/api/dm/LoaiHopDong"), "IdLoaiHopDong", "LoaiHopDong", tbHopDong.IdLoaiHopDong);
            ViewData["IdTinhTrangHopDong"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangHopDong>("/api/dm/TinhTrangHopDong"), "IdTinhTrangHopDong", "TinhTrangHopDong", tbHopDong.IdTinhTrangHopDong);
            return View(tbHopDong);
        }

        // GET: HopDong/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbHopDong = await ApiServices_.GetId<TbHopDong>("/api/cb/HopDong ", id ?? 0);
            if (tbHopDong == null)
            {
                return NotFound();
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbHopDong.IdCanBo);
            ViewData["IdLoaiHopDong"] = new SelectList(await ApiServices_.GetAll<DmLoaiHopDong>("/api/dm/LoaiHopDong"), "IdLoaiHopDong", "LoaiHopDong", tbHopDong.IdLoaiHopDong);
            ViewData["IdTinhTrangHopDong"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangHopDong>("/api/dm/TinhTrangHopDong"), "IdTinhTrangHopDong", "TinhTrangHopDong", tbHopDong.IdTinhTrangHopDong);
            return View(tbHopDong);
        }

        // POST: HopDong/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdHopDong,IdCanBo,SoHopDong,IdLoaiHopDong,SoQuyetDinh,NgayQuyetDinh,CoGiaTriTu,CoGiaTriDen,IdTinhTrangHopDong,LamViecToanThoiGian")] TbHopDong tbHopDong)
        {
            if (id != tbHopDong.IdHopDong)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbHopDong>("/api/cb/HopDong", id, tbHopDong);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbHopDongExists(tbHopDong.IdHopDong) == false)
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
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbHopDong.IdCanBo);
            ViewData["IdLoaiHopDong"] = new SelectList(await ApiServices_.GetAll<DmLoaiHopDong>("/api/dm/LoaiHopDong"), "IdLoaiHopDong", "LoaiHopDong", tbHopDong.IdLoaiHopDong);
            ViewData["IdTinhTrangHopDong"] = new SelectList(await ApiServices_.GetAll<DmTinhTrangHopDong>("/api/dm/TinhTrangHopDong"), "IdTinhTrangHopDong", "TinhTrangHopDong", tbHopDong.IdTinhTrangHopDong);
            return View(tbHopDong);
        }

        // GET: HopDong/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbHopDongs = await TbHopDongs();
            var tbHopDong = tbHopDongs.FirstOrDefault(m => m.IdHopDong == id);
            if (tbHopDong == null)
            {
                return NotFound();
            }

            return View(tbHopDong);
        }


        // POST: HopDong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbHopDong>("/api/cb/HopDong", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        private async Task<bool> TbHopDongExists(int id)
        {
            var tbHopDongs = await ApiServices_.GetAll<TbHopDong>("/api/cb/HopDong");
            return tbHopDongs.Any(e => e.IdHopDong == id);
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

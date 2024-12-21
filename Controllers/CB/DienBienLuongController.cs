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
    public class DienBienLuongController : Controller
    {
        private readonly ApiServices ApiServices_;

        public DienBienLuongController(ApiServices services)
        {
            ApiServices_ = services;
        }

        //============================TẠO DANH SÁCH LẤY API========================
        private async Task<List<TbDienBienLuong>> TbDienBienLuongs()
        {
            List<TbDienBienLuong> tbDienBienLuongs = await ApiServices_.GetAll<TbDienBienLuong>("/api/cb/DienBienLuong");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<DmHeSoLuong> dmheSoLuongs = await ApiServices_.GetAll<DmHeSoLuong>("/api/dm/HeSoLuong");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            List<DmTrinhDoDaoTao> dmtrinhDoDaoTaos = await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao");
            tbDienBienLuongs.ForEach(item => {
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdHeSoLuongNavigation = dmheSoLuongs.FirstOrDefault(x => x.IdHeSoLuong == item.IdHeSoLuong);
                item.IdCanBoNavigation.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdCanBoNavigation.IdNguoi);
                item.IdTrinhDoDaoTaoNavigation = dmtrinhDoDaoTaos.FirstOrDefault(x => x.IdTrinhDoDaoTao == item.IdTrinhDoDaoTao);
            });
            return tbDienBienLuongs;
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
            List<TbDienBienLuong> getall = await TbDienBienLuongs();
            return View(getall);
        }

        // GET: DienBienLuong
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbDienBienLuong> getall = await TbDienBienLuongs();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: DienBienLuong/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // Kiểm tra nếu id là null
            if (id == null)
            {
                return NotFound();
            }

            // Tìm quá trình công tác theo id
            var tbDienBienLuongs = await TbDienBienLuongs();
            var tbDienBienLuong = tbDienBienLuongs.FirstOrDefault(m => m.IdDienBienLuong == id);

            // Kiểm tra nếu không tìm thấy
            if (tbDienBienLuong == null)
            {
                return NotFound();
            }

            // Trả về view với thông tin chi tiết
            return View(tbDienBienLuong);
        }


        // GET: DienBienLuong/Create
      

        public async Task<IActionResult> Create()
        {
            // Tạo danh sách lựa chọn cho IdCanBo, IdChucDanhGiangVien, IdChucVu
            ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao");
            ViewData["IdHeSoLuong"] = new SelectList(await ApiServices_.GetAll<DmHeSoLuong>("/api/dm/HeSoLuong"), "IdHeSoLuong", "HeSoLuong");
            //ViewData["IdBacLuong"] = new SelectList(await ApiServices_.GetAll<DmBacLuong1>("/api/dm/BacLuong"), "IdBacLuong", "BacLuong");
            ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name");
            return View();
        }



        // POST: DienBienLuong/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDienBienLuong,IdCanBo,IdTrinhDoDaoTao,NgayThangNam,IdBacLuong,IdHeSoLuong")] TbDienBienLuong tbDienBienLuong)
        {
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbDienBienLuong>("/api/cb/DienBienLuong", tbDienBienLuong);
                return RedirectToAction(nameof(Index));
            }

            ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao", tbDienBienLuong.IdTrinhDoDaoTao);
            ViewData["IdHeSoLuong"] = new SelectList(await ApiServices_.GetAll<DmHeSoLuong>("/api/dm/HeSoLuong"), "IdHeSoLuong", "HeSoLuong", tbDienBienLuong.IdHeSoLuong);
            //ViewData["IdBacLuong"] = new SelectList(await ApiServices_.GetAll<DmBacLuong1>("/api/dm/BacLuong"), "IdBacLuong", "BacLuong", tbDienBienLuong.IdBacLuong);
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbDienBienLuong.IdCanBo);
            return View(tbDienBienLuong);
        }

        // GET: DienBienLuong/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbDienBienLuong = await ApiServices_.GetId<TbDienBienLuong>("/api/cb/DienBienLuong", id ?? 0);
            if (tbDienBienLuong == null)
            {
                return NotFound();
            }
            ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao", tbDienBienLuong.IdTrinhDoDaoTao);
            ViewData["IdHeSoLuong"] = new SelectList(await ApiServices_.GetAll<DmHeSoLuong>("/api/dm/HeSoLuong"), "IdHeSoLuong", "HeSoLuong", tbDienBienLuong.IdHeSoLuong);
            //ViewData["IdBacLuong"] = new SelectList(await ApiServices_.GetAll<DmBacLuong1>("/api/dm/BacLuong"), "IdBacLuong", "BacLuong", tbDienBienLuong.IdBacLuong);
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbDienBienLuong.IdCanBo);
            return View(tbDienBienLuong);
        }

        // POST: DienBienLuong/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDienBienLuong,IdCanBo,IdTrinhDoDaoTao,NgayThangNam,IdBacLuong,IdHeSoLuong")] TbDienBienLuong tbDienBienLuong)
        {
            if (id != tbDienBienLuong.IdDienBienLuong)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbDienBienLuong>("/api/cb/DienBienLuong", id, tbDienBienLuong);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbDienBienLuongExists(tbDienBienLuong.IdDienBienLuong) == false)
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
            ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao", tbDienBienLuong.IdTrinhDoDaoTao);
            ViewData["IdHeSoLuong"] = new SelectList(await ApiServices_.GetAll<DmHeSoLuong>("/api/dm/HeSoLuong"), "IdHeSoLuong", "HeSoLuong", tbDienBienLuong.IdHeSoLuong);
            //ViewData["IdBacLuong"] = new SelectList(await ApiServices_.GetAll<DmBacLuong1>("/api/dm/BacLuong"), "IdBacLuong", "BacLuong", tbDienBienLuong.IdBacLuong);
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbDienBienLuong.IdCanBo);
            return View(tbDienBienLuong);
        }

        // GET: DienBienLuong/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbDienBienLuongs = await TbDienBienLuongs();
            var tbDienBienLuong = tbDienBienLuongs.FirstOrDefault(m => m.IdDienBienLuong == id);
            if (tbDienBienLuong == null)
            {
                return NotFound();
            }

            return View(tbDienBienLuong);
        }

        // POST: DienBienLuong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbDienBienLuong>("/api/cb/DienBienLuong", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<bool> TbDienBienLuongExists(int id)
        {
            var tbDienBienLuongs = await ApiServices_.GetAll<TbDienBienLuong>("/api/cb/DienBienLuong");
            return tbDienBienLuongs.Any(e => e.IdDienBienLuong == id);
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

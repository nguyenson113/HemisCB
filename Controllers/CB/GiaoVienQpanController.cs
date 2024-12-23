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
    public class GiaoVienQpanController : Controller
    {
        private readonly ApiServices ApiServices_;

        public GiaoVienQpanController(ApiServices services)
        {
            ApiServices_ = services;
        }

        //==========================================TẠO DANH SÁCH THÔNG TIN TỪ API===========================
        private async Task<List<TbGiaoVienQpan>> TbGiaoVienQpans()
        {
            List<TbGiaoVienQpan> tbGiaoVienQpans = await ApiServices_.GetAll<TbGiaoVienQpan>("/api/cb/GiaoVienQPAN");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<DmLoaiGiangVienQuocPhong> dmloaiGiangVienQuocPhongs = await ApiServices_.GetAll<DmLoaiGiangVienQuocPhong>("/api/dm/LoaiGiangVienQuocPhong");
            List<DmQuanHam> dmquanHams = await ApiServices_.GetAll<DmQuanHam>("/api/dm/QuanHam");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            tbGiaoVienQpans.ForEach(item => {
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdLoaiGiangVienQpNavigation = dmloaiGiangVienQuocPhongs.FirstOrDefault(x => x.IdLoaiGiangVienQuocPhong == item.IdLoaiGiangVienQp);
                item.IdQuanHamNavigation = dmquanHams.FirstOrDefault(x => x.IdQuanHam == item.IdQuanHam);
                item.IdCanBoNavigation.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdCanBoNavigation.IdNguoi);
            });
            return tbGiaoVienQpans;
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
            List<TbGiaoVienQpan> getall = await TbGiaoVienQpans();
            return View(getall);
        }



        // GET: GiaoVienQpan
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbGiaoVienQpan> getall = await TbGiaoVienQpans();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: GiaoVienQpan/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbGiaoVienQpans = await TbGiaoVienQpans();
                var tbGiaoVienQpan = tbGiaoVienQpans.FirstOrDefault(m => m.IdGiaoVienQpan == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbGiaoVienQpan == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbGiaoVienQpan);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: GiaoVienQpan/Create
     
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name");
                ViewData["IdLoaiGiangVienQp"] = new SelectList(await ApiServices_.GetAll<DmLoaiGiangVienQuocPhong>("/api/dm/LoaiGiangVienQuocPhong"), "IdLoaiGiangVienQuocPhong", "LoaiGiangVienQuocPhong");
                ViewData["IdQuanHam"] = new SelectList(await ApiServices_.GetAll<DmQuanHam>("/api/dm/QuanHam"), "IdQuanHam", "QuanHam");
                
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: GiaoVienQpan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGiaoVienQpan,IdCanBo,NamBatDauBietPhai,SoNamBietPhai,IdLoaiGiangVienQp,DaoTaoGdqpan,IdQuanHam,SoTruongCongTac")] TbGiaoVienQpan tbGiaoVienQpan)
        {
            if (await TbGiaoVienQpanExists(tbGiaoVienQpan.IdGiaoVienQpan)) ModelState.AddModelError("IdGiaoVienQpan", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbGiaoVienQpan>("/api/cb/GiaoVienQPAN", tbGiaoVienQpan);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbGiaoVienQpan.IdCanBo);
            ViewData["IdLoaiGiangVienQp"] = new SelectList(await ApiServices_.GetAll<DmLoaiGiangVienQuocPhong>("/api/dm/LoaiGiangVienQuocPhong"), "IdLoaiGiangVienQuocPhong", "LoaiGiangVienQuocPhong", tbGiaoVienQpan.IdLoaiGiangVienQp);
            ViewData["IdQuanHam"] = new SelectList(await ApiServices_.GetAll<DmQuanHam>("/api/dm/QuanHam"), "IdQuanHam", "QuanHam", tbGiaoVienQpan.IdQuanHam);
            return View(tbGiaoVienQpan);
        }

        // GET: GiaoVienQpan/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbGiaoVienQpan = await ApiServices_.GetId<TbGiaoVienQpan>("/api/cb/GiaoVienQPAN", id ?? 0);
            if (tbGiaoVienQpan == null)
            {
                return NotFound();
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbGiaoVienQpan.IdCanBo);
            ViewData["IdLoaiGiangVienQp"] = new SelectList(await ApiServices_.GetAll<DmLoaiGiangVienQuocPhong>("/api/dm/LoaiGiangVienQuocPhong"), "IdLoaiGiangVienQuocPhong", "LoaiGiangVienQuocPhong", tbGiaoVienQpan.IdLoaiGiangVienQp);
            ViewData["IdQuanHam"] = new SelectList(await ApiServices_.GetAll<DmQuanHam>("/api/dm/QuanHam"), "IdQuanHam", "QuanHam", tbGiaoVienQpan.IdQuanHam);
            return View(tbGiaoVienQpan);
        }

        // POST: GiaoVienQpan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdGiaoVienQpan,IdCanBo,NamBatDauBietPhai,SoNamBietPhai,IdLoaiGiangVienQp,DaoTaoGdqpan,IdQuanHam,SoTruongCongTac")] TbGiaoVienQpan tbGiaoVienQpan)
        {
            if (id != tbGiaoVienQpan.IdGiaoVienQpan)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbGiaoVienQpan>("/api/cb/GiaoVienQPAN", id, tbGiaoVienQpan);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbGiaoVienQpanExists(tbGiaoVienQpan.IdGiaoVienQpan) == false)
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
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbGiaoVienQpan.IdCanBo);
            ViewData["IdLoaiGiangVienQp"] = new SelectList(await ApiServices_.GetAll<DmLoaiGiangVienQuocPhong>("/api/dm/LoaiGiangVienQuocPhong"), "IdLoaiGiangVienQuocPhong", "LoaiGiangVienQuocPhong", tbGiaoVienQpan.IdLoaiGiangVienQp);
            ViewData["IdQuanHam"] = new SelectList(await ApiServices_.GetAll<DmQuanHam>("/api/dm/QuanHam"), "IdQuanHam", "QuanHam", tbGiaoVienQpan.IdQuanHam);
            return View(tbGiaoVienQpan);
        }

        // GET: GiaoVienQpan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbGiaoVienQpans = await TbGiaoVienQpans();
            var tbGiaoVienQpan = tbGiaoVienQpans.FirstOrDefault(m => m.IdGiaoVienQpan == id);
            if (tbGiaoVienQpan == null)
            {
                return NotFound();
            }

            return View(tbGiaoVienQpan);
        }

        // POST: QuaTrinhDaoTaoCuaCanBo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbQuaTrinhDaoTaoCuaCanBo>("/api/cb/GiaoVienQPAN", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<bool> TbGiaoVienQpanExists(int id)
        {
            var tbGiaoVienQpans = await ApiServices_.GetAll<TbGiaoVienQpan>("/api/cb/GiaoVienQPAN");
            return tbGiaoVienQpans.Any(e => e.IdGiaoVienQpan == id);
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
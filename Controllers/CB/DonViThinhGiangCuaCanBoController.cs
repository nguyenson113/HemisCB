using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HemisCB.Models;
using HemisCB.API;
using HemisCB.API;
using HemisCB.Models.DM;
using Newtonsoft.Json;

namespace HemisCB.Controllers.CB
{
    public class DonViThinhGiangCuaCanBoController : Controller
    {
        private readonly ApiServices ApiServices_;

        public DonViThinhGiangCuaCanBoController(ApiServices services)
        {
            ApiServices_ = services;
        }

        //==========================================TẠO DANH SÁCH THÔNG TIN TỪ API===========================
        private async Task<List<TbDonViThinhGiangCuaCanBo>> TbDonViThinhGiangCuaCanBos()
        {
            List<TbDonViThinhGiangCuaCanBo> tbDonViThinhGiangCuaCanBos = await ApiServices_.GetAll<TbDonViThinhGiangCuaCanBo>("/api/cb/DonViThinhGiangCuaCanBo");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            
            tbDonViThinhGiangCuaCanBos.ForEach(item => {
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                
            });
            return tbDonViThinhGiangCuaCanBos;
        }

        public async Task<IActionResult> Statistics()
        {
            List<TbDonViThinhGiangCuaCanBo> getall = await TbDonViThinhGiangCuaCanBos();
            return View(getall);
        }


        // GET: DonViThinhGiangCuaCanBo
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbDonViThinhGiangCuaCanBo> getall = await TbDonViThinhGiangCuaCanBos();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: DonViThinhGiangCuaCanBo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbDonViThinhGiangCuaCanBos = await TbDonViThinhGiangCuaCanBos();
                var tbDonViThinhGiangCuaCanBo = tbDonViThinhGiangCuaCanBos.FirstOrDefault(m => m.IdDonViThinhGiangCuaCanBo== id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbDonViThinhGiangCuaCanBo == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbDonViThinhGiangCuaCanBo);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: DonViThinhGiangCuaCanBo/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo");
                
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: DonViThinhGiangCuaCanBo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdDonViThinhGiangCuaCanBo,IdCanBo,DonViThinhGiang,SoHopDongThinhGiang,ThoiGianBatDau,ThoiGianKetThuc,ThamNienGiangDay")] TbDonViThinhGiangCuaCanBo tbDonViThinhGiangCuaCanBo)
        {
            if (await TbDonViThinhGiangCuaCanBoExists(tbDonViThinhGiangCuaCanBo.IdDonViThinhGiangCuaCanBo)) ModelState.AddModelError("IdDonViThinhGiangCuaCanBo", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbDonViThinhGiangCuaCanBo>("/api/cb/DonViThinhGiangCuaCanBo", tbDonViThinhGiangCuaCanBo);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbDonViThinhGiangCuaCanBo.IdCanBo);
            return View(tbDonViThinhGiangCuaCanBo);
        }

        // GET: DonViThinhGiangCuaCanBo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbDonViThinhGiangCuaCanBo = await ApiServices_.GetId<TbDonViThinhGiangCuaCanBo>("/api/cb/DonViThinhGiangCuaCanBo ", id ?? 0);
            if (tbDonViThinhGiangCuaCanBo == null)
            {
                return NotFound();
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbDonViThinhGiangCuaCanBo.IdCanBo);
            return View(tbDonViThinhGiangCuaCanBo);
        }

        // POST: DonViThinhGiangCuaCanBo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdDonViThinhGiangCuaCanBo,IdCanBo,DonViThinhGiang,SoHopDongThinhGiang,ThoiGianBatDau,ThoiGianKetThuc,ThamNienGiangDay")] TbDonViThinhGiangCuaCanBo tbDonViThinhGiangCuaCanBo)
        {
            if (id != tbDonViThinhGiangCuaCanBo.IdDonViThinhGiangCuaCanBo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbDonViThinhGiangCuaCanBo>("/api/cb/DonViThinhGiangCuaCanBo", id, tbDonViThinhGiangCuaCanBo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbDonViThinhGiangCuaCanBoExists(tbDonViThinhGiangCuaCanBo.IdDonViThinhGiangCuaCanBo) == false)
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
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbDonViThinhGiangCuaCanBo.IdCanBo);

            return View(tbDonViThinhGiangCuaCanBo);
        }

        // GET: DonViThinhGiangCuaCanBo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbDonViThinhGiangCuaCanBos = await ApiServices_.GetAll< TbDonViThinhGiangCuaCanBo > ("/api/cb/DonViThinhGiangCuaCanBo");
            var tbDonViThinhGiangCuaCanBo = tbDonViThinhGiangCuaCanBos.FirstOrDefault(m => m.IdDonViThinhGiangCuaCanBo == id);
            if (tbDonViThinhGiangCuaCanBo == null)
            {
                return NotFound();
            }

            return View(tbDonViThinhGiangCuaCanBo);
        }

        // POST: DonViThinhGiangCuaCanBo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbDonViThinhGiangCuaCanBo>("/api/cb/DonViThinhGiangCuaCanBo", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<bool> TbDonViThinhGiangCuaCanBoExists(int id)
        {
            var tbDonViThinhGiangCuaCanBos = await ApiServices_.GetAll<TbDonViThinhGiangCuaCanBo>("/api/cb/DonViThinhGiangCuaCanBo");
            return tbDonViThinhGiangCuaCanBos.Any(e => e.IdDonViThinhGiangCuaCanBo == id);
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

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
    public class TrinhDoTiengDanTocController : Controller
    {
        private readonly ApiServices ApiServices_;

        public TrinhDoTiengDanTocController(ApiServices services)
        {
            ApiServices_ = services;
        }


        //========================== TẠO LIST LẤY DỮ LIỆU TỪ API ====================================
        private async Task<List<TbTrinhDoTiengDanToc>> TbTrinhDoTiengDanTocs()
        {
            List<TbTrinhDoTiengDanToc> tbTrinhDoTiengDanTocs = await ApiServices_.GetAll<TbTrinhDoTiengDanToc>("/api/cb/TrinhDoTiengDanToc");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<DmKhungNangLucNgoaiNgu> dmkhungNangLucNgoaiNgus = await ApiServices_.GetAll<DmKhungNangLucNgoaiNgu>("/api/dm/KhungNangLucNgoaiNgu");
            List<DmTiengDanToc> dmtiengDanTocs = await ApiServices_.GetAll<DmTiengDanToc>("/api/dm/TiengDanToc");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            tbTrinhDoTiengDanTocs.ForEach(item => {
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdKhungNangLucNgoaiNguNavigation = dmkhungNangLucNgoaiNgus.FirstOrDefault(x => x.IdKhungNangLucNgoaiNgu == item.IdKhungNangLucNgoaiNgu);
                item.IdTiengDanTocNavigation = dmtiengDanTocs.FirstOrDefault(x => x.IdTiengDanToc == item.IdTiengDanToc);
                item.IdCanBoNavigation.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdCanBoNavigation.IdNguoi);
            });
            return tbTrinhDoTiengDanTocs;
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
            List<TbTrinhDoTiengDanToc> getall = await TbTrinhDoTiengDanTocs();
            return View(getall);
        }

        // GET: TrinhDoTiengDanToc
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbTrinhDoTiengDanToc> getall = await TbTrinhDoTiengDanTocs();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: TrinhDoTiengDanToc/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbTrinhDoTiengDanTocs = await TbTrinhDoTiengDanTocs();
                var tbTrinhDoTiengDanToc = tbTrinhDoTiengDanTocs.FirstOrDefault(m => m.IdTrinhDoTiengDanToc == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbTrinhDoTiengDanToc == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbTrinhDoTiengDanToc);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        // GET: TrinhDoTiengDanToc/Create

        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name");
                ViewData["IdKhungNangLucNgoaiNgu"] = new SelectList(await ApiServices_.GetAll<DmKhungNangLucNgoaiNgu>("/api/dm/KhungNangLucNgoaiNgu"), "IdKhungNangLucNgoaiNgu", "TenKhungNangLucNgoaiNgu");
                ViewData["IdTiengDanToc"] = new SelectList(await ApiServices_.GetAll<DmTiengDanToc>("/api/dm/TiengDanToc"), "IdTiengDanToc", "TiengDanToc");
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: TrinhDoTiengDanToc/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTrinhDoTiengDanToc,IdCanBo,IdTiengDanToc,IdKhungNangLucNgoaiNgu")] TbTrinhDoTiengDanToc tbTrinhDoTiengDanToc)
        {
            if (await TbTrinhDoTiengDanTocExists(tbTrinhDoTiengDanToc.IdTrinhDoTiengDanToc)) ModelState.AddModelError("IdTrinhDoTiengDanToc", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbTrinhDoTiengDanToc>("/api/cb/TrinhDoTiengDanToc", tbTrinhDoTiengDanToc);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbTrinhDoTiengDanToc.IdCanBo);
            ViewData["IdKhungNangLucNgoaiNgu"]= new SelectList(await ApiServices_.GetAll<DmKhungNangLucNgoaiNgu>("/api/dm/KhungNangLucNgoaiNgu"), "IdKhungNangLucNgoaiNgu", "TenKhungNangLucNgoaiNgu", tbTrinhDoTiengDanToc.IdKhungNangLucNgoaiNgu);
            ViewData["IdTiengDanToc"] = new SelectList(await ApiServices_.GetAll<DmTiengDanToc>("/api/dm/TiengDanToc"), "IdTiengDanToc", "TiengDanToc", tbTrinhDoTiengDanToc.IdTiengDanToc);
          
            return View(tbTrinhDoTiengDanToc);

        }

        // GET: TrinhDoTiengDanToc/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbTrinhDoTiengDanToc = await ApiServices_.GetId<TbTrinhDoTiengDanToc>("/api/cb/TrinhDoTiengDanToc", id ?? 0);
            if (tbTrinhDoTiengDanToc == null)
            {
                return NotFound();
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbTrinhDoTiengDanToc.IdCanBo);
            ViewData["IdKhungNangLucNgoaiNgu"] = new SelectList(await ApiServices_.GetAll<DmKhungNangLucNgoaiNgu>("/api/dm/KhungNangLucNgoaiNgu"), "IdKhungNangLucNgoaiNgu", "TenKhungNangLucNgoaiNgu", tbTrinhDoTiengDanToc.IdKhungNangLucNgoaiNgu);
            ViewData["IdTiengDanToc"] = new SelectList(await ApiServices_.GetAll<DmTiengDanToc>("/api/dm/TiengDanToc"), "IdTiengDanToc", "TiengDanToc", tbTrinhDoTiengDanToc.IdTiengDanToc);
            return View(tbTrinhDoTiengDanToc);
        }

        // POST: TrinhDoTiengDanToc/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTrinhDoTiengDanToc,IdCanBo,IdTiengDanToc,IdKhungNangLucNgoaiNgu")] TbTrinhDoTiengDanToc tbTrinhDoTiengDanToc)
        {
            if (id != tbTrinhDoTiengDanToc.IdTrinhDoTiengDanToc)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbTrinhDoTiengDanToc>("/api/cb/TrinhDoTiengDanToc", id, tbTrinhDoTiengDanToc);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbTrinhDoTiengDanTocExists(tbTrinhDoTiengDanToc.IdTrinhDoTiengDanToc) == false)
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
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbTrinhDoTiengDanToc.IdCanBo);
            ViewData["IdKhungNangLucNgoaiNgu"] = new SelectList(await ApiServices_.GetAll<DmKhungNangLucNgoaiNgu>("/api/dm/KhungNangLucNgoaiNgu"), "IdKhungNangLucNgoaiNgu", "TenKhungNangLucNgoaiNgu", tbTrinhDoTiengDanToc.IdKhungNangLucNgoaiNgu);
            ViewData["IdTiengDanToc"] = new SelectList(await ApiServices_.GetAll<DmTiengDanToc>("/api/dm/TiengDanToc"), "IdTiengDanToc", "TiengDanToc", tbTrinhDoTiengDanToc.IdTiengDanToc);
            return View(tbTrinhDoTiengDanToc);
        }

        // GET: TrinhDoTiengDanToc/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var tbTrinhDoTiengDanTocs = await TbTrinhDoTiengDanTocs();
            var tbTrinhDoTiengDanToc = tbTrinhDoTiengDanTocs.FirstOrDefault(m => m.IdTrinhDoTiengDanToc == id);
            if (tbTrinhDoTiengDanToc == null)
            {
                return NotFound();
            }

            return View(tbTrinhDoTiengDanToc);
        }

        // POST: TrinhDoTiengDanToc/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbTrinhDoTiengDanToc>("/api/cb/TrinhDoTiengDanToc", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<bool> TbTrinhDoTiengDanTocExists(int id)
        {
            var tbTrinhDoTiengDanTocs = await ApiServices_.GetAll<TbTrinhDoTiengDanToc>("/api/cb/TrinhDoTiengDanToc");
            return tbTrinhDoTiengDanTocs.Any(e => e.IdTrinhDoTiengDanToc == id);
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

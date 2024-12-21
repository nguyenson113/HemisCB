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
    public class NganhGiangDayCuaCanBoController : Controller
    {
        private readonly ApiServices ApiServices_;

        public NganhGiangDayCuaCanBoController(ApiServices services)
        {
            ApiServices_ = services;
        }


        //============================TẠO DANH SÁCH LẤY API========================
        private async Task<List<TbNganhGiangDayCuaCanBo>> TbNganhGiangDayCuaCanBos()
        {
            List<TbNganhGiangDayCuaCanBo> tbNganhGiangDayCuaCanBos = await ApiServices_.GetAll<TbNganhGiangDayCuaCanBo>("/api/cb/NganhGiangDayCuaCanBo");
            List<DmNganhDaoTao> dmnganhDaoTaos = await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao");
            List<DmTrinhDoDaoTao> dmtrinhDoDaoTaos = await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            tbNganhGiangDayCuaCanBos.ForEach(item => {
                item.IdTrinhDoDaoTaoNavigation = dmtrinhDoDaoTaos.FirstOrDefault(x => x.IdTrinhDoDaoTao == item.IdTrinhDoDaoTao);
                item.IdNganhNavigation = dmnganhDaoTaos.FirstOrDefault(x => x.IdNganhDaoTao == item.IdNganh);
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdCanBoNavigation.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdCanBoNavigation.IdNguoi);
            });
            return tbNganhGiangDayCuaCanBos;
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
            List<TbNganhGiangDayCuaCanBo> getall = await TbNganhGiangDayCuaCanBos();
            return View(getall);
        }



        // GET: NganhGiangDayCuaCanBo
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbNganhGiangDayCuaCanBo> getall = await TbNganhGiangDayCuaCanBos();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: NganhGiangDayCuaCanBo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbNganhGiangDayCuaCanBos = await TbNganhGiangDayCuaCanBos();
                var tbNganhGiangDayCuaCanBo = tbNganhGiangDayCuaCanBos.FirstOrDefault(m => m.IdNganhGiangDayCuaCanBo == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbNganhGiangDayCuaCanBo == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbNganhGiangDayCuaCanBo);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // GET: NganhGiangDayCuaCanBo/Create
   
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name");
                ViewData["IdNganh"] = new SelectList(await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao"), "IdNganhDaoTao", "NganhDaoTao");
                ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao");
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        // POST: NganhGiangDayCuaCanBo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNganhGiangDayCuaCanBo,IdCanBo,IdTrinhDoDaoTao,IdNganh,LaNganhChinh,DonViThinhGiang")] TbNganhGiangDayCuaCanBo tbNganhGiangDayCuaCanBo)
        {
            if (await TbNganhGiangDayCuaCanBoExists(tbNganhGiangDayCuaCanBo.IdNganhGiangDayCuaCanBo)) ModelState.AddModelError("IdNganhGiangDayCuaCanBo", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbNganhGiangDayCuaCanBo>("/api/cb/NganhGiangDayCuaCanBo", tbNganhGiangDayCuaCanBo);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbNganhGiangDayCuaCanBo.IdCanBo);
            ViewData["IdNganh"] = new SelectList(await ApiServices_.GetAll<DmLoaiHinhDaoTao>("/api/dm/NganhDaoTao"), "IdNganhDaoTao", "NganhDaoTao", tbNganhGiangDayCuaCanBo.IdNganh);
            ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao", tbNganhGiangDayCuaCanBo.IdTrinhDoDaoTao);
            return View(tbNganhGiangDayCuaCanBo);
        }

        // GET: NganhGiangDayCuaCanBo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbNganhGiangDayCuaCanBo = await ApiServices_.GetId<TbNganhGiangDayCuaCanBo>("/api/cb/NganhGiangDayCuaCanBo", id ?? 0);
            if (tbNganhGiangDayCuaCanBo == null)
            {
                return NotFound();
            }
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbNganhGiangDayCuaCanBo.IdCanBo);
            ViewData["IdNganh"] = new SelectList(await ApiServices_.GetAll<DmLoaiHinhDaoTao>("/api/dm/NganhDaoTao"), "IdNganhDaoTao", "NganhDaoTao", tbNganhGiangDayCuaCanBo.IdNganh);
            ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao", tbNganhGiangDayCuaCanBo.IdTrinhDoDaoTao);
            return View(tbNganhGiangDayCuaCanBo);
        }

        // POST: NganhGiangDayCuaCanBo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNganhGiangDayCuaCanBo,IdCanBo,IdTrinhDoDaoTao,IdNganh,LaNganhChinh,DonViThinhGiang")] TbNganhGiangDayCuaCanBo tbNganhGiangDayCuaCanBo)
        {
            if (id != tbNganhGiangDayCuaCanBo.IdNganhGiangDayCuaCanBo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbNganhGiangDayCuaCanBo>("/api/cb/NganhGiangDayCuaCanBo", id, tbNganhGiangDayCuaCanBo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbNganhGiangDayCuaCanBoExists(tbNganhGiangDayCuaCanBo.IdNganhGiangDayCuaCanBo) == false)
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
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbNganhGiangDayCuaCanBo.IdCanBo);
            ViewData["IdNganh"] = new SelectList(await ApiServices_.GetAll<DmLoaiHinhDaoTao>("/api/dm/NganhDaoTao"), "IdNganhDaoTao", "NganhDaoTao", tbNganhGiangDayCuaCanBo.IdNganh);
            ViewData["IdTrinhDoDaoTao"] = new SelectList(await ApiServices_.GetAll<DmTrinhDoDaoTao>("/api/dm/TrinhDoDaoTao"), "IdTrinhDoDaoTao", "TrinhDoDaoTao", tbNganhGiangDayCuaCanBo.IdTrinhDoDaoTao);
            return View(tbNganhGiangDayCuaCanBo);
        }

        // GET: NganhGiangDayCuaCanBo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbNganhGiangDayCuaCanBos = await ApiServices_.GetAll<TbNganhGiangDayCuaCanBo>("/api/cb/NganhGiangDayCuaCanBo");
            var tbNganhGiangDayCuaCanBo = tbNganhGiangDayCuaCanBos.FirstOrDefault(m => m.IdNganhGiangDayCuaCanBo == id);
            if (tbNganhGiangDayCuaCanBo == null)
            {
                return NotFound();
            }

            return View(tbNganhGiangDayCuaCanBo);
        }


        // POST: NganhGiangDayCuaCanBo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbNganhGiangDayCuaCanBo>("/api/cb/NganhGiangDayCuaCanBo", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private async Task<bool> TbNganhGiangDayCuaCanBoExists(int id)
        {
            var tbNganhGiangDayCuaCanBos = await ApiServices_.GetAll<TbNganhGiangDayCuaCanBo>("/api/cb/NganhGiangDayCuaCanBo");
            return tbNganhGiangDayCuaCanBos.Any(e => e.IdNganhGiangDayCuaCanBo == id);
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

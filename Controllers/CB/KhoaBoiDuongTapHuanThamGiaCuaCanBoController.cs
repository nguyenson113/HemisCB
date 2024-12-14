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
    public class KhoaBoiDuongTapHuanThamGiaCuaCanBoController : Controller
    {
        private readonly ApiServices ApiServices_;

        public KhoaBoiDuongTapHuanThamGiaCuaCanBoController(ApiServices services)
        {
            ApiServices_ = services;
        }

        //============================TẠO DANH SÁCH LẤY API========================
        private async Task<List<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo>> TbKhoaBoiDuongTapHuanThamGiaCuaCanBos()
        {
            List<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo> tbKhoaBoiDuongTapHuanThamGiaCuaCanBos = await ApiServices_.GetAll<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo>("/api/cb/KhoaBoiDuongTapHuanThamGiaCuaCanBo");
            List<DmLoaiBoiDuong> dmloaiBoiDuongs = await ApiServices_.GetAll<DmLoaiBoiDuong>("/api/dm/LoaiBoiDuong");
            List<DmNguonKinhPhi> dmnguonKinhPhis = await ApiServices_.GetAll<DmNguonKinhPhi>("/api/dm/NguonKinhPhi");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            tbKhoaBoiDuongTapHuanThamGiaCuaCanBos.ForEach(item => {
                item.IdLoaiBoiDuongNavigation = dmloaiBoiDuongs.FirstOrDefault(x => x.IdLoaiBoiDuong == item.IdLoaiBoiDuong);
                item.IdNguonKinhPhiNavigation = dmnguonKinhPhis.FirstOrDefault(x => x.IdNguonKinhPhi == item.IdNguonKinhPhi);
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
            });
            return tbKhoaBoiDuongTapHuanThamGiaCuaCanBos;
        }

        // GET: KhoaBoiDuongTapHuanThamGiaCuaCanBo
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo> getall = await TbKhoaBoiDuongTapHuanThamGiaCuaCanBos();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: KhoaBoiDuongTapHuanThamGiaCuaCanBo/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbKhoaBoiDuongTapHuanThamGiaCuaCanBos = await TbKhoaBoiDuongTapHuanThamGiaCuaCanBos();
                var tbKhoaBoiDuongTapHuanThamGiaCuaCanBo = tbKhoaBoiDuongTapHuanThamGiaCuaCanBos.FirstOrDefault(m => m.IdKhoaBoiDuongTapHuanThamGiaCuaCanBo == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbKhoaBoiDuongTapHuanThamGiaCuaCanBo == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbKhoaBoiDuongTapHuanThamGiaCuaCanBo);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        // GET: KhoaBoiDuongTapHuanThamGiaCuaCanBo/Create
        //sửa các selectlist 
       

        public async Task<IActionResult> Create()
        {
            try
            {
                ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo");
                ViewData["IdLoaiBoiDuong"] = new SelectList(await ApiServices_.GetAll<DmLoaiBoiDuong>("/api/dm/LoaiBoiDuong"), "IdLoaiBoiDuong", "LoaiBoiDuong");
                ViewData["IdNguonKinhPhi"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhi>("/api/dm/NguonKinhPhi"), "IdNguonKinhPhi", "NguonKinhPhi");
              
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }


        // POST: KhoaBoiDuongTapHuanThamGiaCuaCanBo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdKhoaBoiDuongTapHuanThamGiaCuaCanBo,IdCanBo,TenKhoaBoiDuongTapHuan,DonViToChuc,IdLoaiBoiDuong,DiaDiemToChuc,ThoiGianBatDau,ThoiGianKetThuc,IdNguonKinhPhi,ChungChi,NgayCap")] TbKhoaBoiDuongTapHuanThamGiaCuaCanBo tbKhoaBoiDuongTapHuanThamGiaCuaCanBo)
        {
            if (await TbKhoaBoiDuongTapHuanThamGiaCuaCanBoExists(tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdKhoaBoiDuongTapHuanThamGiaCuaCanBo)) ModelState.AddModelError("IdKhoaBoiDuongTapHuanThamGiaCuaCanBo", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo>("/api/cb/KhoaBoiDuongTapHuanThamGiaCuaCanBo", tbKhoaBoiDuongTapHuanThamGiaCuaCanBo);
                return RedirectToAction(nameof(Index));
            }
            try
            {
               
                ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdCanBo);
                ViewData["IdLoaiBoiDuong"] = new SelectList(await ApiServices_.GetAll<DmLoaiBoiDuong>("/api/dm/LoaiBoiDuong"), "IdLoaiBoiDuong", "LoaiBoiDuong", tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdLoaiBoiDuong);
                ViewData["IdNguonKinhPhi"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhi>("/api/dm/NguonKinhPhi"), "IdNguonKinhPhi", "NguonKinhPhi", tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdNguonKinhPhi);
                return View(tbKhoaBoiDuongTapHuanThamGiaCuaCanBo);
            }catch(Exception ex)
            {
                return BadRequest();
            }
        }

        // GET: KhoaBoiDuongTapHuanThamGiaCuaCanBo/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var tbKhoaBoiDuongTapHuanThamGiaCuaCanBo = await ApiServices_.GetId<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo>("/api/cb/KhoaBoiDuongTapHuanThamGiaCuaCanBo", id ?? 0);
                if (tbKhoaBoiDuongTapHuanThamGiaCuaCanBo == null)
                {
                    return NotFound();
                }
                ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdCanBo);
                ViewData["IdLoaiBoiDuong"] = new SelectList(await ApiServices_.GetAll<DmLoaiBoiDuong>("/api/dm/LoaiBoiDuong"), "IdLoaiBoiDuong", "LoaiBoiDuong", tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdLoaiBoiDuong);
                ViewData["IdNguonKinhPhi"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhi>("/api/dm/NguonKinhPhi"), "IdNguonKinhPhi", "NguonKinhPhi", tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdNguonKinhPhi);
                return View(tbKhoaBoiDuongTapHuanThamGiaCuaCanBo);
            }catch(Exception ex)
            {
                return BadRequest();
            }
        }

        // POST: KhoaBoiDuongTapHuanThamGiaCuaCanBo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdKhoaBoiDuongTapHuanThamGiaCuaCanBo,IdCanBo,TenKhoaBoiDuongTapHuan,DonViToChuc,IdLoaiBoiDuong,DiaDiemToChuc,ThoiGianBatDau,ThoiGianKetThuc,IdNguonKinhPhi,ChungChi,NgayCap")] TbKhoaBoiDuongTapHuanThamGiaCuaCanBo tbKhoaBoiDuongTapHuanThamGiaCuaCanBo)
        {
            try
            {
                if (id != tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdKhoaBoiDuongTapHuanThamGiaCuaCanBo)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        await ApiServices_.Update<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo>("/api/cb/KhoaBoiDuongTapHuanThamGiaCuaCanBo", id, tbKhoaBoiDuongTapHuanThamGiaCuaCanBo);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (await TbKhoaBoiDuongTapHuanThamGiaCuaCanBoExists(tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdKhoaBoiDuongTapHuanThamGiaCuaCanBo) == false)
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
                ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdCanBo", tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdCanBo);
                ViewData["IdLoaiBoiDuong"] = new SelectList(await ApiServices_.GetAll<DmLoaiBoiDuong>("/api/dm/LoaiBoiDuong"), "IdLoaiBoiDuong", "LoaiBoiDuong", tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdLoaiBoiDuong);
                ViewData["IdNguonKinhPhi"] = new SelectList(await ApiServices_.GetAll<DmNguonKinhPhi>("/api/dm/NguonKinhPhi"), "IdNguonKinhPhi", "NguonKinhPhi", tbKhoaBoiDuongTapHuanThamGiaCuaCanBo.IdNguonKinhPhi);
                return View(tbKhoaBoiDuongTapHuanThamGiaCuaCanBo);
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
            }

        // GET: KhoaBoiDuongTapHuanThamGiaCuaCanBo/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbKhoaBoiDuongTapHuanThamGiaCuaCanBos = await ApiServices_.GetAll<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo>("/api/cb/KhoaBoiDuongTapHuanThamGiaCuaCanBo");
            var tbKhoaBoiDuongTapHuanThamGiaCuaCanBo = tbKhoaBoiDuongTapHuanThamGiaCuaCanBos.FirstOrDefault(m => m.IdKhoaBoiDuongTapHuanThamGiaCuaCanBo == id);
            if (tbKhoaBoiDuongTapHuanThamGiaCuaCanBo == null)
            {
                return NotFound();
            }

            return View(tbKhoaBoiDuongTapHuanThamGiaCuaCanBo);
        }

        // POST: KhoaBoiDuongTapHuanThamGiaCuaCanBo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo>("/api/cb/KhoaBoiDuongTapHuanThamGiaCuaCanBo", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        private async Task<bool> TbKhoaBoiDuongTapHuanThamGiaCuaCanBoExists(int id)
        {
            var tbKhoaBoiDuongTapHuanThamGiaCuaCanBos = await ApiServices_.GetAll<TbKhoaBoiDuongTapHuanThamGiaCuaCanBo>("/api/cb/KhoaBoiDuongTapHuanThamGiaCuaCanBo");
            return tbKhoaBoiDuongTapHuanThamGiaCuaCanBos.Any(e => e.IdKhoaBoiDuongTapHuanThamGiaCuaCanBo == id);
        }
    }
}

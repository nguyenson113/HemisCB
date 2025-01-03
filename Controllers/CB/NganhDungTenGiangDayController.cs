﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HemisCB.Models;
using HemisCB.Models.DM;
using HemisCB.API;
using Newtonsoft.Json;
using System.Globalization;

namespace HemisCB.Controllers.CB
{
    public class NganhDungTenGiangDayController : Controller
    {
        private readonly ApiServices ApiServices_;

        public NganhDungTenGiangDayController(ApiServices services)
        {
            ApiServices_ = services;
        }

        //==========================================TẠO DANH SÁCH THÔNG TIN TỪ API===========================
        private async Task<List<TbNganhDungTenGiangDay>> TbNganhDungTenGiangDays()
        {
            List<TbNganhDungTenGiangDay> tbNganhDungTenGiangDays = await ApiServices_.GetAll<TbNganhDungTenGiangDay>("/api/cb/NganhDungTenGiangDay");
            List<TbCanBo> tbcanbos = await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo");
            List<DmNganhDaoTao> dmnganhDaoTaos = await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao");
            List<TbNguoi> tbNguois = await ApiServices_.GetAll<TbNguoi>("/api/Nguoi");
            tbNganhDungTenGiangDays.ForEach(item => {
                item.IdCanBoNavigation = tbcanbos.FirstOrDefault(x => x.IdCanBo == item.IdCanBo);
                item.IdNganhDaoTaoNavigation = dmnganhDaoTaos.FirstOrDefault(x => x.IdNganhDaoTao == item.IdNganhDaoTao);
                if (item.IdCanBoNavigation != null)
                    item.IdCanBoNavigation.IdNguoiNavigation = tbNguois.FirstOrDefault(x => x.IdNguoi == item.IdCanBoNavigation.IdNguoi);
            });
            return tbNganhDungTenGiangDays;
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
            List<TbNganhDungTenGiangDay> getall = await TbNganhDungTenGiangDays();
            return View(getall);
        }


        // GET: NganhDungTenGiangDay
        public async Task<IActionResult> Index()
        {
            try
            {
                List<TbNganhDungTenGiangDay> getall = await TbNganhDungTenGiangDays();
                // Lấy data từ các table khác có liên quan (khóa ngoài) để hiển thị trên Index
                return View(getall);
                // Bắt lỗi các trường hợp ngoại lệ
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        // GET: NganhDungTenGiangDay/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Tìm các dữ liệu theo Id tương ứng đã truyền vào view Details
                var tbNganhDungTenGiangDays = await TbNganhDungTenGiangDays();
                var tbNganhDungTenGiangDay = tbNganhDungTenGiangDays.FirstOrDefault(m => m.IdNganhDungTenGiangDay == id);
                // Nếu không tìm thấy Id tương ứng, chương trình sẽ báo lỗi NotFound
                if (tbNganhDungTenGiangDay == null)
                {
                    return NotFound();
                }
                // Nếu đã tìm thấy Id tương ứng, chương trình sẽ dẫn đến view Details
                // Hiển thị thông thi chi tiết CTĐT thành công
                return View(tbNganhDungTenGiangDay);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
        // GET: NganhDungTenGiangDay/Create
  
        public async Task<IActionResult> Create()
        {
            // Tạo danh sách lựa chọn cho IdCanBo, IdChucDanhGiangVien, IdChucVu
          
            ViewData["IdNganhDaoTao"] = new SelectList(await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao"), "IdNganhDaoTao", "NganhDaoTao");
            ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name");
            return View();
        }


        // POST: NganhDungTenGiangDay/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNganhDungTenGiangDay,IdCanBo,IdNganhDaoTao,TrongSo,TenCanBo,TenNganhGiangDay,NgayBatDau,NgayKetThuc")] TbNganhDungTenGiangDay tbNganhDungTenGiangDay)
        {
            if (await TbNganhDungTenGiangDayExists(tbNganhDungTenGiangDay.IdNganhDungTenGiangDay)) ModelState.AddModelError("IdNganhDungTenGiangDay", "ID này đã tồn tại!");
            if (ModelState.IsValid)
            {
                await ApiServices_.Create<TbNganhDungTenGiangDay>("/api/cb/NganhDungTenGiangDay", tbNganhDungTenGiangDay);
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdNganhDaoTao"] = new SelectList(await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao"), "IdNganhDaoTao", "NganhDaoTao", tbNganhDungTenGiangDay.IdNganhDaoTao);
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<TbCanBo>("/api/cb/CanBo"), "IdCanBo", "IdNguoiNavigation.name", tbNganhDungTenGiangDay.IdCanBo);

            return View(tbNganhDungTenGiangDay);
        }

        // GET: NganhDungTenGiangDay/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbNganhDungTenGiangDay = await ApiServices_.GetId<TbNganhDungTenGiangDay>("/api/cb/NganhDungTenGiangDay", id ?? 0);
            if (tbNganhDungTenGiangDay == null)
            {
                return NotFound();
            }
            ViewData["IdCanBo"] = new SelectList(await TbCanBos(), "IdCanBo", "IdNguoiNavigation.name", tbNganhDungTenGiangDay.IdCanBo);
            ViewData["IdNganhDaoTao"] = new SelectList(await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao"), "IdNganhDaoTao", "NganhDaoTao", tbNganhDungTenGiangDay.IdNganhDaoTao);
            return View(tbNganhDungTenGiangDay);
        }

        // POST: NganhDungTenGiangDay/Edit/5
        // To protect from overposting attack
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdNganhDungTenGiangDay,IdCanBo,IdNganhDaoTao,TrongSo,TenCanBo,TenNganhGiangDay,NgayBatDau,NgayKetThuc")] TbNganhDungTenGiangDay tbNganhDungTenGiangDay)
        {
            if (id != tbNganhDungTenGiangDay.IdNganhDungTenGiangDay)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await ApiServices_.Update<TbNganhDungTenGiangDay>("/api/cb/NganhDungTenGiangDay", id, tbNganhDungTenGiangDay);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await TbNganhDungTenGiangDayExists(tbNganhDungTenGiangDay.IdNganhDungTenGiangDay) == false)
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
            ViewData["IdCanBo"] = new SelectList(await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao"), "IdCanBo", "IdCanBo", tbNganhDungTenGiangDay.IdCanBo);
            ViewData["IdNganhDaoTao"] = new SelectList(await ApiServices_.GetAll<DmNganhDaoTao>("/api/dm/NganhDaoTao"), "IdNganhDaoTao", "NganhDaoTao", tbNganhDungTenGiangDay.IdNganhDaoTao);
            return View(tbNganhDungTenGiangDay);
        }

        // GET: NganhDungTenGiangDay/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tbNganhDungTenGiangDays = await TbNganhDungTenGiangDays();
            var tbNganhDungTenGiangDay = tbNganhDungTenGiangDays.FirstOrDefault(m => m.IdNganhDungTenGiangDay == id);
            if (tbNganhDungTenGiangDay == null)
            {
                return NotFound();
            }

            return View(tbNganhDungTenGiangDay);
        }

        // POST: NganhDungTenGiangDay/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await ApiServices_.Delete<TbNganhDungTenGiangDay>("/api/cb/NganhDungTenGiangDay", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        private async Task<bool> TbNganhDungTenGiangDayExists(int id)
        {
            var tbNganhDungTenGiangDays = await ApiServices_.GetAll<TbNganhDungTenGiangDay>("/api/cb/NganhDungTenGiangDay");
            return tbNganhDungTenGiangDays.Any(e => e.IdNganhDungTenGiangDay == id);
        }


        //Import Excel 
        public async Task<IActionResult> Receive(string json)
        {
            try
            {
                // Khai báo thông báo mặc định
                var message = "Không phát hiện lỗi";
                // Giải mã dữ liệu JSON từ client
                List<List<string>> data = JsonConvert.DeserializeObject<List<List<string>>>(json);

                List<TbNganhDungTenGiangDay> lst = new List<TbNganhDungTenGiangDay>();

                // Khởi tạo Random để tạo ID ngẫu nhiên
                Random rnd = new Random();
                List<TbNganhDungTenGiangDay> TbNganhDungTenGiangDays = await ApiServices_.GetAll<TbNganhDungTenGiangDay>("/api/cb/NganhDungTenGiangDay");
                // Duyệt qua từng dòng dữ liệu từ Excel
                foreach (var item in data)
                {
                    TbNganhDungTenGiangDay model = new TbNganhDungTenGiangDay();

                    // Tạo id ngẫu nhiên và kiểm tra xem id đã tồn tại chưa
                    int id;
                    do
                    {
                        id = rnd.Next(1, 100000); // Tạo id ngẫu nhiên
                    } while (lst.Any(t => t.IdNganhDungTenGiangDay == id) || TbNganhDungTenGiangDays.Any(t => t.IdNganhDungTenGiangDay == id)); // Kiểm tra id có tồn tại không

                    // Gán dữ liệu cho các thuộc tính của model
                    model.IdNganhDungTenGiangDay = id; // Gán ID
                    model.IdNganhDaoTao = ParseInt(item[0]);
                    model.TenNganhGiangDay = item[1];
                    model.IdCanBo = ParseInt(item[2]);
                 
                    // Thêm model vào danh sách
                    lst.Add(model);
                }

                // Lưu danh sách vào cơ sở dữ liệu (giả sử có một phương thức tạo đối tượng trong DB)
                foreach (var item in lst)
                {
                    await CreateTbNganhDungTenGiangDay(item); // Giả sử có phương thức tạo dữ liệu vào DB
                }

                return Accepted(Json(new { msg = message }));
            }
            catch (Exception ex)
            {
                // Nếu có lỗi, trả về thông báo lỗi
                return BadRequest(Json(new { msg = ex.Message }));
            }
        }

        private async Task CreateTbNganhDungTenGiangDay(TbNganhDungTenGiangDay item)
        {
            await ApiServices_.Create<TbNganhDungTenGiangDay>("/api/cb/NganhDungTenGiangDay", item);
        }

        private int? ParseInt(string v)
        {
            if (int.TryParse(v, out int result)) // Nếu chuỗi có thể chuyển thành int
            {
                return result; // Trả về giá trị int
            }
            else
            {
                return null; // Nếu không thể chuyển thành int, trả về null
            }
        }
    }
}
